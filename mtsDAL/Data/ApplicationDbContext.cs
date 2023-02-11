using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using mtsDAL.Models;
using mtsDAL.ViewModels;
using System.IO;

namespace mtsDAL.Data
{
    
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public IConfiguration _configuration { get; }
        public string ConnectionString { get; set; }
        public ApplicationDbContext(IConfiguration configuration, DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            _configuration = configuration;
        }

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConnectionString = _configuration.GetConnectionString("DefaultConnection");
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = ConnectionString };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);
            optionsBuilder.UseSqlite(connection);
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserProfile>().HasNoKey().ToView("UserProfile").HasNoKey();
        }
    }
}
