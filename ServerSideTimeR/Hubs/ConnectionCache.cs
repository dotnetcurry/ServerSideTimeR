using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServerSideTimeR.Models;
using System.Timers;
using SignalR;
using System.Threading.Tasks;

namespace ServerSideTimeR.Hubs
{
    public sealed class ConnectionCache
    {

        private static readonly ConnectionCache instance
            = new ConnectionCache();

        private Dictionary<string, UserCredential> _connections;
        private Timer _timer;

        static ConnectionCache()
        {

        }

        private ConnectionCache()
        {
            _connections = new Dictionary<string, UserCredential>();
            _timer = new Timer();
            _timer.Interval = 1000;
            _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            _timer.AutoReset = true;
        }

        public static ConnectionCache Instance
        {
            get
            {
                return instance;
            }
        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            PingClients();
        }

        private void PingClients()
        {
            var hubContext =
                    GlobalHost
                        .ConnectionManager
                            .GetHubContext<ServerSideTimerHub>();
            foreach (var item in _connections.Values)
            {
                TimeSpan span =
                    new TimeSpan(item.GetSessionLengthInTicks());
                ConnectionSession session =
                    item.Sessions[item.Sessions.Count - 1];
                hubContext.Clients[session.ConnectionId]
                    .tick(session.ConnectionId, item.UserId, span.ToString());
            }
        }

        private void CreateNewUserSession(string userId, string connectionId)
        {
            UserCredential currentCred = new UserCredential
            {
                ConnectionStatus = ConnectionStatus.Connected,
                UserId = userId
            };
            currentCred.Sessions.Add(new ConnectionSession
            {
                ConnectionId = connectionId,
                ConnectedTime = DateTime.Now.Ticks,
                DisconnectedTime = 0L,
                ParentUser = currentCred
            });
            _connections.Add(userId, currentCred);
        }

        private void UpdateUserSession(string userId, 
            string connectionId, ConnectionStatus status)
        {
            UserCredential currentCred = _connections[userId];

            ExpireSession(currentCred);
            currentCred.Sessions.Add(new ConnectionSession
            {
                ConnectionId = connectionId,
                ConnectedTime = DateTime.Now.Ticks,
                DisconnectedTime = 0L,
                ParentUser = currentCred
            });
            currentCred.ConnectionStatus = status;
        }

        private static void ExpireSession(UserCredential currentCred)
        {
            ConnectionSession currentSession =
                currentCred.Sessions.Find(sess =>
                    sess.DisconnectedTime == 0);
            if (currentSession != null
                && currentSession.DisconnectedTime == 0)
            {
                currentSession.DisconnectedTime = DateTime.Now.Ticks;
            }
        }

        internal void UpdateCache(string userId,
            string connectionId, ConnectionStatus status)
        {
            if (_connections.ContainsKey(userId)
                && !string.IsNullOrEmpty(userId))
            {
                UpdateUserSession(userId, connectionId, status);
            }
            else
            {
                if (!_timer.Enabled)
                {
                    _timer.Enabled = true;
                    _timer.Start();
                }
                CreateNewUserSession(userId, connectionId);
            }
        }

        internal void Disconnect(string connectionId)
        {
            ConnectionSession session = null;
            if (_connections.Values.Count > 0)
            {
                foreach (var currentCredi in _connections.Values)
                {
                    session = currentCredi.Sessions.Find(ss =>
                        ss.ConnectionId == connectionId);
                    if (session != null)
                    {
                        session.DisconnectedTime = DateTime.Now.Ticks;
                        break;
                    }
                }
            }
        }

        internal void Logout(string userId)
        {
            ExpireSession(this._connections[userId]);
            // Save to DB If required
            this._connections.Remove(userId);
            if (this._connections.Count == 0)
            {
                _timer.Enabled = false;
                _timer.Stop();
            }
        }
    }


}