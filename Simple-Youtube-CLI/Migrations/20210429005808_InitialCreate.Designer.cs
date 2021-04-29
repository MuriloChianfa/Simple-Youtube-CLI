﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Simple_Youtube_CLI;

namespace Simple_Youtube_CLI.Migrations
{
    [DbContext(typeof(Database))]
    [Migration("20210429005808_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.5");

            modelBuilder.Entity("Simple_Youtube_CLI.Account", b =>
                {
                    b.Property<int>("accountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("password")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("updatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("username")
                        .HasColumnType("TEXT");

                    b.HasKey("accountId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Simple_Youtube_CLI.Dislike", b =>
                {
                    b.Property<int>("dislikeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("dislikedBy")
                        .HasColumnType("INTEGER");

                    b.Property<int>("videoId")
                        .HasColumnType("INTEGER");

                    b.HasKey("dislikeId");

                    b.ToTable("Dislikes");
                });

            modelBuilder.Entity("Simple_Youtube_CLI.Like", b =>
                {
                    b.Property<int>("likeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("likedBy")
                        .HasColumnType("INTEGER");

                    b.Property<int>("videoId")
                        .HasColumnType("INTEGER");

                    b.HasKey("likeId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("Simple_Youtube_CLI.VideoModel", b =>
                {
                    b.Property<int>("videoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("accountId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("category")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("description")
                        .HasColumnType("TEXT");

                    b.Property<int>("owner")
                        .HasColumnType("INTEGER");

                    b.Property<string>("title")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("updatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("views")
                        .HasColumnType("INTEGER");

                    b.HasKey("videoId");

                    b.HasIndex("accountId");

                    b.ToTable("VideoModel");

                    b.HasDiscriminator<string>("Discriminator").HasValue("VideoModel");
                });

            modelBuilder.Entity("Simple_Youtube_CLI.Video", b =>
                {
                    b.HasBaseType("Simple_Youtube_CLI.VideoModel");

                    b.HasDiscriminator().HasValue("Video");
                });

            modelBuilder.Entity("Simple_Youtube_CLI.VideoModel", b =>
                {
                    b.HasOne("Simple_Youtube_CLI.Account", null)
                        .WithMany("videos")
                        .HasForeignKey("accountId");
                });

            modelBuilder.Entity("Simple_Youtube_CLI.Account", b =>
                {
                    b.Navigation("videos");
                });
#pragma warning restore 612, 618
        }
    }
}