using Continental.CUP.Repositories.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace Continental.CUP.Repositories.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            IConfigurationBuilder build = new ConfigurationBuilder();

            build.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var config = build.Build();
            string connString = config.GetConnectionString("DefaultConnection");
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connString, sqlServerOptions => sqlServerOptions.CommandTimeout(180));
            }
            optionsBuilder.UseLoggerFactory(
                new LoggerFactory(
                    new[] {
                        new ConsoleLoggerProvider (
                            (category, level) =>
                            category == DbLoggerCategory.Database.Command.Name &&
                            level == LogLevel.Information, true
                        )
                    }
                )
            );

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            //TODO: Validar que funciones e implementar que cuando seas administrador se muestren tambien los inactivos
            modelBuilder.Entity<UserEModel>().HasQueryFilter(x => x.Status == 1);
        }

        public DbSet<UserEModel> Users { get; set; }
        public DbSet<OrderEModel> Order { get; set; }
        public DbSet<ReaderEModel> Reader { get; set; }
        public DbSet<OrderDetailEModel> OrderDetail { get; set; }
        public DbSet<ReaderKindEModel> ReaderKind { get; set; }
        public DbSet<ActionDetailEModel> ActionDetail { get; set; }
        public DbSet<ActionEModel> Action { get; set; }
        public DbSet<RoleActionEModel> RoleAction { get; set; }
        public DbSet<RoleEModel> Role { get; set; }
        public DbSet<UserRoleEModel> UserRole { get; set; }
        public DbSet<LocationEModel> Location { get; set; }
    }
}
