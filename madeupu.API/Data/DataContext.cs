﻿using madeupu.API.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace madeupu.API.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<ParticipationType> ParticipationTypes { get; set; }
        public DbSet<ProjectCategory> ProjectCategories { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Region> Regions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<Region>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<ParticipationType>().HasIndex(x => x.Description).IsUnique();
            modelBuilder.Entity<ProjectCategory>().HasIndex(x => x.Description).IsUnique();
            modelBuilder.Entity<DocumentType>().HasIndex(x => x.Description).IsUnique();
            
        }
    }
}
