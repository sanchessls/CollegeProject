using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ScrumPokerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPokerAPI.Context
{
    public class ApplicationContext : IdentityDbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {     
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<PlanningSessionUser>()
                        .HasKey(pc => new { pc.Id });

            modelBuilder.Entity<PlanningSessionUser>()
                .HasOne(pc => pc.User)
                .WithMany(p => p.PlanningSessionUser)
                .HasForeignKey(pc => pc.UserId);

            modelBuilder.Entity<PlanningSessionUser>()
                .HasOne(pc => pc.PlanningSession)
                .WithMany(c => c.PlanningSessionUser)
                .HasForeignKey(pc => pc.PlanningSessionId); 


            modelBuilder.Entity<FeatureUser>()
                        .HasKey(pc => new { pc.Id });

            modelBuilder.Entity<FeatureUser>()
                .HasOne(pc => pc.User)
                .WithMany(p => p.FeatureUser)
                .HasForeignKey(pc => pc.UserId);

            modelBuilder.Entity<FeatureUser>()
                .HasOne(pc => pc.Feature)
                .WithMany(c => c.FeatureUser)
                .HasForeignKey(pc => pc.FeatureId);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

        }


        //Here we set the Objects that are a table in our Database Context
        public DbSet<PlanningSession> PlanningSession { get; set; }
        public DbSet<PlanningSessionUser> PlanningSessionUser { get; set; }
        public DbSet<Feature> Feature { get; set; }

        public DbSet<FeatureUser> FeatureUser { get; set; }




    }
}
