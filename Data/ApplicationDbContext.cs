using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using TrainApp.Data.Models;


namespace TrainApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Parameters> Parameters { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<TeamUser>  TeamUsers { get; set; }
        public DbSet<Training> Training { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TeamUser>()
               .HasKey(tu => new { tu.UserId, tu.TeamId });

            builder.Entity<TeamUser>()
                .HasOne(tu => tu.ApplicationUser)
                .WithMany(au => au.TeamUser)
                .HasForeignKey(tu => tu.UserId);

            builder.Entity<TeamUser>()
                .HasOne(tu => tu.Team)  
                .WithMany(t => t.TeamUser)  
                .HasForeignKey(tu => tu.TeamId);

            builder.Entity<Training>()
               .HasOne(t => t.Team)  
               .WithMany(t => t.Training)  
               .HasForeignKey(t => t.TeamId);
        }
    }
}
