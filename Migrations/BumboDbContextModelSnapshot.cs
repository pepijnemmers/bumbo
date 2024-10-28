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

                    b.HasIndex("Date")
                        .IsUnique();

                    b.ToTable("Expectations");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Date = new DateOnly(2024, 10, 14),
                            ExpectedCargo = 32,
                            ExpectedCustomers = 850
                        },
                        new
                        {
                            Id = 2,
                            Date = new DateOnly(2024, 10, 15),
                            ExpectedCargo = 40,
                            ExpectedCustomers = 900
                        },
                        new
                        {
                            Id = 3,
                            Date = new DateOnly(2024, 10, 16),
                            ExpectedCargo = 50,
                            ExpectedCustomers = 980
                        },
                        new
                        {
                            Id = 4,
                            Date = new DateOnly(2024, 10, 17),
                            ExpectedCargo = 60,
                            ExpectedCustomers = 1050
                        },
                        new
                        {
                            Id = 5,
                            Date = new DateOnly(2024, 10, 18),
                            ExpectedCargo = 45,
                            ExpectedCustomers = 870
                        },
                        new
                        {
                            Id = 6,
                            Date = new DateOnly(2024, 10, 19),
                            ExpectedCargo = 38,
                            ExpectedCustomers = 810
                        },
                        new
                        {
                            Id = 7,
                            Date = new DateOnly(2024, 10, 20),
                            ExpectedCargo = 55,
                            ExpectedCustomers = 1000
                        },
                        new
                        {
                            Id = 8,
                            Date = new DateOnly(2024, 10, 21),
                            ExpectedCargo = 33,
                            ExpectedCustomers = 830
                        },
                        new
                        {
                            Id = 9,
                            Date = new DateOnly(2024, 10, 22),
                            ExpectedCargo = 48,
                            ExpectedCustomers = 920
                        },
                        new
                        {
                            Id = 10,
                            Date = new DateOnly(2024, 10, 23),
                            ExpectedCargo = 42,
                            ExpectedCustomers = 880
                        },
                        new
                        {
                            Id = 11,
                            Date = new DateOnly(2024, 10, 24),
                            ExpectedCargo = 60,
                            ExpectedCustomers = 1050
                        },
                        new
                        {
                            Id = 12,
                            Date = new DateOnly(2024, 10, 25),
                            ExpectedCargo = 36,
                            ExpectedCustomers = 840
                        },
                        new
                        {
                            Id = 13,
                            Date = new DateOnly(2024, 10, 26),
                            ExpectedCargo = 53,
                            ExpectedCustomers = 980
                        },
                        new
                        {
                            Id = 14,
                            Date = new DateOnly(2024, 10, 27),
                            ExpectedCargo = 50,
                            ExpectedCustomers = 950
                        },
                        new
                        {
                            Id = 15,
                            Date = new DateOnly(2024, 10, 28),
                            ExpectedCargo = 37,
                            ExpectedCustomers = 820
                        },
                        new
                        {
                            Id = 16,
                            Date = new DateOnly(2024, 10, 29),
                            ExpectedCargo = 47,
                            ExpectedCustomers = 930
                        },
                        new
                        {
                            Id = 17,
                            Date = new DateOnly(2024, 10, 30),
                            ExpectedCargo = 35,
                            ExpectedCustomers = 850
                        },
                        new
                        {
                            Id = 18,
                            Date = new DateOnly(2024, 10, 31),
                            ExpectedCargo = 52,
                            ExpectedCustomers = 1000
                        },
                        new
                        {
                            Id = 19,
                            Date = new DateOnly(2024, 11, 1),
                            ExpectedCargo = 40,
                            ExpectedCustomers = 890
                        },
                        new
                        {
                            Id = 20,
                            Date = new DateOnly(2024, 11, 2),
                            ExpectedCargo = 60,
                            ExpectedCustomers = 1050
                        },
                        new
                        {
                            Id = 21,
                            Date = new DateOnly(2024, 11, 3),
                            ExpectedCargo = 44,
                            ExpectedCustomers = 870
                        });
                });

            modelBuilder.Entity("BumboApp.Models.Norm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Activity")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("NormType")
                        .HasColumnType("int");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Norms");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Activity = 0,
                            CreatedAt = new DateTime(2024, 10, 1, 8, 0, 0, 0, DateTimeKind.Unspecified),
                            NormType = 0,
                            Value = 5
                        },
                        new
                        {
                            Id = 2,
                            Activity = 1,
                            CreatedAt = new DateTime(2024, 10, 1, 8, 0, 0, 0, DateTimeKind.Unspecified),
                            NormType = 1,
                            Value = 30
                        },
                        new
                        {
                            Id = 3,
                            Activity = 2,
                            CreatedAt = new DateTime(2024, 10, 1, 8, 0, 0, 0, DateTimeKind.Unspecified),
                            NormType = 2,
                            Value = 30
                        },
                        new
                        {
                            Id = 4,
                            Activity = 3,
                            CreatedAt = new DateTime(2024, 10, 1, 8, 0, 0, 0, DateTimeKind.Unspecified),
                            NormType = 2,
                            Value = 100
                        },
                        new
                        {
                            Id = 5,
                            Activity = 4,
                            CreatedAt = new DateTime(2024, 10, 1, 8, 0, 0, 0, DateTimeKind.Unspecified),
                            NormType = 3,
                            Value = 30
                        });
                });

            modelBuilder.Entity("BumboApp.Models.OpeningHour", b =>
                {
                    b.Property<int>("WeekDay")
                        .HasColumnType("int");

                    b.Property<TimeOnly?>("ClosingTime")
                        .HasColumnType("time");

                    b.Property<TimeOnly?>("OpeningTime")
                        .HasColumnType("time");

                    b.HasKey("WeekDay");

                    b.ToTable("OpeningHours");

                    b.HasData(
                        new
                        {
                            WeekDay = 1,
                            ClosingTime = new TimeOnly(21, 0, 0),
                            OpeningTime = new TimeOnly(8, 0, 0)
                        },
                        new
                        {
                            WeekDay = 2,
                            ClosingTime = new TimeOnly(21, 0, 0),
                            OpeningTime = new TimeOnly(8, 0, 0)
                        },
                        new
                        {
                            WeekDay = 3,
                            ClosingTime = new TimeOnly(21, 0, 0),
                            OpeningTime = new TimeOnly(8, 0, 0)
                        },
                        new
                        {
                            WeekDay = 4,
                            ClosingTime = new TimeOnly(21, 0, 0),
                            OpeningTime = new TimeOnly(8, 0, 0)
                        },
                        new
                        {
                            WeekDay = 5,
                            ClosingTime = new TimeOnly(21, 0, 0),
                            OpeningTime = new TimeOnly(8, 0, 0)
                        },
                        new
                        {
                            WeekDay = 6,
                            ClosingTime = new TimeOnly(20, 0, 0),
                            OpeningTime = new TimeOnly(9, 0, 0)
                        },
                        new
                        {
                            WeekDay = 0
                        });
                });

            modelBuilder.Entity("BumboApp.Models.Prognosis", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<int>("Department")
                        .HasColumnType("int");

                    b.Property<float>("NeededEmployees")
                        .HasColumnType("real");

                    b.Property<float>("NeededHours")
                        .HasColumnType("real");

                    b.Property<int>("WeekPrognosisId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WeekPrognosisId");

                    b.ToTable("Prognoses");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Date = new DateOnly(2024, 10, 7),
                            Department = 0,
                            NeededEmployees = 5f,
                            NeededHours = 40f,
                            WeekPrognosisId = 1
                        },
                        new
                        {
                            Id = 2,
                            Date = new DateOnly(2024, 10, 7),
                            Department = 1,
                            NeededEmployees = 4f,
                            NeededHours = 35f,
                            WeekPrognosisId = 1
                        },
                        new
                        {
                            Id = 3,
                            Date = new DateOnly(2024, 10, 7),
                            Department = 2,
                            NeededEmployees = 3f,
                            NeededHours = 30f,
                            WeekPrognosisId = 1
                        },
                        new
                        {
                            Id = 4,
                            Date = new DateOnly(2024, 10, 8),
                            Department = 0,
                            NeededEmployees = 4f,
                            NeededHours = 38f,
                            WeekPrognosisId = 1
                        },
                        new
                        {
                            Id = 5,
                            Date = new DateOnly(2024, 10, 8),
                            Department = 1,
                            NeededEmployees = 3f,
                            NeededHours = 37f,
                            WeekPrognosisId = 1
                        },
                        new
                        {
                            Id = 6,
                            Date = new DateOnly(2024, 10, 8),
                            Department = 2,
                            NeededEmployees = 2f,
                            NeededHours = 28f,
                            WeekPrognosisId = 1
                        },
                        new
                        {
                            Id = 7,
                            Date = new DateOnly(2024, 10, 9),
                            Department = 0,
                            NeededEmployees = 6f,
                            NeededHours = 42f,
                            WeekPrognosisId = 1
                        },
                        new
                        {
                            Id = 8,
                            Date = new DateOnly(2024, 10, 9),
                            Department = 1,
                            NeededEmployees = 3f,
                            NeededHours = 32f,
                            WeekPrognosisId = 1
                        },
                        new
                        {
                            Id = 9,
                            Date = new DateOnly(2024, 10, 9),
                            Department = 2,
                            NeededEmployees = 2f,
                            NeededHours = 26f,
                            WeekPrognosisId = 1
                        },
                        new
                        {
                            Id = 10,
                            Date = new DateOnly(2024, 10, 10),
                            Department = 0,
                            NeededEmployees = 6f,
                            NeededHours = 42f,
                            WeekPrognosisId = 1
                        },
                        new
                        {
                            Id = 11,
                            Date = new DateOnly(2024, 10, 10),
                            Department = 1,
                            NeededEmployees = 3f,
                            NeededHours = 32f,
                            WeekPrognosisId = 1
                        },
                        new
                        {
                            Id = 12,
                            Date = new DateOnly(2024, 10, 10),
                            Department = 2,
                            NeededEmployees = 2f,
                            NeededHours = 26f,
                            WeekPrognosisId = 1
                        },
                        new
                        {
                            Id = 13,
                            Date = new DateOnly(2024, 10, 11),
                            Department = 0,
                            NeededEmployees = 6f,
                            NeededHours = 42f,
                            WeekPrognosisId = 1
                        },
                        new
                        {
                            Id = 14,
                            Date = new DateOnly(2024, 10, 11),
                            Department = 1,
                            NeededEmployees = 3f,
                            NeededHours = 32f,
                            WeekPrognosisId = 1
                        },
                        new
                        {
                            Id = 15,
                            Date = new DateOnly(2024, 10, 11),
                            Department = 2,
                            NeededEmployees = 2f,
                            NeededHours = 26f,
                            WeekPrognosisId = 1
                        },
                        new
                        {
                            Id = 16,
                            Date = new DateOnly(2024, 10, 12),
                            Department = 0,
                            NeededEmployees = 6f,
                            NeededHours = 42f,
                            WeekPrognosisId = 1
                        },
                        new
                        {
                            Id = 17,
                            Date = new DateOnly(2024, 10, 12),
                            Department = 1,
                            NeededEmployees = 3f,
                            NeededHours = 32f,
                            WeekPrognosisId = 1
                        },
                        new
                        {
                            Id = 18,
                            Date = new DateOnly(2024, 10, 12),
                            Department = 2,
                            NeededEmployees = 2f,
                            NeededHours = 26f,
                            WeekPrognosisId = 1
                        },
                        new
                        {
                            Id = 19,
                            Date = new DateOnly(2024, 10, 13),
                            Department = 0,
                            NeededEmployees = 6f,
                            NeededHours = 42f,
                            WeekPrognosisId = 1
                        },
                        new
                        {
                            Id = 20,
                            Date = new DateOnly(2024, 10, 13),
                            Department = 1,
                            NeededEmployees = 3f,
                            NeededHours = 32f,
                            WeekPrognosisId = 1
                        },
                        new
                        {
                            Id = 21,
                            Date = new DateOnly(2024, 10, 13),
                            Department = 2,
                            NeededEmployees = 2f,
                            NeededHours = 26f,
                            WeekPrognosisId = 1
                        });
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

                    b.HasData(
                        new
                        {
                            Id = 1,
                            EndDate = new DateOnly(2024, 10, 20),
                            Factor = 1.25f,
                            Name = "Customer Appreciation Day",
                            StartDate = new DateOnly(2024, 10, 20)
                        },
                        new
                        {
                            Id = 2,
                            EndDate = new DateOnly(2024, 10, 20),
                            Factor = 1.5f,
                            Name = "VIP Shopping Day",
                            StartDate = new DateOnly(2024, 10, 20)
                        },
                        new
                        {
                            Id = 3,
                            EndDate = new DateOnly(2024, 10, 29),
                            Factor = 1.8f,
                            Name = "Weekend Sale",
                            StartDate = new DateOnly(2024, 10, 28)
                        });
                });

            modelBuilder.Entity("BumboApp.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "john.doe@example.com",
                            FirstName = "John",
                            LastName = "Doe",
                            Password = "qwer1234",
                            Role = 1
                        },
                        new
                        {
                            Id = 2,
                            Email = "jane.smith@example.com",
                            FirstName = "Jane",
                            LastName = "Smith",
                            Password = "asdf1234",
                            Role = 1
                        },
                        new
                        {
                            Id = 3,
                            Email = "emily.jones@example.com",
                            FirstName = "Emily",
                            LastName = "Jones",
                            Password = "zxcv1234",
                            Role = 1
                        });
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

                    b.HasData(
                        new
                        {
                            Id = 1,
                            StartDate = new DateOnly(2024, 10, 7)
                        });
                });

            modelBuilder.Entity("BumboApp.Models.Prognosis", b =>
                {
                    b.HasOne("BumboApp.Models.WeekPrognosis", null)
                        .WithMany("Prognoses")
                        .HasForeignKey("WeekPrognosisId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BumboApp.Models.WeekPrognosis", b =>
                {
                    b.Navigation("Prognoses");
                });
#pragma warning restore 612, 618
        }
    }
}
