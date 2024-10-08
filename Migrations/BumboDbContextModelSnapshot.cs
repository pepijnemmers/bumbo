﻿// <auto-generated />
using System;
using BumboApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BumboApp.Migrations
{
    [DbContext(typeof(BumboDbContext))]
    partial class BumboDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-rc.1.24451.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BumboApp.Models.Expectation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<int>("ExpectedCargo")
                        .HasColumnType("int");

                    b.Property<int>("ExpectedCustomers")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Expectations");
                });

            modelBuilder.Entity("BumboApp.Models.Norm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("NormType")
                        .HasColumnType("int");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Norms");
                });

            modelBuilder.Entity("BumboApp.Models.OpeningHour", b =>
                {
                    b.Property<string>("WeekDay")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<TimeOnly>("ClosingTime")
                        .HasColumnType("time");

                    b.Property<TimeOnly>("OpeningTime")
                        .HasColumnType("time");

                    b.HasKey("WeekDay");

                    b.ToTable("OpeningHours");
                });

            modelBuilder.Entity("BumboApp.Models.Prognosis", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<float>("NeededEmployees")
                        .HasColumnType("real");

                    b.Property<float>("NeededHours")
                        .HasColumnType("real");

                    b.Property<int?>("WeekPrognosisId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WeekPrognosisId");

                    b.ToTable("Prognoses");
                });

            modelBuilder.Entity("BumboApp.Models.UniqueDay", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("EndDate")
                        .HasColumnType("date");

                    b.Property<float>("Factor")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("date");

                    b.HasKey("Id");

                    b.ToTable("UniqueDays");
                });

            modelBuilder.Entity("BumboApp.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BumboApp.Models.WeekPrognosis", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("date");

                    b.HasKey("Id");

                    b.ToTable("WeekPrognoses");
                });

            modelBuilder.Entity("BumboApp.Models.Prognosis", b =>
                {
                    b.HasOne("BumboApp.Models.WeekPrognosis", null)
                        .WithMany("Prognoses")
                        .HasForeignKey("WeekPrognosisId");
                });

            modelBuilder.Entity("BumboApp.Models.WeekPrognosis", b =>
                {
                    b.Navigation("Prognoses");
                });
#pragma warning restore 612, 618
        }
    }
}
