using GamingZone.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GamingZone.Data
{
    public partial class ApplicationDbContext
    {
        public DbSet<Lobby> Lobbies { get; set; }
    }
}