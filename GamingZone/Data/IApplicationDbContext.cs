using GamingZone.Models;
using System.Data.Entity;

namespace GamingZone.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Lobby> Lobbies { get; set; }
    }
}