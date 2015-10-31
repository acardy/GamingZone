using GamingZone.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GamingZone.Data
{
    public class ApplicationDbInitializer : System.Data.Entity.DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            // Create lobbies
            context.Lobbies.AddRange(new List<Lobby>
            {
                new Lobby() { Id = "1", Name = "Fifteen Two From", Tables = 250, SeatsPerTable = 2 },
                new Lobby() { Id = "2", Name = "Peg Pals From", Tables = 250, SeatsPerTable = 2 }
            });

            // Create a user?

            context.SaveChanges();
        }
    }
}