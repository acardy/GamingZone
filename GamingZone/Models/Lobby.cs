using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingZone.Models
{
    public class Lobby
    {
        public string Id { get; internal set; }
        public string Name { get; internal set; }
        public int SeatsPerTable { get; internal set; }
        public int Tables { get; internal set; }
    }
}
