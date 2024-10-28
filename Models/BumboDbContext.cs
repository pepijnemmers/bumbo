using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;

namespace BumboApp.Models;

public partial class BumboDbContext : DbContext
{
    public BumboDbContext()
    {
    }

    public BumboDbContext(DbContextOptions<BumboDbContext> options)
        : base(options)
    {
    }
    
    private IConfiguration Configuration => new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(Configuration.GetConnectionString("BumboDbAzure"));

    public virtual DbSet<Expectation> Expectations { get; set; }

    public virtual DbSet<Norm> Norms { get; set; }

    public virtual DbSet<OpeningHour> OpeningHours { get; set; }

    public virtual DbSet<Prognosis> Prognoses { get; set; }

    public virtual DbSet<UniqueDay> UniqueDays { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WeekPrognosis> WeekPrognoses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
        modelBuilder.Entity<User>()
            .Property<string>("Password").IsRequired();

        modelBuilder.Entity<WeekPrognosis>().HasData(
    new WeekPrognosis { Id = 1, StartDate = new DateOnly(2024, 10, 7) }
);
        modelBuilder.Entity<Prognosis>().HasData(
        // Week 1, Day 1 (2024-10-07)
        new Prognosis { Id = 1, Date = new DateOnly(2024, 10, 7), Department = Department.Vers, NeededHours = 40, NeededEmployees = 5, WeekPrognosisId = 1 },
            new Prognosis { Id = 2, Date = new DateOnly(2024, 10, 7), Department = Department.Vakkenvullen, NeededHours = 35, NeededEmployees = 4, WeekPrognosisId = 1 },
            new Prognosis { Id = 3, Date = new DateOnly(2024, 10, 7), Department = Department.Kassa, NeededHours = 30, NeededEmployees = 3, WeekPrognosisId = 1 },

            // Week 1, Day 2 (2024-10-08)
            new Prognosis { Id = 4, Date = new DateOnly(2024, 10, 8), Department = Department.Vers, NeededHours = 38, NeededEmployees = 4, WeekPrognosisId = 1 },
            new Prognosis { Id = 5, Date = new DateOnly(2024, 10, 8), Department = Department.Vakkenvullen, NeededHours = 37, NeededEmployees = 3, WeekPrognosisId = 1 },
            new Prognosis { Id = 6, Date = new DateOnly(2024, 10, 8), Department = Department.Kassa, NeededHours = 28, NeededEmployees = 2, WeekPrognosisId = 1 },

            // Week 1, Day 3 (2024-10-09)
            new Prognosis { Id = 7, Date = new DateOnly(2024, 10, 9), Department = Department.Vers, NeededHours = 42, NeededEmployees = 6, WeekPrognosisId = 1 },
            new Prognosis { Id = 8, Date = new DateOnly(2024, 10, 9), Department = Department.Vakkenvullen, NeededHours = 32, NeededEmployees = 3, WeekPrognosisId = 1 },
            new Prognosis { Id = 9, Date = new DateOnly(2024, 10, 9), Department = Department.Kassa, NeededHours = 26, NeededEmployees = 2, WeekPrognosisId = 1 },

            // Week 1, Day 4 (2024-10-10)
            new Prognosis { Id = 10, Date = new DateOnly(2024, 10, 10), Department = Department.Vers, NeededHours = 42, NeededEmployees = 6, WeekPrognosisId = 1 },
            new Prognosis { Id = 11, Date = new DateOnly(2024, 10, 10), Department = Department.Vakkenvullen, NeededHours = 32, NeededEmployees = 3, WeekPrognosisId = 1 },
            new Prognosis { Id = 12, Date = new DateOnly(2024, 10, 10), Department = Department.Kassa, NeededHours = 26, NeededEmployees = 2, WeekPrognosisId = 1 },

            // Week 1, Day 5 (2024-10-11)
            new Prognosis { Id = 13, Date = new DateOnly(2024, 10, 11), Department = Department.Vers, NeededHours = 42, NeededEmployees = 6, WeekPrognosisId = 1 },
            new Prognosis { Id = 14, Date = new DateOnly(2024, 10, 11), Department = Department.Vakkenvullen, NeededHours = 32, NeededEmployees = 3, WeekPrognosisId = 1 },
            new Prognosis { Id = 15, Date = new DateOnly(2024, 10, 11), Department = Department.Kassa, NeededHours = 26, NeededEmployees = 2, WeekPrognosisId = 1 },

            // Week 1, Day 6 (2024-10-12)
            new Prognosis { Id = 16, Date = new DateOnly(2024, 10, 12), Department = Department.Vers, NeededHours = 42, NeededEmployees = 6, WeekPrognosisId = 1 },
            new Prognosis { Id = 17, Date = new DateOnly(2024, 10, 12), Department = Department.Vakkenvullen, NeededHours = 32, NeededEmployees = 3, WeekPrognosisId = 1 },
            new Prognosis { Id = 18, Date = new DateOnly(2024, 10, 12), Department = Department.Kassa, NeededHours = 26, NeededEmployees = 2, WeekPrognosisId = 1 },

            // Week 1, Day 7 (2024-10-13)
            new Prognosis { Id = 19, Date = new DateOnly(2024, 10, 13), Department = Department.Vers, NeededHours = 42, NeededEmployees = 6, WeekPrognosisId = 1 },
            new Prognosis { Id = 20, Date = new DateOnly(2024, 10, 13), Department = Department.Vakkenvullen, NeededHours = 32, NeededEmployees = 3, WeekPrognosisId = 1 },
            new Prognosis { Id = 21, Date = new DateOnly(2024, 10, 13), Department = Department.Kassa, NeededHours = 26, NeededEmployees = 2, WeekPrognosisId = 1 }
        );

        // Seed data for Norms
        modelBuilder.Entity<Norm>().HasData(
                    new Norm { Id = 1, Activity = NormActivity.ColiUitladen, Value = 5, NormType = NormType.Minutes, CreatedAt = new DateTime(2024, 10, 1, 8, 0, 0) },
                    new Norm { Id = 2, Activity = NormActivity.VakkenVullen, Value = 30, NormType = NormType.MinutesPerColi, CreatedAt = new DateTime(2024, 10, 1, 8, 0, 0) },
                    new Norm { Id = 3, Activity = NormActivity.Kassa, Value = 30, NormType = NormType.CustomersPerHour, CreatedAt = new DateTime(2024, 10, 1, 8, 0, 0) },
                    new Norm { Id = 4, Activity = NormActivity.Vers, Value = 100, NormType = NormType.CustomersPerHour, CreatedAt = new DateTime(2024, 10, 1, 8, 0, 0) },
                    new Norm { Id = 5, Activity = NormActivity.Spiegelen, Value = 30, NormType = NormType.SecondsPerMeter, CreatedAt = new DateTime(2024, 10, 1, 8, 0, 0) }
        );

        // Seed data for Expectations
        modelBuilder.Entity<Expectation>().HasData(
            new Expectation { Id = 1, Date = new DateOnly(2024, 10, 14), ExpectedCargo = 32, ExpectedCustomers = 850 },
            new Expectation { Id = 2, Date = new DateOnly(2024, 10, 15), ExpectedCargo = 40, ExpectedCustomers = 900 },
            new Expectation { Id = 3, Date = new DateOnly(2024, 10, 16), ExpectedCargo = 50, ExpectedCustomers = 980 },
            new Expectation { Id = 4, Date = new DateOnly(2024, 10, 17), ExpectedCargo = 60, ExpectedCustomers = 1050 },
            new Expectation { Id = 5, Date = new DateOnly(2024, 10, 18), ExpectedCargo = 45, ExpectedCustomers = 870 },
            new Expectation { Id = 6, Date = new DateOnly(2024, 10, 19), ExpectedCargo = 38, ExpectedCustomers = 810 },
            new Expectation { Id = 7, Date = new DateOnly(2024, 10, 20), ExpectedCargo = 55, ExpectedCustomers = 1000 },
            new Expectation { Id = 8, Date = new DateOnly(2024, 10, 21), ExpectedCargo = 33, ExpectedCustomers = 830 },
            new Expectation { Id = 9, Date = new DateOnly(2024, 10, 22), ExpectedCargo = 48, ExpectedCustomers = 920 },
            new Expectation { Id = 10, Date = new DateOnly(2024, 10, 23), ExpectedCargo = 42, ExpectedCustomers = 880 },
            new Expectation { Id = 11, Date = new DateOnly(2024, 10, 24), ExpectedCargo = 60, ExpectedCustomers = 1050 },
            new Expectation { Id = 12, Date = new DateOnly(2024, 10, 25), ExpectedCargo = 36, ExpectedCustomers = 840 },
            new Expectation { Id = 13, Date = new DateOnly(2024, 10, 26), ExpectedCargo = 53, ExpectedCustomers = 980 },
            new Expectation { Id = 14, Date = new DateOnly(2024, 10, 27), ExpectedCargo = 50, ExpectedCustomers = 950 },
            new Expectation { Id = 15, Date = new DateOnly(2024, 10, 28), ExpectedCargo = 37, ExpectedCustomers = 820 },
            new Expectation { Id = 16, Date = new DateOnly(2024, 10, 29), ExpectedCargo = 47, ExpectedCustomers = 930 },
            new Expectation { Id = 17, Date = new DateOnly(2024, 10, 30), ExpectedCargo = 35, ExpectedCustomers = 850 },
            new Expectation { Id = 18, Date = new DateOnly(2024, 10, 31), ExpectedCargo = 52, ExpectedCustomers = 1000 },
            new Expectation { Id = 19, Date = new DateOnly(2024, 11, 01), ExpectedCargo = 40, ExpectedCustomers = 890 },
            new Expectation { Id = 20, Date = new DateOnly(2024, 11, 02), ExpectedCargo = 60, ExpectedCustomers = 1050 },
            new Expectation { Id = 21, Date = new DateOnly(2024, 11, 03), ExpectedCargo = 44, ExpectedCustomers = 870 }
        );

        // Seed data for UniqueDay
        modelBuilder.Entity<UniqueDay>().HasData(
            new UniqueDay
            {
                Id = 1,
                Name = "Customer Appreciation Day",
                StartDate = new DateOnly(2024, 10, 20),
                EndDate = new DateOnly(2024, 10, 20),
                Factor = 1.25f
            },
            new UniqueDay
            {
                Id = 2,
                Name = "VIP Shopping Day",
                StartDate = new DateOnly(2024, 10, 20),
                EndDate = new DateOnly(2024, 10, 20),
                Factor = 1.5f
            },
            new UniqueDay
            {
                Id = 3,
                Name = "Weekend Sale",
                StartDate = new DateOnly(2024, 10, 28),
                EndDate = new DateOnly(2024, 10, 29),
                Factor = 1.8f
            }
        );

        // Seed data for OpeningHours
        modelBuilder.Entity<OpeningHour>().HasData(
            new OpeningHour
            {
                WeekDay = DayOfWeek.Monday,
                OpeningTime = new TimeOnly(8, 0),
                ClosingTime = new TimeOnly(21, 0)
            },
            new OpeningHour
            {
                WeekDay = DayOfWeek.Tuesday,
                OpeningTime = new TimeOnly(8, 0),
                ClosingTime = new TimeOnly(21, 0)
            },
            new OpeningHour
            {
                WeekDay = DayOfWeek.Wednesday,
                OpeningTime = new TimeOnly(8, 0),
                ClosingTime = new TimeOnly(21, 0)
            },
            new OpeningHour
            {
                WeekDay = DayOfWeek.Thursday,
                OpeningTime = new TimeOnly(8, 0),
                ClosingTime = new TimeOnly(21, 0)
            },
            new OpeningHour
            {
                WeekDay = DayOfWeek.Friday,
                OpeningTime = new TimeOnly(8, 0),
                ClosingTime = new TimeOnly(21, 0)
            },
            new OpeningHour
            {
                WeekDay = DayOfWeek.Saturday,
                OpeningTime = new TimeOnly(9, 0),
                ClosingTime = new TimeOnly(20, 0)
            },
            new OpeningHour
            {
                WeekDay = DayOfWeek.Sunday,
                OpeningTime = null,
                ClosingTime = null
            }
        );

        modelBuilder.Entity<User>().HasData(
            new
            {
                Id = 1,
                Role = Role.Manager,
                Email = "john.doe@example.com",
                FirstName = "John",
                LastName = "Doe",
                Password = "qwer1234"
            },
            new
            {
                Id = 2,
                Role = Role.Manager,
                Email = "jane.smith@example.com",
                FirstName = "Jane",
                LastName = "Smith",
                Password = "asdf1234"
            },
            new
            {
                Id = 3,
                Role = Role.Manager,
                Email = "emily.jones@example.com",
                FirstName = "Emily",
                LastName = "Jones",
                Password = "zxcv1234"
            }
        );
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
