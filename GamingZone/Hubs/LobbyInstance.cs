using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GamingZone.Hubs
{
    internal class LobbyInstance
    {
        public LobbyInstance(string lobbyId, int numberOfTables, int seatsPerTable)
        {
            LobbyId = lobbyId;

            Tables = new List<Table>();

            for (int tableIndex = 0; tableIndex < numberOfTables; tableIndex++)
                Tables.Add(new Table(seatsPerTable));
        }

        public string LobbyId { get; internal set; }
        public List<Table> Tables { get; private set; }

        internal bool AddUserToSeat(UserConnection userConnection, int tableIndex, int seatIndex)
        {
            // TODO: Validate the indexes

            var table = Tables[tableIndex];

            // Make sure we don't sit on a table we're already present on.
            if (table.ContainsUser(userConnection))
                return false;

            // Make sure we don't sit on top of someone!
            var seat = table.Seats[seatIndex];
            if (seat.User != null)
                return false;

            seat.User = userConnection;
            return true;
        }

        internal void RemoveUserFromAllSeats(UserConnection userConnection, Action<Removal> removal)
        {
            for (int tableIndex = 0; tableIndex < Tables.Count; tableIndex++)
            {
                var table = Tables[tableIndex];
                for (int seatIndex = 0; seatIndex < table.Seats.Count; seatIndex++)
                {
                    var seat = table.Seats[seatIndex];

                    if (seat.User != userConnection)
                        continue;

                    seat.User = null;
                    seat.ThumbsUp = false; // This needs to be evented

                    removal(new Removal(tableIndex, seatIndex));
                }
            }
        }
    }
    internal class Table
    {
        public Table(int numberOfSeats)
        {
            Seats = new List<Seat>();

            for (int tableIndex = 0; tableIndex < numberOfSeats; tableIndex++)
                Seats.Add(new Seat());
        }

        public List<Seat> Seats { get; private set; }

        internal bool ContainsUser(UserConnection userConnection)
        {
            return Seats.Any(s => s.User == userConnection);
        }
    }

    internal class Seat
    {
        public bool ThumbsUp { get; set; }
        public UserConnection User { get; set; }
    }

    internal class Removal
    {

        public Removal(int tableIndex, int seatIndex)
        {
            TableIndex = tableIndex;
            SeatIndex = seatIndex;
        }

        public int SeatIndex { get; private set; }
        public int TableIndex { get; private set; }
    }
}