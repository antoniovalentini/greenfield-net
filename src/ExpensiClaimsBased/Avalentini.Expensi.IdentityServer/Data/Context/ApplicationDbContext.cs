using Avalentini.Expensi.IdentityServer.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Avalentini.Expensi.IdentityServer.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }

        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}
