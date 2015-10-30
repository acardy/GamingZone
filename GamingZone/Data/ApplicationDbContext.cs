using GamingZone.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace GamingZone.Data
{
    public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext 
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}