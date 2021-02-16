using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace GamesManagementService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "GamesManagementService" in both code and config file together.
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
            catch (Exception ex)
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
            catch (Exception ex)
            {
                throw new FaultException<GamesManagementFault>(new GamesManagementFault(GamesManagementFault.GamesManagementFaultType.ServerFault));
            }
        }
    }
}
