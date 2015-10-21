using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace GamingZone.Hubs
{
    public class LobbyHub : Hub
    {
        private static readonly List<UserConnection> m_connections = new List<UserConnection>();

        public async Task Join()
        {
            var name = Clients.Caller.userName;
            var group = ((long)Clients.Caller.lobbyId).ToString();

            m_connections.Add(new UserConnection(name, Context.ConnectionId, group));

            await Groups.Add(Context.ConnectionId, group);

            var users = m_connections.Where(c => c.GroupId == group).Select(c => c.Name);
            Clients.Group(group).UserJoined(name, users);
        }

        public void SendMessage(string message)
        {
            var userConnection = m_connections.FirstOrDefault(g => g.ConnectionId == Context.ConnectionId);
            if (userConnection == null)
                return;

            Clients.Group(userConnection.GroupId).MessageAdded(userConnection.Name, message);
        }

        public void SelectSeat(int tableIndex, int seatIndex)
        {
            var userConnection = m_connections.FirstOrDefault(g => g.ConnectionId == Context.ConnectionId);
            if (userConnection == null)
                return;

            // Need to add some state somewhere.
            Clients.Group(userConnection.GroupId).UserSat(tableIndex, seatIndex, userConnection.Name);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var userConnection = m_connections.FirstOrDefault(g => g.ConnectionId == Context.ConnectionId);
            if (userConnection == null)
                return Task.FromResult(true);

            m_connections.Remove(userConnection);

            var users = m_connections.Where(c => c.GroupId == userConnection.GroupId).Select(c => c.Name);
            Clients.Group(userConnection.GroupId).UserLeft(userConnection.Name, users);

            return base.OnDisconnected(stopCalled);
        }

        private class UserConnection
        {
            public UserConnection(string name, string connectionId, string groupId)
            {
                Name = name;
                ConnectionId = connectionId;
                GroupId = groupId;
            }

            public string Name { get; private set; }
            public string ConnectionId { get; private set; }
            public string GroupId { get; private set; }
        }
    }
}