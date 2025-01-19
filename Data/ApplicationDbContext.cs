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
        public DbSet<Exercise> Exercise { get; set; }
        public DbSet<UserExercise> UserExercise { get; set; }
        public DbSet<Material> Material { get; set; }
        public DbSet<Notes> Notes { get; set; }

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
                .HasForeignKey(tu => tu.TeamId)
                .OnDelete(DeleteBehavior.Cascade); 

            builder.Entity<Training>()
               .HasOne(t => t.Team)  
               .WithMany(t => t.Training)  
               .HasForeignKey(t => t.TeamId)
               .OnDelete(DeleteBehavior.Cascade); 

            builder.Entity<Exercise>()
                .HasOne(e => e.Team)
                .WithMany(t => t.Exercise)
                .HasForeignKey(e => e.TeamId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);


            builder.Entity<UserExercise>()
                .HasKey(tu => new { tu.UserId, tu.ExerciseId });

            builder.Entity<UserExercise>()
                .HasOne(tu => tu.ApplicationUser)
                .WithMany(au => au.UserExercise)
                .HasForeignKey(tu => tu.UserId);


            builder.Entity<UserExercise>()
                .HasOne(tu => tu.Exercise)
                .WithMany(t => t.UserExercise)
                .HasForeignKey(tu => tu.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Material>()
                .HasOne(m => m.Exercise)
                .WithMany(t => t.Material)
                .HasForeignKey(t => t.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Notes>()
                .HasOne(n => n.User)
                .WithMany(t => t.Notes)
                .HasForeignKey(n => n.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Team>()
                .Property(e => e.TeamId)
                .ValueGeneratedOnAdd();

            builder.Entity<Exercise>()
                .Property(e => e.ExerciseId)
                .ValueGeneratedOnAdd();

            builder.Entity<Parameters>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Material>()
                .Property(e => e.MaterialId)
                .ValueGeneratedOnAdd();

            builder.Entity<Training>()
                .Property(e => e.TrainingId)
                .ValueGeneratedOnAdd();

            builder.Entity<Notes>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
