﻿// <auto-generated />
using System;
using FYPAllocationTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FYPAllocationTest.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FYPAllocationTest.Models.Allocation", b =>
                {
                    b.Property<int>("allocation_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("manual")
                        .HasColumnType("bit");

                    b.Property<string>("student_id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("supervisor_id")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("allocation_id");

                    b.HasIndex("student_id");

                    b.HasIndex("supervisor_id");

                    b.ToTable("allocation");
                });

            modelBuilder.Entity("FYPAllocationTest.Models.Area", b =>
                {
                    b.Property<int>("area_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("area_quota")
                        .HasColumnType("int");

                    b.Property<bool>("available")
                        .HasColumnType("bit");

                    b.Property<string>("description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("supervisor_id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("area_id");

                    b.HasIndex("supervisor_id");

                    b.ToTable("supervisor_area");
                });

            modelBuilder.Entity("FYPAllocationTest.Models.Preference", b =>
                {
                    b.Property<int>("preference_id")
                        .HasColumnType("int");

                    b.Property<string>("student_id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("area_id")
                        .HasColumnType("int");

                    b.Property<DateTime>("time_submitted")
                        .HasColumnType("datetime2");

                    b.HasKey("preference_id", "student_id");

                    b.HasIndex("area_id");

                    b.HasIndex("student_id");

                    b.ToTable("student_preference");
                });

            modelBuilder.Entity("FYPAllocationTest.Models.Student", b =>
                {
                    b.Property<string>("student_id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("average_mark")
                        .HasColumnType("float");

                    b.Property<string>("email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("surname")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("student_id");

                    b.ToTable("student");
                });

            modelBuilder.Entity("FYPAllocationTest.Models.Supervisor", b =>
                {
                    b.Property<string>("supervisor_id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("quota")
                        .HasColumnType("int");

                    b.Property<string>("surname")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("supervisor_id");

                    b.ToTable("supervisor");
                });

            modelBuilder.Entity("FYPAllocationTest.Models.Allocation", b =>
                {
                    b.HasOne("FYPAllocationTest.Models.Student", "student")
                        .WithMany()
                        .HasForeignKey("student_id");

                    b.HasOne("FYPAllocationTest.Models.Supervisor", "supervisor")
                        .WithMany()
                        .HasForeignKey("supervisor_id");
                });

            modelBuilder.Entity("FYPAllocationTest.Models.Area", b =>
                {
                    b.HasOne("FYPAllocationTest.Models.Supervisor", "supervisor")
                        .WithMany()
                        .HasForeignKey("supervisor_id");
                });

            modelBuilder.Entity("FYPAllocationTest.Models.Preference", b =>
                {
                    b.HasOne("FYPAllocationTest.Models.Area", "supervisor_area")
                        .WithMany()
                        .HasForeignKey("area_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FYPAllocationTest.Models.Student", "student")
                        .WithMany()
                        .HasForeignKey("student_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
