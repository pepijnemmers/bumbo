using System;
using System.Collections.Generic;
using System.Reflection.Emit;
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
        => optionsBuilder.UseSqlServer(Configuration.GetConnectionString("BumboDb"));

    //Module 1 classes
    public virtual DbSet<Expectation> Expectations { get; set; }

    public virtual DbSet<Norm> Norms { get; set; }

    public virtual DbSet<OpeningHour> OpeningHours { get; set; }

    public virtual DbSet<Prognosis> Prognoses { get; set; }

    public virtual DbSet<UniqueDay> UniqueDays { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WeekPrognosis> WeekPrognoses { get; set; }

    //Module 2 classes
    public virtual DbSet<Availability> Availabilities { get; set; }
    public virtual DbSet<Employee> Employees { get; set; }
    public virtual DbSet<LeaveRequest> LeaveRequests { get; set; }
    public virtual DbSet<Notification> Notifications { get; set; }
    public virtual DbSet<SchoolSchedule> SchoolSchedules { get; set; }
    public virtual DbSet<Shift> Shifts { get; set; }
    public virtual DbSet<ShiftTakeOver> ShiftTakeOvers { get; set; }
    public virtual DbSet<SickLeave> SickLeaves { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Module 1 classes
        modelBuilder.Entity<WeekPrognosis>()
            .HasIndex(wp => wp.StartDate)
            .IsUnique();

        modelBuilder.Entity<Prognosis>()
            .HasIndex(p => new { p.Date, p.Department })
            .IsUnique();


        modelBuilder.Entity<Prognosis>().ToTable(p =>
            {
                p.HasCheckConstraint("CK_Prognoses_NeededHours_NeededEmployees", "[NeededHours] = [NeededEmployees] * 8");
            });

        modelBuilder.Entity<UniqueDay>().ToTable(ud =>
            {
                ud.HasCheckConstraint("CK_UniqueDays_StartDate_EndDate", "[StartDate] <= [EndDate]");
            });

        modelBuilder.Entity<OpeningHour>().ToTable(oh =>
            {
                oh.HasCheckConstraint("CK_OpeningHours_OpeningTime_ClosingTime", "([OpeningTime] IS NULL AND [ClosingTime] IS NULL) OR ([OpeningTime] IS NOT NULL AND [ClosingTime] IS NOT NULL AND [OpeningTime] < [ClosingTime])");
            });

        //Module 2 classes
        modelBuilder.Entity<Availability>().ToTable(t => t.
            HasCheckConstraint("CK_Availability_StartTime_EndTime", "[StartTime] < [EndTime]"));

        modelBuilder.Entity<Employee>().ToTable(t =>
            {
                t.HasCheckConstraint("CK_Employees_StartOfEmployment_EndOfEmployment", "[StartOfEmployment] <= [EndOfEmployment]");
                t.HasCheckConstraint("CK_Employees_Zipcode", "[Zipcode] LIKE '[1-9][0-9][0-9][0-9][A-Z][A-Z]'");
            });

        modelBuilder.Entity<LeaveRequest>().ToTable(lr =>
            {
                lr.HasCheckConstraint("CK_LeaveRequests_StartDate_EndDate", "[StartDate] <= [EndDate]");
            });

        modelBuilder.Entity<Shift>().ToTable(lr =>
            {
                lr.HasCheckConstraint("CK_Shifts_Start_End", "[Start] < [End]");
            });

        OnModelCreatingPartial(modelBuilder);
        modelBuilder.Entity<User>(u =>
            {
                u.HasIndex(u => u.Email)
                 .IsUnique();
                u.Property<string>("Password").IsRequired();
            });

        //Composite keys tables explicit relations (efcore doesnt interpret right)
        modelBuilder.Entity<Availability>()
            .HasKey(ss => new { ss.EmployeeNumber, ss.Date });

        modelBuilder.Entity<Availability>()
            .HasOne(ss => ss.Employee)
            .WithMany(e => e.Availabilities)
            .HasForeignKey(ss => ss.EmployeeNumber)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SickLeave>()
            .HasKey(ss => new { ss.EmployeeNumber, ss.Date });

        modelBuilder.Entity<SickLeave>()
            .HasOne(ss => ss.Employee)
            .WithMany(e => e.sickLeaves)
            .HasForeignKey(ss => ss.EmployeeNumber)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SchoolSchedule>()
            .HasKey(ss => new { ss.EmployeeNumber, ss.Date });

        modelBuilder.Entity<SchoolSchedule>()
            .HasOne(ss => ss.Employee)
            .WithMany(e => e.SchoolSchedules)
            .HasForeignKey(ss => ss.EmployeeNumber)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Shift>()
            .HasOne(s => s.ShiftTakeOver)
            .WithOne(sto => sto.Shift)
            .HasForeignKey<ShiftTakeOver>(sto => sto.ShiftId)
            .OnDelete(DeleteBehavior.NoAction);

        //------------- SeedData ---------------------------
        EssentialSeedData(modelBuilder);
        SeedData(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    private void EssentialSeedData(ModelBuilder modelBuilder)
    {
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
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WeekPrognosis>().HasData(
            new WeekPrognosis { Id = 1, StartDate = new DateOnly(2024, 10, 7) },
            new WeekPrognosis { Id = 2, StartDate = new DateOnly(2024, 12, 9) }
        );

        modelBuilder.Entity<Prognosis>().HasData(
            // Week 1, Day 1 (2024-10-07)
            new Prognosis { Id = 1, Date = new DateOnly(2024, 10, 7), Department = Department.Vers, NeededHours = 40, NeededEmployees = 5, WeekPrognosisId = 1 },
            new Prognosis { Id = 2, Date = new DateOnly(2024, 10, 7), Department = Department.Vakkenvullen, NeededHours = 32, NeededEmployees = 4, WeekPrognosisId = 1 },
            new Prognosis { Id = 3, Date = new DateOnly(2024, 10, 7), Department = Department.Kassa, NeededHours = 24, NeededEmployees = 3, WeekPrognosisId = 1 },

            // Week 1, Day 2 (2024-10-08)
            new Prognosis { Id = 4, Date = new DateOnly(2024, 10, 8), Department = Department.Vers, NeededHours = 32, NeededEmployees = 4, WeekPrognosisId = 1 },
            new Prognosis { Id = 5, Date = new DateOnly(2024, 10, 8), Department = Department.Vakkenvullen, NeededHours = 24, NeededEmployees = 3, WeekPrognosisId = 1 },
            new Prognosis { Id = 6, Date = new DateOnly(2024, 10, 8), Department = Department.Kassa, NeededHours = 16, NeededEmployees = 2, WeekPrognosisId = 1 },

            // Week 1, Day 3 (2024-10-09)
            new Prognosis { Id = 7, Date = new DateOnly(2024, 10, 9), Department = Department.Vers, NeededHours = 48, NeededEmployees = 6, WeekPrognosisId = 1 },
            new Prognosis { Id = 8, Date = new DateOnly(2024, 10, 9), Department = Department.Vakkenvullen, NeededHours = 24, NeededEmployees = 3, WeekPrognosisId = 1 },
            new Prognosis { Id = 9, Date = new DateOnly(2024, 10, 9), Department = Department.Kassa, NeededHours = 16, NeededEmployees = 2, WeekPrognosisId = 1 },

            // Week 1, Day 4 (2024-10-10)
            new Prognosis { Id = 10, Date = new DateOnly(2024, 10, 10), Department = Department.Vers, NeededHours = 48, NeededEmployees = 6, WeekPrognosisId = 1 },
            new Prognosis { Id = 11, Date = new DateOnly(2024, 10, 10), Department = Department.Vakkenvullen, NeededHours = 24, NeededEmployees = 3, WeekPrognosisId = 1 },
            new Prognosis { Id = 12, Date = new DateOnly(2024, 10, 10), Department = Department.Kassa, NeededHours = 16, NeededEmployees = 2, WeekPrognosisId = 1 },

            // Week 1, Day 5 (2024-10-11)
            new Prognosis { Id = 13, Date = new DateOnly(2024, 10, 11), Department = Department.Vers, NeededHours = 48, NeededEmployees = 6, WeekPrognosisId = 1 },
            new Prognosis { Id = 14, Date = new DateOnly(2024, 10, 11), Department = Department.Vakkenvullen, NeededHours = 24, NeededEmployees = 3, WeekPrognosisId = 1 },
            new Prognosis { Id = 15, Date = new DateOnly(2024, 10, 11), Department = Department.Kassa, NeededHours = 16, NeededEmployees = 2, WeekPrognosisId = 1 },

            // Week 1, Day 6 (2024-10-12)
            new Prognosis { Id = 16, Date = new DateOnly(2024, 10, 12), Department = Department.Vers, NeededHours = 48, NeededEmployees = 6, WeekPrognosisId = 1 },
            new Prognosis { Id = 17, Date = new DateOnly(2024, 10, 12), Department = Department.Vakkenvullen, NeededHours = 24, NeededEmployees = 3, WeekPrognosisId = 1 },
            new Prognosis { Id = 18, Date = new DateOnly(2024, 10, 12), Department = Department.Kassa, NeededHours = 16, NeededEmployees = 2, WeekPrognosisId = 1 },

            // Week 1, Day 7 (2024-10-13)
            new Prognosis { Id = 19, Date = new DateOnly(2024, 10, 13), Department = Department.Vers, NeededHours = 48, NeededEmployees = 6, WeekPrognosisId = 1 },
            new Prognosis { Id = 20, Date = new DateOnly(2024, 10, 13), Department = Department.Vakkenvullen, NeededHours = 24, NeededEmployees = 3, WeekPrognosisId = 1 },
            new Prognosis { Id = 21, Date = new DateOnly(2024, 10, 13), Department = Department.Kassa, NeededHours = 16, NeededEmployees = 2, WeekPrognosisId = 1 },

            // Week 2, Day 1 (2024-12-09)
            new Prognosis { Id = 22, Date = new DateOnly(2024, 12, 9), Department = Department.Vers, NeededHours = 40, NeededEmployees = 5.0f, WeekPrognosisId = 2 },
            new Prognosis { Id = 23, Date = new DateOnly(2024, 12, 9), Department = Department.Vakkenvullen, NeededHours = 36, NeededEmployees = 4.5f, WeekPrognosisId = 2 },
            new Prognosis { Id = 24, Date = new DateOnly(2024, 12, 9), Department = Department.Kassa, NeededHours = 20, NeededEmployees = 2.5f, WeekPrognosisId = 2 },

            // Week 2, Day 2 (2024-12-10)
            new Prognosis { Id = 25, Date = new DateOnly(2024, 12, 10), Department = Department.Vers, NeededHours = 32, NeededEmployees = 4.0f, WeekPrognosisId = 2 },
            new Prognosis { Id = 26, Date = new DateOnly(2024, 12, 10), Department = Department.Vakkenvullen, NeededHours = 28, NeededEmployees = 3.5f, WeekPrognosisId = 2 },
            new Prognosis { Id = 27, Date = new DateOnly(2024, 12, 10), Department = Department.Kassa, NeededHours = 16, NeededEmployees = 2.0f, WeekPrognosisId = 2 },

            // Week 2, Day 3 (2024-12-11)
            new Prognosis { Id = 28, Date = new DateOnly(2024, 12, 11), Department = Department.Vers, NeededHours = 40, NeededEmployees = 5.0f, WeekPrognosisId = 2 },
            new Prognosis { Id = 29, Date = new DateOnly(2024, 12, 11), Department = Department.Vakkenvullen, NeededHours = 24, NeededEmployees = 3.0f, WeekPrognosisId = 2 },
            new Prognosis { Id = 30, Date = new DateOnly(2024, 12, 11), Department = Department.Kassa, NeededHours = 20, NeededEmployees = 2.5f, WeekPrognosisId = 2 },

            // Week 2, Day 4 (2024-12-12)
            new Prognosis { Id = 31, Date = new DateOnly(2024, 12, 12), Department = Department.Vers, NeededHours = 48, NeededEmployees = 6.0f, WeekPrognosisId = 2 },
            new Prognosis { Id = 32, Date = new DateOnly(2024, 12, 12), Department = Department.Vakkenvullen, NeededHours = 36, NeededEmployees = 4.5f, WeekPrognosisId = 2 },
            new Prognosis { Id = 33, Date = new DateOnly(2024, 12, 12), Department = Department.Kassa, NeededHours = 16, NeededEmployees = 2.0f, WeekPrognosisId = 2 },

            // Week 2, Day 5 (2024-12-13)
            new Prognosis { Id = 34, Date = new DateOnly(2024, 12, 13), Department = Department.Vers, NeededHours = 40, NeededEmployees = 5.0f, WeekPrognosisId = 2 },
            new Prognosis { Id = 35, Date = new DateOnly(2024, 12, 13), Department = Department.Vakkenvullen, NeededHours = 28, NeededEmployees = 3.5f, WeekPrognosisId = 2 },
            new Prognosis { Id = 36, Date = new DateOnly(2024, 12, 13), Department = Department.Kassa, NeededHours = 12, NeededEmployees = 1.5f, WeekPrognosisId = 2 },

            // Week 2, Day 6 (2024-12-14)
            new Prognosis { Id = 37, Date = new DateOnly(2024, 12, 14), Department = Department.Vers, NeededHours = 32, NeededEmployees = 4.0f, WeekPrognosisId = 2 },
            new Prognosis { Id = 38, Date = new DateOnly(2024, 12, 14), Department = Department.Vakkenvullen, NeededHours = 40, NeededEmployees = 5.0f, WeekPrognosisId = 2 },
            new Prognosis { Id = 39, Date = new DateOnly(2024, 12, 14), Department = Department.Kassa, NeededHours = 24, NeededEmployees = 3.0f, WeekPrognosisId = 2 },

            // Week 2, Day 7 (2024-12-15)
            new Prognosis { Id = 40, Date = new DateOnly(2024, 12, 15), Department = Department.Vers, NeededHours = 48, NeededEmployees = 6.0f, WeekPrognosisId = 2 },
            new Prognosis { Id = 41, Date = new DateOnly(2024, 12, 15), Department = Department.Vakkenvullen, NeededHours = 32, NeededEmployees = 4.0f, WeekPrognosisId = 2 },
            new Prognosis { Id = 42, Date = new DateOnly(2024, 12, 15), Department = Department.Kassa, NeededHours = 16, NeededEmployees = 2.0f, WeekPrognosisId = 2 }
        );

        // Seed data for Norms
        modelBuilder.Entity<Norm>().HasData(
            new Norm { Id = 1, Activity = NormActivity.ColiUitladen, Value = 5, NormType = NormType.Minutes, CreatedAt = new DateTime(2024, 10, 1, 8, 0, 0) },
            new Norm { Id = 2, Activity = NormActivity.VakkenVullen, Value = 30, NormType = NormType.MinutesPerColi, CreatedAt = new DateTime(2024, 10, 1, 8, 0, 0) },
            new Norm { Id = 3, Activity = NormActivity.Kassa, Value = 30, NormType = NormType.CustomersPerHour, CreatedAt = new DateTime(2024, 10, 1, 8, 0, 0) },
            new Norm { Id = 4, Activity = NormActivity.Vers, Value = 100, NormType = NormType.CustomersPerHour, CreatedAt = new DateTime(2024, 10, 1, 8, 0, 0) },
            new Norm { Id = 5, Activity = NormActivity.Spiegelen, Value = 30, NormType = NormType.SecondsPerMeter, CreatedAt = new DateTime(2024, 10, 1, 8, 0, 0) }
        );

        modelBuilder.Entity<Expectation>().HasData(
            new Expectation { Id = 1, Date = new DateOnly(2024, 11, 18), ExpectedCargo = 30, ExpectedCustomers = 800 },
            new Expectation { Id = 2, Date = new DateOnly(2024, 11, 19), ExpectedCargo = 40, ExpectedCustomers = 900 },
            new Expectation { Id = 3, Date = new DateOnly(2024, 11, 20), ExpectedCargo = 50, ExpectedCustomers = 950 },
            new Expectation { Id = 4, Date = new DateOnly(2024, 11, 21), ExpectedCargo = 60, ExpectedCustomers = 1000 },
            new Expectation { Id = 5, Date = new DateOnly(2024, 11, 22), ExpectedCargo = 45, ExpectedCustomers = 850 },
            new Expectation { Id = 6, Date = new DateOnly(2024, 11, 23), ExpectedCargo = 38, ExpectedCustomers = 780 },
            new Expectation { Id = 7, Date = new DateOnly(2024, 11, 24), ExpectedCargo = 55, ExpectedCustomers = 960 },
            new Expectation { Id = 8, Date = new DateOnly(2024, 11, 25), ExpectedCargo = 35, ExpectedCustomers = 810 },
            new Expectation { Id = 9, Date = new DateOnly(2024, 11, 26), ExpectedCargo = 50, ExpectedCustomers = 900 },
            new Expectation { Id = 10, Date = new DateOnly(2024, 11, 27), ExpectedCargo = 42, ExpectedCustomers = 850 },
            new Expectation { Id = 11, Date = new DateOnly(2024, 11, 28), ExpectedCargo = 60, ExpectedCustomers = 1000 },
            new Expectation { Id = 12, Date = new DateOnly(2024, 11, 29), ExpectedCargo = 37, ExpectedCustomers = 820 },
            new Expectation { Id = 13, Date = new DateOnly(2024, 11, 30), ExpectedCargo = 53, ExpectedCustomers = 940 },
            new Expectation { Id = 14, Date = new DateOnly(2024, 12, 01), ExpectedCargo = 50, ExpectedCustomers = 900 },
            new Expectation { Id = 15, Date = new DateOnly(2024, 12, 02), ExpectedCargo = 36, ExpectedCustomers = 810 },
            new Expectation { Id = 16, Date = new DateOnly(2024, 12, 03), ExpectedCargo = 47, ExpectedCustomers = 870 },
            new Expectation { Id = 17, Date = new DateOnly(2024, 12, 04), ExpectedCargo = 38, ExpectedCustomers = 780 },
            new Expectation { Id = 18, Date = new DateOnly(2024, 12, 05), ExpectedCargo = 55, ExpectedCustomers = 950 },
            new Expectation { Id = 19, Date = new DateOnly(2024, 12, 06), ExpectedCargo = 45, ExpectedCustomers = 840 },
            new Expectation { Id = 20, Date = new DateOnly(2024, 12, 07), ExpectedCargo = 60, ExpectedCustomers = 1000 },
            new Expectation { Id = 21, Date = new DateOnly(2024, 12, 08), ExpectedCargo = 40, ExpectedCustomers = 890 },
            new Expectation { Id = 22, Date = new DateOnly(2024, 12, 09), ExpectedCargo = 50, ExpectedCustomers = 920 },
            new Expectation { Id = 23, Date = new DateOnly(2024, 12, 10), ExpectedCargo = 42, ExpectedCustomers = 860 },
            new Expectation { Id = 24, Date = new DateOnly(2024, 12, 11), ExpectedCargo = 60, ExpectedCustomers = 1000 },
            new Expectation { Id = 25, Date = new DateOnly(2024, 12, 12), ExpectedCargo = 48, ExpectedCustomers = 930 },
            new Expectation { Id = 26, Date = new DateOnly(2024, 12, 13), ExpectedCargo = 35, ExpectedCustomers = 780 },
            new Expectation { Id = 27, Date = new DateOnly(2024, 12, 14), ExpectedCargo = 53, ExpectedCustomers = 950 },
            new Expectation { Id = 28, Date = new DateOnly(2024, 12, 15), ExpectedCargo = 50, ExpectedCustomers = 900 }
        );

        modelBuilder.Entity<UniqueDay>().HasData(
            new UniqueDay
            {
                Id = 1,
                Name = "Customer Appreciation Day",
                StartDate = new DateOnly(2024, 11, 22),
                EndDate = new DateOnly(2024, 11, 22),
                Factor = 1.25f
            },
            new UniqueDay
            {
                Id = 2,
                Name = "VIP Shopping Day",
                StartDate = new DateOnly(2024, 11, 29),
                EndDate = new DateOnly(2024, 11, 29),
                Factor = 1.5f
            },
            new UniqueDay
            {
                Id = 3,
                Name = "Weekend Sale",
                StartDate = new DateOnly(2024, 12, 7),
                EndDate = new DateOnly(2024, 12, 8),
                Factor = 1.8f
            }
        );


        modelBuilder.Entity<User>().HasData(
            new
            {
                Id = 1,
                Role = Role.Manager,
                Email = "john.doe@example.com",
                Password = "qwer1234"
            },
            new
            {
                Id = 2,
                Role = Role.Employee,
                Email = "jane.smith@example.com",
                Password = "asdf1234"
            },
            new
            {
                Id = 3,
                Role = Role.Employee,
                Email = "emily.jones@example.com",
                Password = "zxcv1234"
            }
        );


        modelBuilder.Entity<Employee>().HasData(
    new
    {
        EmployeeNumber = 1,
        FirstName = "John",
        LastName = "Doe",
        DateOfBirth = new DateOnly(1990, 5, 20),
        Zipcode = "1234AB",
        HouseNumber = "1A",
        ContractHours = 40,
        LeaveHours = 60,
        StartOfEmployment = new DateOnly(2020, 1, 15),
        UserId = 1
    },
    new
    {
        EmployeeNumber = 2,
        FirstName = "Jane",
        LastName = "Smith",
        DateOfBirth = new DateOnly(1995, 8, 12),
        Zipcode = "5684AC",
        HouseNumber = "2B",
        ContractHours = 20,
        LeaveHours = 5,
        StartOfEmployment = new DateOnly(2021, 3, 1),
        UserId = 2
    },
    new
    {
        EmployeeNumber = 3,
        FirstName = "Emily",
        LastName = "Jones",
        DateOfBirth = new DateOnly(1998, 12, 5),
        Zipcode = "5211DG",
        HouseNumber = "3C",
        ContractHours = 35,
        LeaveHours = 40,
        StartOfEmployment = new DateOnly(2019, 7, 30),
        UserId = 3
    }
);

        modelBuilder.Entity<Availability>().HasData(
            new
            {
                Date = new DateOnly(2024, 12, 9),
                EmployeeNumber = 1,
                StartTime = new TimeOnly(9, 0),
                EndTime = new TimeOnly(17, 0)
            },
            new
            {
                Date = new DateOnly(2024, 12, 10),
                EmployeeNumber = 2,
                StartTime = new TimeOnly(13, 0),
                EndTime = new TimeOnly(17, 0)
            },
            new
            {
                Date = new DateOnly(2024, 12, 11),
                EmployeeNumber = 3,
                StartTime = new TimeOnly(8, 0),
                EndTime = new TimeOnly(14, 0)
            }
        );

        modelBuilder.Entity<SchoolSchedule>().HasData(
            // Schedule for John (Employee 1)
            new { EmployeeNumber = 1, Date = new DateOnly(2024, 12, 9), DurationInHours = 4.0f },
            new { EmployeeNumber = 1, Date = new DateOnly(2024, 12, 10), DurationInHours = 4.0f },
            // Schedule for Jane (Employee 2)
            new { EmployeeNumber = 2, Date = new DateOnly(2024, 12, 9), DurationInHours = 3.0f },
            new { EmployeeNumber = 2, Date = new DateOnly(2024, 12, 10), DurationInHours = 3.0f },
            // Schedule for Emily (Employee 3)
            new { EmployeeNumber = 3, Date = new DateOnly(2024, 12, 9), DurationInHours = 6.0f },
            new { EmployeeNumber = 3, Date = new DateOnly(2024, 12, 10), DurationInHours = 6.0f }
        );

        modelBuilder.Entity<LeaveRequest>().HasData(
            new
            {
                Id = 1,
                Status = Status.Geaccepteerd,
                StartDate = new DateTime(2024, 12, 13),
                EndDate = new DateTime(2024, 12, 14),
                Reason = "Family event",
                EmployeeNumber = 1
            },
            new
            {
                Id = 2,
                Status = Status.Aangevraagd,
                StartDate = new DateTime(2024, 12, 15),
                EndDate = new DateTime(2024, 12, 15),
                Reason = "Medical appointment",
                EmployeeNumber = 2
            }
        );

        modelBuilder.Entity<Notification>().HasData(
            new
            {
                Id = 1,
                EmployeeNumber = 1,
                Title = "Meeting Reminder",
                Description = "Don't forget the department meeting on Dec 10.",
                SentAt = new DateTime(2024, 12, 8, 9, 0, 0),
                HasBeenRead = false
            },
            new
            {
                Id = 2,
                EmployeeNumber = 3,
                Title = "Holiday Hours",
                Description = "Check your holiday hours for December.",
                SentAt = new DateTime(2024, 12, 7, 15, 30, 0),
                HasBeenRead = true
            }
        );

        modelBuilder.Entity<Shift>().HasData(
            new
            {
                Id = 1,
                Start = new DateTime(2024, 12, 9, 9, 0, 0),
                End = new DateTime(2024, 12, 9, 17, 0, 0),
                Department = Department.Kassa,
                EmployeeNumber = 1,
                IsFinal = false
            },
            new
            {
                Id = 2,
                Start = new DateTime(2024, 12, 10, 13, 0, 0),
                End = new DateTime(2024, 12, 10, 17, 0, 0),
                Department = Department.Vakkenvullen,
                EmployeeNumber = 2,
                IsFinal = false
            }
        );

        modelBuilder.Entity<ShiftTakeOver>().HasData(
            new
            {
                ShiftId = 2,
                EmployeeTakingOverEmployeeNumber = 3,
                Status = Status.Aangevraagd
            },
            new
            {
                ShiftId = 1,
                EmployeeTakingOverEmployeeNumber = 2,
                Status = Status.Afgewezen
            }
        );
    }
}
