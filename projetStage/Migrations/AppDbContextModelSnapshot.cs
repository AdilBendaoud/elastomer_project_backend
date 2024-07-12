﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using projetStage.Data;

#nullable disable

namespace projetStage.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("projetStage.Models.Acheteur", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Code")
                        .HasColumnType("int");

                    b.Property<string>("Departement")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("varchar(4)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("NeedsPasswordChange")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("varchar(1)");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("Acheteurs");
                });

            modelBuilder.Entity("projetStage.Models.Admin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Code")
                        .HasColumnType("int");

                    b.Property<string>("Departement")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("varchar(4)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("NeedsPasswordChange")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("varchar(1)");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("projetStage.Models.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("projetStage.Models.Demande", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AcheteurId")
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CommentCFO")
                        .HasColumnType("longtext");

                    b.Property<string>("CommentCOO")
                        .HasColumnType("longtext");

                    b.Property<int>("DemandeurId")
                        .HasColumnType("int");

                    b.Property<bool>("IsValidateurCFORejected")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsValidateurCFOValidated")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsValidateurCOORejected")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsValidateurCOOValidated")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("OpenedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ValidatedOrRejectedByCFOAt")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("ValidatedOrRejectedByCOOAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("ValidateurCFOId")
                        .HasColumnType("int");

                    b.Property<int?>("ValidateurCOOId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AcheteurId");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("DemandeurId");

                    b.HasIndex("ValidateurCFOId");

                    b.HasIndex("ValidateurCOOId");

                    b.ToTable("Demandes");
                });

            modelBuilder.Entity("projetStage.Models.DemandeArticle", b =>
                {
                    b.Property<int>("DemandeId")
                        .HasColumnType("int");

                    b.Property<int>("ArticleId")
                        .HasColumnType("int");

                    b.Property<string>("BonCommande")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Qtt")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("DemandeId", "ArticleId");

                    b.HasIndex("ArticleId");

                    b.ToTable("DemandeArticles");
                });

            modelBuilder.Entity("projetStage.Models.DemandeHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateChanged")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("DemandeId")
                        .HasColumnType("int");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("UserCode")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DemandeId");

                    b.ToTable("DemandeHistories");
                });

            modelBuilder.Entity("projetStage.Models.Demandeur", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Code")
                        .HasColumnType("int");

                    b.Property<string>("Departement")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("varchar(4)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("NeedsPasswordChange")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("varchar(1)");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("Demandeurs");
                });

            modelBuilder.Entity("projetStage.Models.Devis", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateReception")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("DemandeId")
                        .HasColumnType("int");

                    b.Property<string>("Devise")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("FournisseurId")
                        .HasColumnType("int");

                    b.Property<decimal>("Prix")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("Qtt")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DemandeId");

                    b.HasIndex("FournisseurId");

                    b.ToTable("Devis");
                });

            modelBuilder.Entity("projetStage.Models.Fournisseur", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Adresse")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Fournisseurs");
                });

            modelBuilder.Entity("projetStage.Models.PasswordResetToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("Expiration")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Token")
                        .IsUnique();

                    b.ToTable("PasswordResetTokens");
                });

            modelBuilder.Entity("projetStage.Models.Validateur", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Code")
                        .HasColumnType("int");

                    b.Property<string>("Departement")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("varchar(4)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("NeedsPasswordChange")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("varchar(1)");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("Validateurs");
                });

            modelBuilder.Entity("projetStage.Models.Demande", b =>
                {
                    b.HasOne("projetStage.Models.Acheteur", "Acheteur")
                        .WithMany("Demandes")
                        .HasForeignKey("AcheteurId");

                    b.HasOne("projetStage.Models.Demandeur", "Demandeur")
                        .WithMany("Demandes")
                        .HasForeignKey("DemandeurId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("projetStage.Models.Validateur", "ValidateurCFO")
                        .WithMany("DemandesCFO")
                        .HasForeignKey("ValidateurCFOId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("projetStage.Models.Validateur", "ValidateurCOO")
                        .WithMany("DemandesCOO")
                        .HasForeignKey("ValidateurCOOId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Acheteur");

                    b.Navigation("Demandeur");

                    b.Navigation("ValidateurCFO");

                    b.Navigation("ValidateurCOO");
                });

            modelBuilder.Entity("projetStage.Models.DemandeArticle", b =>
                {
                    b.HasOne("projetStage.Models.Article", "Article")
                        .WithMany("DemandeArticles")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("projetStage.Models.Demande", "Demande")
                        .WithMany("DemandeArticles")
                        .HasForeignKey("DemandeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");

                    b.Navigation("Demande");
                });

            modelBuilder.Entity("projetStage.Models.DemandeHistory", b =>
                {
                    b.HasOne("projetStage.Models.Demande", "Demande")
                        .WithMany("DemandeHistories")
                        .HasForeignKey("DemandeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Demande");
                });

            modelBuilder.Entity("projetStage.Models.Devis", b =>
                {
                    b.HasOne("projetStage.Models.Demande", "Demande")
                        .WithMany("Devis")
                        .HasForeignKey("DemandeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("projetStage.Models.Fournisseur", "Fournisseur")
                        .WithMany("Devis")
                        .HasForeignKey("FournisseurId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Demande");

                    b.Navigation("Fournisseur");
                });

            modelBuilder.Entity("projetStage.Models.Acheteur", b =>
                {
                    b.Navigation("Demandes");
                });

            modelBuilder.Entity("projetStage.Models.Article", b =>
                {
                    b.Navigation("DemandeArticles");
                });

            modelBuilder.Entity("projetStage.Models.Demande", b =>
                {
                    b.Navigation("DemandeArticles");

                    b.Navigation("DemandeHistories");

                    b.Navigation("Devis");
                });

            modelBuilder.Entity("projetStage.Models.Demandeur", b =>
                {
                    b.Navigation("Demandes");
                });

            modelBuilder.Entity("projetStage.Models.Fournisseur", b =>
                {
                    b.Navigation("Devis");
                });

            modelBuilder.Entity("projetStage.Models.Validateur", b =>
                {
                    b.Navigation("DemandesCFO");

                    b.Navigation("DemandesCOO");
                });
#pragma warning restore 612, 618
        }
    }
}
