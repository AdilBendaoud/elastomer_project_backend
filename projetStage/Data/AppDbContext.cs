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
        public DbSet<DevisItem> DevisItems { get; set; }
        public DbSet<Fournisseur> Fournisseurs { get; set; }
        public DbSet<DemandeHistory> DemandeHistories { get; set; }
        public DbSet<SupplierRequest> SupplierRequests { get; set; }
        public DbSet<Currency> Currencies { get; set; }
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

            modelBuilder.Entity<DemandeHistory>()
                .HasOne(dh => dh.Demande)
                .WithMany(d => d.DemandeHistories)
                .HasForeignKey(dh => dh.DemandeId);

            modelBuilder.Entity<SupplierRequest>()
               .HasOne(sr => sr.Demande)
               .WithMany(d => d.SupplierRequests)
               .HasForeignKey(sr => sr.DemandeId);

            modelBuilder.Entity<SupplierRequest>()
                .HasOne(sr => sr.Supplier)
                .WithMany(s => s.SupplierRequests)
                .HasForeignKey(sr => sr.SupplierId);
            
            modelBuilder.Entity<DevisItem>()
                .HasOne(di => di.DemandeArticle)
                .WithMany(da => da.DevisItems)
                .HasForeignKey(di => di.DemandeArticleId);
        }
    }
}
