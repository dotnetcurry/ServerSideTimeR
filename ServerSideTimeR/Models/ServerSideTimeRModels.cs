using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerSideTimeR.Models
{
    public class UserCredential
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ConnectionStatus ConnectionStatus { get; set; }
        public List<ConnectionSession> Sessions { get; set; }
        public long GetSessionLengthInTicks()
        {
            long totalSession = 0;
            foreach (var session in Sessions)
            {
                if (session.DisconnectedTime != 0)
                {
                    totalSession += session.DisconnectedTime - session.ConnectedTime;
                }
                else
                {
                    totalSession += DateTime.Now.Ticks - session.ConnectedTime;
                }
            }
            return totalSession;
        }

        public UserCredential()
        {
            Sessions = new List<ConnectionSession>();
        }
    }

    public class ConnectionSession
    {
        public int Id { get; set; }
        public string ConnectionId { get; set; }
        public long ConnectedTime { get; set; }
        public long DisconnectedTime { get; set; }
        public UserCredential ParentUser { get; set; }
    }

    public enum ConnectionStatus
    {
        Connected,
        Disconnected,
        Refreshed
    }
}