// Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using projetStage.Models;

namespace projetStage.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Acheteur> Acheteurs { get; set; }
        public DbSet<Demandeur> Demandeurs { get; set; }
        public DbSet<Validateur> Validateurs { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        public DbSet<Demande> Demandes { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<DemandeArticle> DemandeArticles { get; set; }
        public DbSet<Devis> Devis { get; set; }
        public DbSet<Fournisseur> Fournisseurs { get; set; }
        public DbSet<DemandeHistory> DemandeHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>().HasIndex(u => u.Code).IsUnique();
            modelBuilder.Entity<Acheteur>().HasIndex(u => u.Code).IsUnique();
            modelBuilder.Entity<Demandeur>().HasIndex(u => u.Code).IsUnique();
            modelBuilder.Entity<Validateur>().HasIndex(u => u.Code).IsUnique();
            modelBuilder.Entity<Demande>().HasIndex(d => d.Code).IsUnique();

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
            modelBuilder.Entity<Demande>().HasKey(u => u.Id);
            modelBuilder.Entity<Demande>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Article>().HasKey(u => u.Id);
            modelBuilder.Entity<Article>().Property(u => u.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<PasswordResetToken>().HasIndex(t => t.Token).IsUnique();

            modelBuilder.Entity<Demandeur>()
                .HasMany(d => d.Demandes)
                .WithOne(d => d.Demandeur)
                .HasForeignKey(d => d.DemandeurId);

            modelBuilder.Entity<Acheteur>()
                .HasMany(a => a.Demandes)
                .WithOne(d => d.Acheteur)
                .HasForeignKey(d => d.AcheteurId);
            modelBuilder.Entity<Demande>()
                .HasOne(d => d.ValidateurCFO)
                .WithMany(v => v.DemandesCFO)
                .HasForeignKey(d => d.ValidateurCFOId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Demande>()
                .HasOne(d => d.ValidateurCOO)
                .WithMany(v => v.DemandesCOO)
                .HasForeignKey(d => d.ValidateurCOOId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DemandeArticle>()
                .HasKey(da => new { da.DemandeId, da.ArticleId });

            modelBuilder.Entity<DemandeArticle>()
                .HasOne(da => da.Demande)
                .WithMany(d => d.DemandeArticles)
                .HasForeignKey(da => da.DemandeId);

            modelBuilder.Entity<DemandeArticle>()
                .HasOne(da => da.Article)
                .WithMany(a => a.DemandeArticles)
                .HasForeignKey(da => da.ArticleId);

            modelBuilder.Entity<DemandeHistory>()
                .HasOne(dh => dh.Demande)
                .WithMany(d => d.DemandeHistories)
                .HasForeignKey(dh => dh.DemandeId);

            modelBuilder.Entity<Devis>()
                .HasOne(d => d.Demande)
                .WithMany(d => d.Devis)
                .HasForeignKey(d => d.DemandeId);

            modelBuilder.Entity<Devis>()
                .HasOne(d => d.Fournisseur)
                .WithMany(f => f.Devis)
                .HasForeignKey(d => d.FournisseurId);
        }
    }
}
