using Microsoft.EntityFrameworkCore;
using projetStage.Models;

namespace projetStage.Data
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) {}
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Acheteur> Acheteurs { get; set; }
        public DbSet<Demandeur> Demandeurs { get; set; }
        public DbSet<Validateur> Validateurs { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>().HasIndex(u => u.Code).IsUnique();
            modelBuilder.Entity<Acheteur>().HasIndex(u => u.Code).IsUnique();
            modelBuilder.Entity<Demandeur>().HasIndex(u => u.Code).IsUnique();
            modelBuilder.Entity<Validateur>().HasIndex(u => u.Code).IsUnique();

            modelBuilder.Entity<Admin>().Property(u => u.Departement).HasMaxLength(4);
            modelBuilder.Entity<Acheteur>().Property(u => u.Departement).HasMaxLength(4);
            modelBuilder.Entity<Demandeur>().Property(u => u.Departement).HasMaxLength(4);
            modelBuilder.Entity<Validateur>().Property(u => u.Departement).HasMaxLength(4);

            modelBuilder.Entity<Admin>().Property(u => u.Role).HasMaxLength(1);
            modelBuilder.Entity<Acheteur>().Property(u => u.Role).HasMaxLength(1);
            modelBuilder.Entity<Demandeur>().Property(u => u.Role).HasMaxLength(1);
            modelBuilder.Entity<Validateur>().Property(u => u.Role).HasMaxLength(1);

            modelBuilder.Entity<Admin>().HasKey(u => u.Id);
            modelBuilder.Entity<Admin>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Acheteur>().HasKey(u => u.Id);
            modelBuilder.Entity<Acheteur>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Demandeur>().HasKey(u => u.Id);
            modelBuilder.Entity<Demandeur>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Validateur>().HasKey(u => u.Id);
            modelBuilder.Entity<Validateur>().Property(u => u.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<PasswordResetToken>().HasIndex(t => t.Token).IsUnique();
        }
    }
}
