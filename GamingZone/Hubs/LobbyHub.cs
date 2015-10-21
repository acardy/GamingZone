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
        private static readonly List<LobbyTables> m_lobbyTables = new List<LobbyTables>();

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

            var lobbyTable = m_lobbyTables.FirstOrDefault(lt => lt.LobbyId == userConnection.GroupId);
            if (lobbyTable == null)
            {
                lobbyTable = new LobbyTables(userConnection.GroupId, 250, 2);
                m_lobbyTables.Add(lobbyTable);
            }

            var seat = lobbyTable.Tables[tableIndex].Seats[seatIndex];

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

        private class LobbyTables
        {

            public LobbyTables(string lobbyId, int numberOfTables, int seatsPerTable)
            {
                LobbyId = lobbyId;

                Tables = new List<Table>();

                for (int tableIndex = 0; tableIndex < numberOfTables; tableIndex++)
                    Tables.Add(new Table(seatsPerTable));
            }

            public string LobbyId { get; internal set; }
            public List<Table> Tables { get; private set; }
        }

        private class Table
        {
            public Table(int numberOfSeats)
            {
                Seats = new List<Seat>();

                for (int tableIndex = 0; tableIndex < numberOfSeats; tableIndex++)
                    Seats.Add(new Seat());
            }

            public List<Seat> Seats { get; private set; }
        }

        private class Seat
        {
            public bool ThumbsUp { get; set; }
            public UserConnection User { get; set; }
        }
    }

}