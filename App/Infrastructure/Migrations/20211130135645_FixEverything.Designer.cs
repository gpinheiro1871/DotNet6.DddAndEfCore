﻿// <auto-generated />
using System;
using App.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace App.Infrastructure.Migrations
{
    [DbContext(typeof(SchoolContext))]
    [Migration("20211130135645_FixEverything")]
    partial class FixEverything
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.0");

            modelBuilder.Entity("App.Models.Course", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Course", (string)null);
                });

            modelBuilder.Entity("App.Models.Enrollment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long?>("CourseId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("StudentId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("StudentId");

                    b.ToTable("Enrollment", (string)null);
                });

            modelBuilder.Entity("App.Models.Student", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("FavoriteCourseId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FavoriteCourseId");

                    b.ToTable("Student", (string)null);
                });

            modelBuilder.Entity("App.Models.Enrollment", b =>
                {
                    b.HasOne("App.Models.Course", null)
                        .WithMany()
                        .HasForeignKey("CourseId");

                    b.HasOne("App.Models.Student", null)
                        .WithMany()
                        .HasForeignKey("StudentId");
                });

            modelBuilder.Entity("App.Models.Student", b =>
                {
                    b.HasOne("App.Models.Course", "FavoriteCourse")
                        .WithMany()
                        .HasForeignKey("FavoriteCourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FavoriteCourse");
                });
#pragma warning restore 612, 618
        }
    }
}
