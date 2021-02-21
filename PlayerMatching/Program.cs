using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PlayerMatching
{
    class Program
    {
        static ManualResetEvent manualResetEvent = new ManualResetEvent(false);

        static void Unblock()
        {
            manualResetEvent.Set();
        } 
        static void Main(string[] args)
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings.Get("redis_connection_string"));
            ISubscriber subscriber = redis.GetSubscriber();
            subscriber.Subscribe("PlayerAddEvents", (channel, message) =>
            {
                Unblock();
            });

            RedisKey playerListKey = new RedisKey("PlayerList");
            IDatabase database = redis.GetDatabase();
            Random random = new Random(DateTime.Now.Second);

            while (true)
            {
                manualResetEvent.Reset();
                manualResetEvent.WaitOne();
                long playerListLength = database.ListLength(playerListKey);

                if (playerListLength > 1)
                {
                    RedisValue player1 = database.ListLeftPop(playerListKey);
                    RedisValue player2 = database.ListLeftPop(playerListKey);
                    string game_topic = random.Next() + "_" + player1.ToString() + player2.ToString() + "_" + random.Next();
                    database.StringSet(player1.ToString(), "W" + game_topic);
                    database.StringSet(player2.ToString(), "B" + game_topic);
                }
            }
        }
    }
}
