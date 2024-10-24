﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations;

[DbContext(typeof(DataContext))]
partial class DataContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "6.0.7")
            .HasAnnotation("Relational:MaxIdentifierLength", 63);

        NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

        modelBuilder.Entity("Domain.Entities.Computer", b =>
            {
                b.Property<int>("ComputerId")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer");

                NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ComputerId"));

                b.Property<string>("Description")
                    .HasMaxLength(2048)
                    .HasColumnType("character varying(2048)");

                b.Property<string>("ImageUrl")
                    .HasColumnType("text");

                b.Property<bool>("IsArcade")
                    .HasColumnType("boolean");

                b.Property<string>("Name")
                    .HasMaxLength(255)
                    .HasColumnType("character varying(255)");

                b.HasKey("ComputerId");

                b.ToTable("Computers");
            });

        modelBuilder.Entity("Domain.Entities.Console", b =>
            {
                b.Property<int>("ConsoleId")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer");

                NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ConsoleId"));

                b.Property<string>("Description")
                    .HasMaxLength(2048)
                    .HasColumnType("character varying(2048)");

                b.Property<string>("ImageUrl")
                    .HasColumnType("text");

                b.Property<bool>("IsPortable")
                    .HasColumnType("boolean");

                b.Property<string>("Name")
                    .HasMaxLength(255)
                    .HasColumnType("character varying(255)");

                b.HasKey("ConsoleId");

                b.ToTable("Consoles");
            });

        modelBuilder.Entity("Domain.Entities.Game", b =>
            {
                b.Property<int>("GameId")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer");

                NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("GameId"));

                b.Property<string>("Description")
                    .HasColumnType("text");

                b.Property<List<string>>("Genres")
                    .HasColumnType("text[]");

                b.Property<string>("ImageUrl")
                    .HasColumnType("text");

                b.Property<int>("ReleaseYear")
                    .HasColumnType("integer");

                b.Property<string>("Summary")
                    .HasColumnType("text");

                b.Property<string>("Title")
                    .HasMaxLength(255)
                    .HasColumnType("character varying(255)");

                b.HasKey("GameId");

                b.ToTable("Games");
            });

        modelBuilder.Entity("Domain.Entities.Rating", b =>
            {
                b.Property<Guid>("RatingId")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("timestamp without time zone");

                b.Property<int>("GameId")
                    .HasColumnType("integer");

                b.Property<int>("RatingValue")
                    .HasColumnType("integer");

                b.Property<string>("Review")
                    .HasColumnType("text");

                b.Property<DateTime>("UpdatedAt")
                    .HasColumnType("timestamp without time zone");

                b.Property<Guid>("UserId")
                    .HasColumnType("uuid");

                b.HasKey("RatingId");

                b.HasIndex("GameId");

                b.HasIndex("UserId");

                b.ToTable("Ratings");
            });

        modelBuilder.Entity("Domain.Entities.User", b =>
            {
                b.Property<Guid>("UserId")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("timestamp without time zone");

                b.Property<string>("Email")
                    .HasMaxLength(255)
                    .HasColumnType("character varying(255)");

                b.Property<string>("FirstName")
                    .HasMaxLength(255)
                    .HasColumnType("character varying(255)");

                b.Property<string>("LastName")
                    .HasMaxLength(255)
                    .HasColumnType("character varying(255)");

                b.Property<string>("Password")
                    .HasColumnType("text");

                b.Property<DateTime>("UpdatedAt")
                    .HasColumnType("timestamp without time zone");

                b.Property<string>("Username")
                    .HasMaxLength(255)
                    .HasColumnType("character varying(255)");

                b.Property<DateTime>("VerifiedAt")
                    .HasColumnType("timestamp without time zone");

                b.HasKey("UserId");

                b.HasIndex("Email")
                    .IsUnique();

                b.HasIndex("Username")
                    .IsUnique();

                b.ToTable("Users");
            });

        modelBuilder.Entity("Domain.Entities.UserCollection", b =>
            {
                b.Property<Guid>("UserCollectionId")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid");

                b.Property<int>("ComputerId")
                    .HasColumnType("integer");

                b.Property<int>("Condition")
                    .HasColumnType("integer");

                b.Property<int>("ConsoleId")
                    .HasColumnType("integer");

                b.Property<int>("GameId")
                    .HasColumnType("integer");

                b.Property<string>("Notes")
                    .HasColumnType("text");

                b.Property<int>("OwnershipStatus")
                    .HasColumnType("integer");

                b.Property<DateTime>("PurchaseDate")
                    .HasColumnType("timestamp without time zone");

                b.Property<Guid>("UserId")
                    .HasColumnType("uuid");

                b.HasKey("UserCollectionId");

                b.HasIndex("GameId");

                b.HasIndex("UserId");

                b.ToTable("UserCollections");
            });

        modelBuilder.Entity("Domain.Entities.UserComputer", b =>
            {
                b.Property<Guid>("UserComputerId")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid");

                b.Property<int>("ComputerId")
                    .HasColumnType("integer");

                b.Property<int>("Condition")
                    .HasColumnType("integer");

                b.Property<string>("Notes")
                    .HasColumnType("text");

                b.Property<int>("OwnershipStatus")
                    .HasColumnType("integer");

                b.Property<DateTime>("PurchaseDate")
                    .HasColumnType("timestamp without time zone");

                b.Property<Guid>("UserId")
                    .HasColumnType("uuid");

                b.HasKey("UserComputerId");

                b.HasIndex("ComputerId");

                b.HasIndex("UserId");

                b.ToTable("UserComputers");
            });

        modelBuilder.Entity("Domain.Entities.UserConsole", b =>
            {
                b.Property<Guid>("UserConsoleId")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid");

                b.Property<int>("Condition")
                    .HasColumnType("integer");

                b.Property<int>("ConsoleId")
                    .HasColumnType("integer");

                b.Property<string>("Notes")
                    .HasColumnType("text");

                b.Property<int>("OwnershipStatus")
                    .HasColumnType("integer");

                b.Property<DateTime>("PurchaseDate")
                    .HasColumnType("timestamp without time zone");

                b.Property<Guid>("UserId")
                    .HasColumnType("uuid");

                b.HasKey("UserConsoleId");

                b.HasIndex("ConsoleId");

                b.HasIndex("UserId");

                b.ToTable("UserConsoles");
            });

        modelBuilder.Entity("Domain.Entities.Wishlist", b =>
            {
                b.Property<Guid>("WishlistId")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid");

                b.Property<int>("GameId")
                    .HasColumnType("integer");

                b.Property<Guid>("UserId")
                    .HasColumnType("uuid");

                b.HasKey("WishlistId");

                b.HasIndex("GameId");

                b.HasIndex("UserId");

                b.ToTable("Wishlists");
            });

        modelBuilder.Entity("Domain.Entities.Rating", b =>
            {
                b.HasOne("Domain.Entities.Game", "Game")
                    .WithMany("Ratings")
                    .HasForeignKey("GameId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("Domain.Entities.User", "User")
                    .WithMany("Ratings")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Game");

                b.Navigation("User");
            });

        modelBuilder.Entity("Domain.Entities.UserCollection", b =>
            {
                b.HasOne("Domain.Entities.Game", "Game")
                    .WithMany("UserCollections")
                    .HasForeignKey("GameId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("Domain.Entities.User", "User")
                    .WithMany("UserCollections")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Game");

                b.Navigation("User");
            });

        modelBuilder.Entity("Domain.Entities.UserComputer", b =>
            {
                b.HasOne("Domain.Entities.Computer", "Computer")
                    .WithMany("UserComputers")
                    .HasForeignKey("ComputerId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("Domain.Entities.User", "User")
                    .WithMany("UserComputers")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Computer");

                b.Navigation("User");
            });

        modelBuilder.Entity("Domain.Entities.UserConsole", b =>
            {
                b.HasOne("Domain.Entities.Console", "Console")
                    .WithMany("UserConsoles")
                    .HasForeignKey("ConsoleId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("Domain.Entities.User", "User")
                    .WithMany("UserConsoles")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Console");

                b.Navigation("User");
            });

        modelBuilder.Entity("Domain.Entities.Wishlist", b =>
            {
                b.HasOne("Domain.Entities.Game", "Game")
                    .WithMany()
                    .HasForeignKey("GameId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("Domain.Entities.User", "User")
                    .WithMany("Wishlists")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Game");

                b.Navigation("User");
            });

        modelBuilder.Entity("Domain.Entities.Computer", b =>
            {
                b.Navigation("UserComputers");
            });

        modelBuilder.Entity("Domain.Entities.Console", b =>
            {
                b.Navigation("UserConsoles");
            });

        modelBuilder.Entity("Domain.Entities.Game", b =>
            {
                b.Navigation("Ratings");

                b.Navigation("UserCollections");
            });

        modelBuilder.Entity("Domain.Entities.User", b =>
            {
                b.Navigation("Ratings");

                b.Navigation("UserCollections");

                b.Navigation("UserComputers");

                b.Navigation("UserConsoles");

                b.Navigation("Wishlists");
            });
#pragma warning restore 612, 618
    }
}
