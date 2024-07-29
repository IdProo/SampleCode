using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SampleCore.Data;
using System.IO;

namespace SampleCore
{
    public class Context : DbContext
    {
        public static IConfigurationRoot Configuration { get; set; }
        public Context()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json");
            // define builder
            Configuration = builder.Build();
        }
        public DbSet<UsersTable> Users { get; set; }
       

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseSqlite(
                    Configuration["ConnectionStrings:DefaultConnection"]);


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

    }
}
