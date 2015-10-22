using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GamingZone.Hubs
{
    internal class UserConnection
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