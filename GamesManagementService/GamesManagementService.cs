using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace GamesManagementService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "GamesManagementService" in both code and config file together.
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class GamesManagementService : IGamesManagementService
    {
        void IGamesManagementService.SaveGame(Game game, string token)
        {
            try
            {
                AuthorizationServiceReference.User user;
                using (AuthorizationServiceReference.AuthorizationServiceClient client = new AuthorizationServiceReference.AuthorizationServiceClient())
                {
                    user = client.AuthorizeUser(token);
                }

                string connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand("INSERT INTO SavedGames (user_id, game_string, played_as) VALUES (@user_id, @game_string, @played_as)", conn);
                    command.Parameters.AddWithValue("@user_id", user.Id);
                    command.Parameters.AddWithValue("@game_string", game.GameString);
                    command.Parameters.AddWithValue("@played_as", game.PlayedAs == Game.Player.White ? 'W' : 'B');
                    conn.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (FaultException<AuthorizationServiceReference.AuthorizationFault> ex)
            {
                if (ex.Detail.FaultType == AuthorizationServiceReference.AuthorizationFault.AuthorizationFaultType.TokenExpired)
                {
                    throw new FaultException<GamesManagementFault>(new GamesManagementFault(GamesManagementFault.GamesManagementFaultType.TokenExpired));
                }
                else
                {
                    throw new FaultException<GamesManagementFault>(new GamesManagementFault(GamesManagementFault.GamesManagementFaultType.InvalidSignature));
                }
            }
            catch (Exception)
            {
                throw new FaultException<GamesManagementFault>(new GamesManagementFault(GamesManagementFault.GamesManagementFaultType.ServerFault));
            }
        }

        List<Game> IGamesManagementService.GetAllSavedGames(string token)
        {
            try
            {
                AuthorizationServiceReference.User user;
                using (AuthorizationServiceReference.AuthorizationServiceClient client = new AuthorizationServiceReference.AuthorizationServiceClient())
                {
                    user = client.AuthorizeUser(token);
                }

                string connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
                List<Game> games = new List<Game>();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand("SELECT _id, game_string, played_as FROM SavedGames WHERE user_id = @user_id", conn);
                    command.Parameters.AddWithValue("@user_id", user.Id);
                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        games.Add(new Game((Int64)reader["_id"], (string)reader["game_string"], (string)reader["played_as"] == "W" ? Game.Player.White : Game.Player.Black));
                    }
                }

                return games;
            }
            catch (FaultException<AuthorizationServiceReference.AuthorizationFault> ex)
            {
                if (ex.Detail.FaultType == AuthorizationServiceReference.AuthorizationFault.AuthorizationFaultType.TokenExpired)
                {
                    throw new FaultException<GamesManagementFault>(new GamesManagementFault(GamesManagementFault.GamesManagementFaultType.TokenExpired));
                }
                else
                {
                    throw new FaultException<GamesManagementFault>(new GamesManagementFault(GamesManagementFault.GamesManagementFaultType.InvalidSignature));
                }
            }
            catch (Exception)
            {
                throw new FaultException<GamesManagementFault>(new GamesManagementFault(GamesManagementFault.GamesManagementFaultType.ServerFault));
            }
        }

        void IGamesManagementService.DeleteGame(Game game, string token)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;

                using (AuthorizationServiceReference.AuthorizationServiceClient client = new AuthorizationServiceReference.AuthorizationServiceClient())
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    client.AuthorizeUser(token);

                    SqlCommand command = new SqlCommand("DELETE FROM SavedGames WHERE _id = @_id", conn);
                    command.Parameters.AddWithValue("@_id", game.GameId);
                    conn.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (FaultException<AuthorizationServiceReference.AuthorizationFault> ex)
            {
                if (ex.Detail.FaultType == AuthorizationServiceReference.AuthorizationFault.AuthorizationFaultType.TokenExpired)
                {
                    throw new FaultException<GamesManagementFault>(new GamesManagementFault(GamesManagementFault.GamesManagementFaultType.TokenExpired));
                }
                else
                {
                    throw new FaultException<GamesManagementFault>(new GamesManagementFault(GamesManagementFault.GamesManagementFaultType.InvalidSignature));
                }
            }
            catch (Exception)
            {
                throw new FaultException<GamesManagementFault>(new GamesManagementFault(GamesManagementFault.GamesManagementFaultType.ServerFault));
            }
        }
        
        string IGamesManagementService.FindMatch(string token)
        {
            string game_topic = null;
            try
            {
                using (AuthorizationServiceReference.AuthorizationServiceClient client = new AuthorizationServiceReference.AuthorizationServiceClient())
                {
                    AuthorizationServiceReference.User player = client.AuthorizeUser(token);
                    ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings.Get("redis_connection_string"));
                    RedisKey playerListKey = new RedisKey("PlayerList");
                    IDatabase database = redis.GetDatabase();
                    string userID = Convert.ToString(player.Id);
                    database.ListRightPush(playerListKey, userID);
                    ISubscriber subscriber = redis.GetSubscriber();
                    subscriber.Publish("PlayerAddEvents", userID);
                    for (int i = 0; i < 60; i++)
                    {
                        Thread.Sleep(500);
                        game_topic = database.StringGet(userID);
                        database.KeyDelete(userID);
                        if (game_topic != null)
                        {
                            break;
                        }
                    }
                    database.ListRemove(playerListKey, userID);
                    if (game_topic == null)
                    {
                        throw new TimeoutException();
                    }
                    return game_topic;
                }
            }
            catch (FaultException<AuthorizationServiceReference.AuthorizationFault> ex)
            {
                if (ex.Detail.FaultType == AuthorizationServiceReference.AuthorizationFault.AuthorizationFaultType.TokenExpired)
                {
                    throw new FaultException<GamesManagementFault>(new GamesManagementFault(GamesManagementFault.GamesManagementFaultType.TokenExpired));
                }
                else
                {
                    throw new FaultException<GamesManagementFault>(new GamesManagementFault(GamesManagementFault.GamesManagementFaultType.InvalidSignature));
                }
            }
            catch (TimeoutException)
            {
                throw new TimeoutException();
            }
            catch (Exception)
            {
                throw new FaultException<GamesManagementFault>(new GamesManagementFault(GamesManagementFault.GamesManagementFaultType.ServerFault));
            }
        }
    }
}
