using ChessOnlineWebApp.GamesManagementServiceReference;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ChessOnlineWebApp
{
    public class GameHub : Hub
    {

        public void StartGame(string game_topic)
        {
            Groups.Add(Context.ConnectionId, game_topic.Substring(1));
            Clients.Caller.playingAs(game_topic[0]);
        }

        public void ResignGame(string game_topic)
        {
            Clients.OthersInGroup(game_topic.Substring(1)).playerResigned();
        }

        public void EndGame(string game_topic)
        {
            Groups.Remove(Context.ConnectionId, game_topic.Substring(1));
        }

        public void SendMove(string game_topic, string move)
        {
            Clients.OthersInGroup(game_topic.Substring(1)).playMove(move);
        }
    }
}