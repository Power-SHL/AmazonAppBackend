﻿// <auto-generated />
using AmazonAppBackend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AmazonAppBackend.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230912184032_Verification")]
    partial class Verification
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AmazonAppBackend.DTO.FriendRequest", b =>
                {
                    b.Property<string>("Sender")
                        .HasColumnType("text");

                    b.Property<string>("Receiver")
                        .HasColumnType("text");

                    b.Property<long>("TimeAdded")
                        .HasColumnType("bigint");

                    b.HasKey("Sender", "Receiver");

                    b.HasIndex("Receiver");

                    b.ToTable("FriendRequests", (string)null);
                });

            modelBuilder.Entity("AmazonAppBackend.DTO.Friendship", b =>
                {
                    b.Property<string>("User1")
                        .HasColumnType("text");

                    b.Property<string>("User2")
                        .HasColumnType("text");

                    b.Property<long>("TimeAdded")
                        .HasColumnType("bigint");

                    b.HasKey("User1", "User2");

                    b.HasIndex("User2");

                    b.ToTable("Friendships", (string)null);
                });

            modelBuilder.Entity("AmazonAppBackend.DTO.Profile", b =>
                {
                    b.Property<string>("Username")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Username");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Profiles", (string)null);
                });

            modelBuilder.Entity("AmazonAppBackend.DTO.UnverifiedProfile", b =>
                {
                    b.Property<string>("Username")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("VerificationCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Username");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("UnverifiedProfiles", (string)null);
                });

            modelBuilder.Entity("AmazonAppBackend.DTO.FriendRequest", b =>
                {
                    b.HasOne("AmazonAppBackend.DTO.Profile", "ReceiverProfile")
                        .WithMany()
                        .HasForeignKey("Receiver")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AmazonAppBackend.DTO.Profile", "SenderProfile")
                        .WithMany()
                        .HasForeignKey("Sender")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ReceiverProfile");

                    b.Navigation("SenderProfile");
                });

            modelBuilder.Entity("AmazonAppBackend.DTO.Friendship", b =>
                {
                    b.HasOne("AmazonAppBackend.DTO.Profile", "User1Profile")
                        .WithMany()
                        .HasForeignKey("User1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AmazonAppBackend.DTO.Profile", "User2Profile")
                        .WithMany()
                        .HasForeignKey("User2")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User1Profile");

                    b.Navigation("User2Profile");
                });
#pragma warning restore 612, 618
        }
    }
}