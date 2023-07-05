﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TimeManager.Database;

#nullable disable

namespace TimeManager.Database.Migrations
{
    [DbContext(typeof(TimeManagerDbContext))]
    [Migration("20230705141213_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.8");

            modelBuilder.Entity("TimeManager.Core.Models.ObservedProcess", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ClosedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("OpenedAt")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("TotalSpent")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ObservedProcesses");
                });
#pragma warning restore 612, 618
        }
    }
}
