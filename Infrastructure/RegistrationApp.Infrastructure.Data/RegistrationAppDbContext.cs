using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using RegistrationApp.Entities;

namespace RegistrationApp.Infrastructure.Data
{
    public class RegistrationAppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Province> Provinces { get; set; }

        public RegistrationAppDbContext(DbContextOptions<RegistrationAppDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Index for Email. Often is used during email validation
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Index for CountryId. Often is used to filter Provinces by country
            modelBuilder.Entity<Province>()
                .HasIndex(p => p.CountryId);

            // Seed data
            modelBuilder.Entity<Country>().HasData(
                new Country { Id = 1, Name = "USA" },
                new Country { Id = 2, Name = "Canada" }
            );

            modelBuilder.Entity<Province>().HasData(
                new Province { Id = 1, Name = "California", CountryId = 1 },
                new Province { Id = 2, Name = "Texas", CountryId = 1 },
                new Province { Id = 3, Name = "Ontario", CountryId = 2 },
                new Province { Id = 4, Name = "Quebec", CountryId = 2 }
            );
        }
    }
}
