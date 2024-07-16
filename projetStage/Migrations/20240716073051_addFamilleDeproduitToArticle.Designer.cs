﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using projetStage.Data;

#nullable disable

namespace projetStage.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240716073051_addFamilleDeproduitToArticle")]
    partial class addFamilleDeproduitToArticle
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

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

                    b.Property<string>("Destination")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FamilleDeProduit")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("WESM_articles");
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
                        .HasColumnType("longtext");

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

                    b.HasIndex("DemandeurId");

                    b.HasIndex("ValidateurCFOId");

                    b.HasIndex("ValidateurCOOId");

                    b.ToTable("WESM_demandes");
                });

            modelBuilder.Entity("projetStage.Models.DemandeArticle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ArticleId")
                        .HasColumnType("int");

                    b.Property<string>("BonCommande")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("DemandeId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("Destination")
                        .HasColumnType("longtext");

                    b.Property<string>("FamilleDeProduit")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<int>("Qtt")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.HasIndex("DemandeId");

                    b.ToTable("WESM_demandeArticles");
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

                    b.ToTable("WESM_demandeHistories");
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

                    b.ToTable("WESM_devis");
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

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("WESM_fournisseurs");
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

                    b.ToTable("WESM_passwordResetTokens");
                });

            modelBuilder.Entity("projetStage.Models.User", b =>
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

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsPurchaser")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsRequester")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsValidator")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("NeedsPasswordChange")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("WESM_users");
                });

            modelBuilder.Entity("projetStage.Models.Demande", b =>
                {
                    b.HasOne("projetStage.Models.User", "Acheteur")
                        .WithMany()
                        .HasForeignKey("AcheteurId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("projetStage.Models.User", "Demandeur")
                        .WithMany("Demandes")
                        .HasForeignKey("DemandeurId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("projetStage.Models.User", "ValidateurCFO")
                        .WithMany()
                        .HasForeignKey("ValidateurCFOId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("projetStage.Models.User", "ValidateurCOO")
                        .WithMany()
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
                        .HasForeignKey("ArticleId");

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

            modelBuilder.Entity("projetStage.Models.Fournisseur", b =>
                {
                    b.Navigation("Devis");
                });

            modelBuilder.Entity("projetStage.Models.User", b =>
                {
                    b.Navigation("Demandes");
                });
#pragma warning restore 612, 618
        }
    }
}
