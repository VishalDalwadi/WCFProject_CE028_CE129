using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ChessOnlineWebApp
{
    public class GameHub : Hub
    {
        private GamesManagementServiceReference.GamesManagementServiceClient _gms_client;

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public void Hello()
        {
            Clients.All.hello();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            _gms_client.Close();
            return base.OnDisconnected(stopCalled);
        }
    }
}