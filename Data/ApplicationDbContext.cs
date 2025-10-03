using nplBackEnd.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace nplBackEnd.Data;
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Analysis> Analyses => Set<Analysis>();
        public DbSet<LibraryKeyword> LibraryKeywords => Set<LibraryKeyword>();
        public DbSet<AnalysisResult> AnalysisResults => Set<AnalysisResult>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Garante que cada palavra-chave na biblioteca seja única
            modelBuilder.Entity<LibraryKeyword>()
                .HasIndex(lk => lk.Keyword)
                .IsUnique();
        }
    }
