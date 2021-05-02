using Avalentini.Expensi.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Avalentini.Expensi.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ExpenseEntity> Expenses { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}
