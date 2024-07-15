// Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using projetStage.Models;

namespace projetStage.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        public DbSet<Demande> Demandes { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<DemandeArticle> DemandeArticles { get; set; }
        public DbSet<Devis> Devis { get; set; }
        public DbSet<Fournisseur> Fournisseurs { get; set; }
        public DbSet<DemandeHistory> DemandeHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Code)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Departement)
                .HasMaxLength(4);

            //modelBuilder.Entity<User>().Property(u => u.Role).HasMaxLength(1);

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);
            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();
            
            modelBuilder.Entity<Demande>().HasKey(u => u.Id);
            modelBuilder.Entity<Demande>().Property(u => u.Id).ValueGeneratedOnAdd();
            
            modelBuilder.Entity<Article>().HasKey(u => u.Id);
            modelBuilder.Entity<Article>().Property(u => u.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<PasswordResetToken>().HasIndex(t => t.Token).IsUnique();

            modelBuilder.Entity<Demande>()
                .HasOne(d => d.Demandeur)
                .WithMany(u => u.Demandes)
                .HasForeignKey(d => d.DemandeurId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Demande>()
                 .HasOne(d => d.Acheteur)
                 .WithMany()
                 .HasForeignKey(d => d.AcheteurId)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Demande>()
                .HasOne(d => d.ValidateurCFO)
                .WithMany()
                .HasForeignKey(d => d.ValidateurCFOId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Demande>()
                .HasOne(d => d.ValidateurCOO)
                .WithMany()
                .HasForeignKey(d => d.ValidateurCOOId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DemandeArticle>()
                .HasKey(da => da.Id);

            modelBuilder.Entity<DemandeArticle>()
                .HasOne(da => da.Demande)
                .WithMany(d => d.DemandeArticles)
                .HasForeignKey(da => da.DemandeId);

            modelBuilder.Entity<DemandeArticle>()
               .HasOne(da => da.Article)
               .WithMany(a => a.DemandeArticles)
               .HasForeignKey(da => da.ArticleId)
               .IsRequired(false);

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
