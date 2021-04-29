using System.Reflection;

using Microsoft.EntityFrameworkCore;

namespace Simple_Youtube_CLI
{
    public class Database : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public DbSet<Video> Videos { get; set; }

        public DbSet<Like> Likes { get; set; }

        public DbSet<Dislike> Dislikes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite(@$"Data Source={System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\..\..\..\{Constants.PersistencePath}");
        }
    }
}
