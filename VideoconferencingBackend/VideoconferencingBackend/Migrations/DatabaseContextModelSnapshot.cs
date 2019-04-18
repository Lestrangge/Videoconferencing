﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using VideoconferencingBackend.Models;

namespace VideoconferencingBackend.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("VideoconferencingBackend.Models.DBModels.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AvatarLink");

                    b.Property<int?>("CreatorId");

                    b.Property<string>("Description")
                        .HasMaxLength(256);

                    b.Property<string>("GroupGuid");

                    b.Property<bool?>("InCall");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("GroupGuid")
                        .IsUnique();

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("VideoconferencingBackend.Models.DBModels.GroupUser", b =>
                {
                    b.Property<int>("GroupId");

                    b.Property<int>("UserId");

                    b.Property<int>("Id");

                    b.HasKey("GroupId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("GroupUsers");
                });

            modelBuilder.Entity("VideoconferencingBackend.Models.DBModels.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("GroupId");

                    b.Property<int?>("SenderId");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(4096);

                    b.Property<DateTime>("Time");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("VideoconferencingBackend.Models.DBModels.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AvatarLink");

                    b.Property<string>("ConnectionId");

                    b.Property<string>("FcmToken");

                    b.Property<long?>("HandleId");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<long?>("SessionId");

                    b.Property<string>("Surname");

                    b.Property<string>("UserGuid");

                    b.HasKey("Id");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.HasIndex("UserGuid")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("VideoconferencingBackend.Models.DBModels.Group", b =>
                {
                    b.HasOne("VideoconferencingBackend.Models.DBModels.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId");
                });

            modelBuilder.Entity("VideoconferencingBackend.Models.DBModels.GroupUser", b =>
                {
                    b.HasOne("VideoconferencingBackend.Models.DBModels.Group", "Group")
                        .WithMany("GroupUsers")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("VideoconferencingBackend.Models.DBModels.User", "User")
                        .WithMany("GroupUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VideoconferencingBackend.Models.DBModels.Message", b =>
                {
                    b.HasOne("VideoconferencingBackend.Models.DBModels.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("VideoconferencingBackend.Models.DBModels.User", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId");
                });
#pragma warning restore 612, 618
        }
    }
}
