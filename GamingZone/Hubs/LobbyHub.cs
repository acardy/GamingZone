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
        private static readonly List<LobbyInstance> m_lobbyTables = new List<LobbyInstance>();

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

            LobbyInstance lobbyInstance = GetLobbyInstance(userConnection);
            var userSat = lobbyInstance.AddUserToSeat(userConnection, tableIndex, seatIndex);

            // Check if they were able to sit
            if (!userSat)
                return;
            
            Clients.Group(userConnection.GroupId).UpdateSeat(tableIndex, seatIndex, userConnection.Name, false);
        }

        public void ExitSeat(int tableIndex, int seatIndex)
        {
            var userConnection = m_connections.FirstOrDefault(g => g.ConnectionId == Context.ConnectionId);
            if (userConnection == null)
                return;

            LobbyInstance lobbyInstance = GetLobbyInstance(userConnection);
            var userStood = lobbyInstance.RemoveUserFromSeat(userConnection, tableIndex, seatIndex);

            // Check if they were able to stand
            if (!userStood)
                return;
            
            Clients.Group(userConnection.GroupId).UpdateSeat(tableIndex, seatIndex, userConnection.Name, false);
        }

        private static LobbyInstance GetLobbyInstance(UserConnection userConnection)
        {
            var lobbyInstance = m_lobbyTables.FirstOrDefault(lt => lt.LobbyId == userConnection.GroupId);
            if (lobbyInstance == null)
            {
                lobbyInstance = new LobbyInstance(userConnection.GroupId, 250, 2);
                m_lobbyTables.Add(lobbyInstance);
            }

            return lobbyInstance;
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var userConnection = m_connections.FirstOrDefault(g => g.ConnectionId == Context.ConnectionId);
            if (userConnection == null)
                return Task.FromResult(true);

            m_connections.Remove(userConnection);

            // Remove user from all tables
            LobbyInstance lobbyInstance = GetLobbyInstance(userConnection);

            lobbyInstance.RemoveUserFromAllSeats(userConnection, r =>
            {
                // Fire events
                Clients.Group(userConnection.GroupId).UserStood(r.TableIndex, r.SeatIndex, userConnection.Name);
            });

            // New user list
            var users = m_connections.Where(c => c.GroupId == userConnection.GroupId).Select(c => c.Name);
            Clients.Group(userConnection.GroupId).UserLeft(userConnection.Name, users);

            return base.OnDisconnected(stopCalled);
        }
    }
}