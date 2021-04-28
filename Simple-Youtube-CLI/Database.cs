using Microsoft.EntityFrameworkCore;

namespace Simple_Youtube_CLI
{
    public class Database : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public DbSet<Video> Videos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite(@$"Data Source={Constants.PersistencePath}");
        }
    }
}
