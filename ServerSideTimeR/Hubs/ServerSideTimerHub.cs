using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalR.Hubs;
using ServerSideTimeR.Models;
using System.Threading.Tasks;

namespace ServerSideTimeR.Hubs
{
    public class ServerSideTimerHub : Hub, IDisconnect, IConnected
    {
        public Task Disconnect()
        {
            ConnectionCache.Instance.Disconnect(Context.ConnectionId);
            return Clients.leave(Context.ConnectionId,
                DateTime.Now.ToString());
        }

        public Task Connect()
        {
            string connectionName = Context.ConnectionId;
            if (Context.User != null && Context.User.Identity != null
                && Context.User.Identity.IsAuthenticated)
            {
                ConnectionCache.Instance.UpdateCache(
                    Context.User.Identity.Name,
                    Context.ConnectionId,
                    ConnectionStatus.Connected);
                connectionName = Context.User.Identity.Name;
            }
            return Clients[Context.ConnectionId].joined(connectionName,
                DateTime.Now.ToString());
        }

        public Task Reconnect(IEnumerable<string> groups)
        {
            string connectionName = Context.ConnectionId;
            if (!string.IsNullOrEmpty(Context.User.Identity.Name))
            {
                ConnectionCache.Instance.UpdateCache(
                    Context.User.Identity.Name,
                    Context.ConnectionId,
                    ConnectionStatus.Connected);
                connectionName = Context.User.Identity.Name;
            }
            return Clients[Context.ConnectionId].rejoined(connectionName,
                DateTime.Now.ToString());
        }
    }
}