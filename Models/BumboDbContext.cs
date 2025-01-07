using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BumboApp.Models;

public partial class BumboDbContext : IdentityDbContext<User>
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

    public override DbSet<User> Users { get; set; }

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
    public virtual DbSet<StandardAvailability> StandardAvailabilities { get; set; }
    //Module 3
    public virtual DbSet<WorkedHour> WorkedHours { get; set; }
    public virtual DbSet<Break> Breaks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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
            HasCheckConstraint("CK_Availability_StartTime_EndTime", "[StartTime] <= [EndTime]"));

        modelBuilder.Entity<StandardAvailability>().ToTable(t => t.
            HasCheckConstraint("CK_StandardAvailability_StartTime_EndTime", "[StartTime] <= [EndTime]"));

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
            .WithMany(e => e.SickLeaves)
            .HasForeignKey(ss => ss.EmployeeNumber)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SchoolSchedule>()
            .HasKey(ss => new { ss.EmployeeNumber, ss.Date });

        modelBuilder.Entity<SchoolSchedule>()
            .HasOne(ss => ss.Employee)
            .WithMany(e => e.SchoolSchedules)
            .HasForeignKey(ss => ss.EmployeeNumber)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<StandardAvailability>()
            .HasKey(sa => new { sa.Day, sa.EmployeeNumber });

        modelBuilder.Entity<StandardAvailability>()
            .HasOne(sa => sa.Employee)
            .WithMany(e => e.StandardAvailability)
            .HasForeignKey(sa => sa.EmployeeNumber)
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

    private static void EssentialSeedData(ModelBuilder modelBuilder)
    {
        //Add roles
        var managerRoleId = "dc065cdc-e1d7-4202-936a-fbf03070c74d";
        var employeeRoleId = "4cd8ce88-df2a-49fb-ac51-0610e1be0f0b";

        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Id = managerRoleId,
                Name = Role.Manager.ToString(),
                NormalizedName = Role.Manager.ToString().ToUpper()
            },
            new IdentityRole
            {
                Id = employeeRoleId,
                Name = Role.Employee.ToString(),
                NormalizedName = Role.Employee.ToString().ToUpper()
            }
        );

        //Seed Users
        var user1Id = "2ab03136-c316-4b70-a7fc-4c9cb044a6be"; //Manager
        var user2Id = "12544476-38da-4113-9c40-4bc508f8c0f2"; //Employee
        var user3Id = "2667ab01-7225-451b-adbb-c99eea968d02"; //Employee
        var user4Id = "a60c8f93-cb79-441e-8ec9-627d8a679ff3"; //Employee
        var user5Id = "4b26e441-e63a-497e-82da-3b629212431b"; //Employee

        modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = user1Id,
                    UserName = "john.doe@example.com",
                    NormalizedUserName = "JOHN.DOE@EXAMPLE.COM",
                    Email = "john.doe@example.com",
                    NormalizedEmail = "JOHN.DOE@EXAMPLE.COM",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = "static-security-stamp",
                    ConcurrencyStamp = "static-concurrency-stamp",
                    PasswordHash = "AQAAAAIAAYagAAAAEHElifiD+iCmgFS/WCucV8tMzAcHwDdy1B4kwXCYsxB7xOwvRsxjkQbdJ6YrI77xDA=="
                },
                new User
                {
                    Id = user2Id,
                    UserName = "jane.smith@example.com",
                    NormalizedUserName = "JANE.SMITH@EXAMPLE.COM",
                    Email = "jane.smith@example.com",
                    NormalizedEmail = "JANE.SMITH@EXAMPLE.COM",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = "static-security-stamp",
                    ConcurrencyStamp = "static-concurrency-stamp",
                    PasswordHash = "AQAAAAIAAYagAAAAEGk4lj3QRvRZzy4Oas9sTTW0A2nJ1X41eB0uiNnGNFQT7RdiOs/FLSjxWz/x4KDk+w=="
                },
                new User
                {
                    Id = user3Id,
                    UserName = "emily.jones@example.com",
                    NormalizedUserName = "EMILY.JONES@EXAMPLE.COM",
                    Email = "emily.jones@example.com",
                    NormalizedEmail = "EMILY.JONES@EXAMPLE.COM",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = "static-security-stamp",
                    ConcurrencyStamp = "static-concurrency-stamp",
                    PasswordHash = "AQAAAAIAAYagAAAAEHv/0P6Xoo7fFyIXoIwA78DUHxHCFNYGaR8vPnMjmnx+QoW0Khto6+ptFaVzpYAWFw=="
                },
                new User
                {
                    Id = user4Id,
                    UserName = "bob.square@example.com",
                    NormalizedUserName = "BOB.SQUARE@EXAMPLE.COM",
                    Email = "bob.square@example.com",
                    NormalizedEmail = "BOB.SQUARE@EXAMPLE.COM",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = "static-security-stamp",
                    ConcurrencyStamp = "static-concurrency-stamp",
                    PasswordHash = "AQAAAAIAAYagAAAAEHv/0P6Xoo7fFyIXoIwA78DUHxHCFNYGaR8vPnMjmnx+QoW0Khto6+ptFaVzpYAWFw=="
                },
                new User
                {
                    Id = user5Id,
                    UserName = "IkHoudVanPaul@example.com",
                    NormalizedUserName = "IKHOUDVANPAUL@EXAMPLE.COM",
                    Email = "IkHoudVanPaul@example.com",
                    NormalizedEmail = "IKHOUDVANPAUL@EXAMPLE.COM",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = "static-security-stamp",
                    ConcurrencyStamp = "static-concurrency-stamp",
                    PasswordHash = "AQAAAAIAAYagAAAAEHv/0P6Xoo7fFyIXoIwA78DUHxHCFNYGaR8vPnMjmnx+QoW0Khto6+ptFaVzpYAWFw=="
                }
            );

        // Assign Roles to Users
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                UserId = user1Id,
                RoleId = managerRoleId
            },
            new IdentityUserRole<string>
            {
                UserId = user2Id,
                RoleId = employeeRoleId
            },
            new IdentityUserRole<string>
            {
                UserId = user3Id,
                RoleId = employeeRoleId
            },
            new IdentityUserRole<string>
            {
                UserId = user4Id,
                RoleId = employeeRoleId
            },
            new IdentityUserRole<string>
            {
                UserId = user5Id,
                RoleId = employeeRoleId
            }
        );

        //seed Employees
        modelBuilder.Entity<Employee>().HasData(
            new
            {
                EmployeeNumber = 1,
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateOnly(1990, 5, 20),
                Zipcode = "5583AA",
                HouseNumber = "1",
                ContractHours = 40,
                LeaveHours = 60,
                StartOfEmployment = new DateOnly(2020, 1, 15),
                UserId = user1Id
            },
            new
            {
                EmployeeNumber = 2,
                FirstName = "Jane",
                LastName = "Smith",
                DateOfBirth = new DateOnly(1995, 8, 12),
                Zipcode = "5684AS",
                HouseNumber = "2",
                ContractHours = 20,
                LeaveHours = 5,
                StartOfEmployment = new DateOnly(2021, 3, 1),
                UserId = user2Id
            },
            new
            {
                EmployeeNumber = 3,
                FirstName = "Emily",
                LastName = "Jones",
                DateOfBirth = new DateOnly(1998, 12, 5),
                Zipcode = "5683AA",
                HouseNumber = "1",
                ContractHours = 20,
                LeaveHours = 40,
                StartOfEmployment = new DateOnly(2020, 7, 30),
                UserId = user3Id
            },
            new
            {
                EmployeeNumber = 4,
                FirstName = "Bob",
                LastName = "van der Steen",
                DateOfBirth = new DateOnly(2002, 6, 21),
                Zipcode = "5622AX",
                HouseNumber = "25",
                ContractHours = 20,
                LeaveHours = 40,
                StartOfEmployment = new DateOnly(2020, 6, 30),
                UserId = user4Id
            },
            new
            {
                EmployeeNumber = 5,
                FirstName = "Paul",
                LastName = "Bakker",
                DateOfBirth = new DateOnly(1966, 10, 10),
                Zipcode = "5622AX",
                HouseNumber = "25",
                ContractHours = 40,
                LeaveHours = 40,
                StartOfEmployment = new DateOnly(2010, 7, 30),
                UserId = user5Id
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

        modelBuilder.Entity<StandardAvailability>().HasData(
            // Employee 2
            new StandardAvailability { Day = DayOfWeek.Monday, EmployeeNumber = 2, StartTime = new TimeOnly(18, 0), EndTime = new TimeOnly(21, 0) },
            new StandardAvailability { Day = DayOfWeek.Tuesday, EmployeeNumber = 2, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(21, 0) },
            new StandardAvailability { Day = DayOfWeek.Wednesday, EmployeeNumber = 2, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(21, 0) },
            new StandardAvailability { Day = DayOfWeek.Thursday, EmployeeNumber = 2, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(18, 0) },
            new StandardAvailability { Day = DayOfWeek.Friday, EmployeeNumber = 2, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(21, 0) },
            new StandardAvailability { Day = DayOfWeek.Saturday, EmployeeNumber = 2, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(16, 0) },
            new StandardAvailability { Day = DayOfWeek.Sunday, EmployeeNumber = 2, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(21, 0) },

            // Employee 3
            new StandardAvailability { Day = DayOfWeek.Monday, EmployeeNumber = 3, StartTime = new TimeOnly(8, 30), EndTime = new TimeOnly(14, 30) },
            new StandardAvailability { Day = DayOfWeek.Tuesday, EmployeeNumber = 3, StartTime = new TimeOnly(13, 0), EndTime = new TimeOnly(19, 0) },
            new StandardAvailability { Day = DayOfWeek.Wednesday, EmployeeNumber = 3, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(17, 0) },
            new StandardAvailability { Day = DayOfWeek.Thursday, EmployeeNumber = 3, StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(16, 0) },
            new StandardAvailability { Day = DayOfWeek.Friday, EmployeeNumber = 3, StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(20, 0) },
            new StandardAvailability { Day = DayOfWeek.Saturday, EmployeeNumber = 3, StartTime = new TimeOnly(11, 0), EndTime = new TimeOnly(18, 0) },
            new StandardAvailability { Day = DayOfWeek.Sunday, EmployeeNumber = 3, StartTime = new TimeOnly(12, 0), EndTime = new TimeOnly(16, 0) },

            // Employee 4
            new StandardAvailability { Day = DayOfWeek.Monday, EmployeeNumber = 4, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(17, 0) },
            new StandardAvailability { Day = DayOfWeek.Tuesday, EmployeeNumber = 4, StartTime = new TimeOnly(11, 0), EndTime = new TimeOnly(19, 0) },
            new StandardAvailability { Day = DayOfWeek.Wednesday, EmployeeNumber = 4, StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(18, 0) },
            new StandardAvailability { Day = DayOfWeek.Thursday, EmployeeNumber = 4, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(17, 0) },
            new StandardAvailability { Day = DayOfWeek.Friday, EmployeeNumber = 4, StartTime = new TimeOnly(13, 0), EndTime = new TimeOnly(21, 0) },
            new StandardAvailability { Day = DayOfWeek.Saturday, EmployeeNumber = 4, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(16, 0) },
            new StandardAvailability { Day = DayOfWeek.Sunday, EmployeeNumber = 4, StartTime = new TimeOnly(12, 0), EndTime = new TimeOnly(20, 0) },

            // Employee 5
            new StandardAvailability { Day = DayOfWeek.Monday, EmployeeNumber = 5, StartTime = new TimeOnly(7, 0), EndTime = new TimeOnly(15, 0) },
            new StandardAvailability { Day = DayOfWeek.Tuesday, EmployeeNumber = 5, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(16, 0) },
            new StandardAvailability { Day = DayOfWeek.Wednesday, EmployeeNumber = 5, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(17, 0) },
            new StandardAvailability { Day = DayOfWeek.Thursday, EmployeeNumber = 5, StartTime = new TimeOnly(11, 0), EndTime = new TimeOnly(19, 0) },
            new StandardAvailability { Day = DayOfWeek.Friday, EmployeeNumber = 5, StartTime = new TimeOnly(12, 0), EndTime = new TimeOnly(20, 0) },
            new StandardAvailability { Day = DayOfWeek.Saturday, EmployeeNumber = 5, StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(18, 0) },
            new StandardAvailability { Day = DayOfWeek.Sunday, EmployeeNumber = 5, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(17, 0) }
        );
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WeekPrognosis>().HasData(
            new WeekPrognosis { Id = 1, StartDate = new DateOnly(2025, 1, 6) },
            new WeekPrognosis { Id = 2, StartDate = new DateOnly(2025, 1, 13) },
            new WeekPrognosis { Id = 3, StartDate = new DateOnly(2025, 1, 20) }
        );

        modelBuilder.Entity<Prognosis>().HasData(
            // Week 2, Day 1 (2024-01-06)
            new Prognosis { Id = 1, Date = new DateOnly(2025, 1, 6), Department = Department.Vers, NeededHours = 40, NeededEmployees = 5.0f, WeekPrognosisId = 1 },
            new Prognosis { Id = 2, Date = new DateOnly(2025, 1, 6), Department = Department.Vakkenvullen, NeededHours = 32, NeededEmployees = 4.0f, WeekPrognosisId = 1 },
            new Prognosis { Id = 3, Date = new DateOnly(2025, 1, 6), Department = Department.Kassa, NeededHours = 24, NeededEmployees = 3.0f, WeekPrognosisId = 1 },

            // Week 2, Day 2 (2024-01-07)
            new Prognosis { Id = 4, Date = new DateOnly(2025, 1, 7), Department = Department.Vers, NeededHours = 32, NeededEmployees = 4.0f, WeekPrognosisId = 1 },
            new Prognosis { Id = 5, Date = new DateOnly(2025, 1, 7), Department = Department.Vakkenvullen, NeededHours = 24, NeededEmployees = 3.0f, WeekPrognosisId = 1 },
            new Prognosis { Id = 6, Date = new DateOnly(2025, 1, 7), Department = Department.Kassa, NeededHours = 16, NeededEmployees = 2.0f, WeekPrognosisId = 1 },

            // Week 2, Day 3 (2024-01-08)
            new Prognosis { Id = 7, Date = new DateOnly(2025, 1, 8), Department = Department.Vers, NeededHours = 48, NeededEmployees = 6.0f, WeekPrognosisId = 1 },
            new Prognosis { Id = 8, Date = new DateOnly(2025, 1, 8), Department = Department.Vakkenvullen, NeededHours = 24, NeededEmployees = 3.0f, WeekPrognosisId = 1 },
            new Prognosis { Id = 9, Date = new DateOnly(2025, 1, 8), Department = Department.Kassa, NeededHours = 16, NeededEmployees = 2.0f, WeekPrognosisId = 1 },

            // Week 2, Day 4 (2024-01-09)
            new Prognosis { Id = 10, Date = new DateOnly(2025, 1, 9), Department = Department.Vers, NeededHours = 48, NeededEmployees = 6.0f, WeekPrognosisId = 1 },
            new Prognosis { Id = 11, Date = new DateOnly(2025, 1, 9), Department = Department.Vakkenvullen, NeededHours = 24, NeededEmployees = 3.0f, WeekPrognosisId = 1 },
            new Prognosis { Id = 12, Date = new DateOnly(2025, 1, 9), Department = Department.Kassa, NeededHours = 16, NeededEmployees = 2.0f, WeekPrognosisId = 1 },

            // Week 2, Day 5 (2024-01-10)
            new Prognosis { Id = 13, Date = new DateOnly(2025, 1, 10), Department = Department.Vers, NeededHours = 48, NeededEmployees = 6.0f, WeekPrognosisId = 1 },
            new Prognosis { Id = 14, Date = new DateOnly(2025, 1, 10), Department = Department.Vakkenvullen, NeededHours = 24, NeededEmployees = 3.0f, WeekPrognosisId = 1 },
            new Prognosis { Id = 15, Date = new DateOnly(2025, 1, 10), Department = Department.Kassa, NeededHours = 16, NeededEmployees = 2.0f, WeekPrognosisId = 1 },

            // Week 2, Day 6 (2024-01-11)
            new Prognosis { Id = 16, Date = new DateOnly(2025, 1, 11), Department = Department.Vers, NeededHours = 48, NeededEmployees = 6.0f, WeekPrognosisId = 1 },
            new Prognosis { Id = 17, Date = new DateOnly(2025, 1, 11), Department = Department.Vakkenvullen, NeededHours = 24, NeededEmployees = 3.0f, WeekPrognosisId = 1 },
            new Prognosis { Id = 18, Date = new DateOnly(2025, 1, 11), Department = Department.Kassa, NeededHours = 16, NeededEmployees = 2.0f, WeekPrognosisId = 1 },

            // Week 2, Day 7 (2024-01-12)
            new Prognosis { Id = 19, Date = new DateOnly(2025, 1, 12), Department = Department.Vers, NeededHours = 48, NeededEmployees = 6.0f, WeekPrognosisId = 1 },
            new Prognosis { Id = 20, Date = new DateOnly(2025, 1, 12), Department = Department.Vakkenvullen, NeededHours = 24, NeededEmployees = 3.0f, WeekPrognosisId = 1 },
            new Prognosis { Id = 21, Date = new DateOnly(2025, 1, 12), Department = Department.Kassa, NeededHours = 16, NeededEmployees = 2.0f, WeekPrognosisId = 1 },

            // Week 3, Day 1 (2024-01-13)
            new Prognosis { Id = 22, Date = new DateOnly(2025, 1, 13), Department = Department.Vers, NeededHours = 40, NeededEmployees = 5.0f, WeekPrognosisId = 2 },
            new Prognosis { Id = 23, Date = new DateOnly(2025, 1, 13), Department = Department.Vakkenvullen, NeededHours = 32, NeededEmployees = 4.0f, WeekPrognosisId = 2 },
            new Prognosis { Id = 24, Date = new DateOnly(2025, 1, 13), Department = Department.Kassa, NeededHours = 24, NeededEmployees = 3.0f, WeekPrognosisId = 2 },

            // Week 3, Day 2 (2024-01-14)
            new Prognosis { Id = 25, Date = new DateOnly(2025, 1, 14), Department = Department.Vers, NeededHours = 32, NeededEmployees = 4.0f, WeekPrognosisId = 2 },
            new Prognosis { Id = 26, Date = new DateOnly(2025, 1, 14), Department = Department.Vakkenvullen, NeededHours = 24, NeededEmployees = 3.0f, WeekPrognosisId = 2 },
            new Prognosis { Id = 27, Date = new DateOnly(2025, 1, 14), Department = Department.Kassa, NeededHours = 16, NeededEmployees = 2.0f, WeekPrognosisId = 2 },

            // Week 3, Day 3 (2024-01-15)
            new Prognosis { Id = 28, Date = new DateOnly(2025, 1, 15), Department = Department.Vers, NeededHours = 40, NeededEmployees = 5.0f, WeekPrognosisId = 2 },
            new Prognosis { Id = 29, Date = new DateOnly(2025, 1, 15), Department = Department.Vakkenvullen, NeededHours = 32, NeededEmployees = 4.0f, WeekPrognosisId = 2 },
            new Prognosis { Id = 30, Date = new DateOnly(2025, 1, 15), Department = Department.Kassa, NeededHours = 24, NeededEmployees = 3.0f, WeekPrognosisId = 2 },

            // Week 3, Day 4 (2024-01-16)
            new Prognosis { Id = 31, Date = new DateOnly(2025, 1, 16), Department = Department.Vers, NeededHours = 48, NeededEmployees = 6.0f, WeekPrognosisId = 2 },
            new Prognosis { Id = 32, Date = new DateOnly(2025, 1, 16), Department = Department.Vakkenvullen, NeededHours = 32, NeededEmployees = 4.0f, WeekPrognosisId = 2 },
            new Prognosis { Id = 33, Date = new DateOnly(2025, 1, 16), Department = Department.Kassa, NeededHours = 16, NeededEmployees = 2.0f, WeekPrognosisId = 2 },

            // Week 3, Day 5 (2024-01-17)
            new Prognosis { Id = 34, Date = new DateOnly(2025, 1, 17), Department = Department.Vers, NeededHours = 40, NeededEmployees = 5.0f, WeekPrognosisId = 2 },
            new Prognosis { Id = 35, Date = new DateOnly(2025, 1, 17), Department = Department.Vakkenvullen, NeededHours = 32, NeededEmployees = 4.0f, WeekPrognosisId = 2 },
            new Prognosis { Id = 36, Date = new DateOnly(2025, 1, 17), Department = Department.Kassa, NeededHours = 24, NeededEmployees = 3.0f, WeekPrognosisId = 2 },

            // Week 3, Day 6 (2024-01-18)
            new Prognosis { Id = 37, Date = new DateOnly(2025, 1, 18), Department = Department.Vers, NeededHours = 32, NeededEmployees = 4.0f, WeekPrognosisId = 2 },
            new Prognosis { Id = 38, Date = new DateOnly(2025, 1, 18), Department = Department.Vakkenvullen, NeededHours = 40, NeededEmployees = 5.0f, WeekPrognosisId = 2 },
            new Prognosis { Id = 39, Date = new DateOnly(2025, 1, 18), Department = Department.Kassa, NeededHours = 24, NeededEmployees = 3.0f, WeekPrognosisId = 2 },

            // Week 3, Day 7 (2024-01-19)
            new Prognosis { Id = 40, Date = new DateOnly(2025, 1, 19), Department = Department.Vers, NeededHours = 48, NeededEmployees = 6.0f, WeekPrognosisId = 2 },
            new Prognosis { Id = 41, Date = new DateOnly(2025, 1, 19), Department = Department.Vakkenvullen, NeededHours = 32, NeededEmployees = 4.0f, WeekPrognosisId = 2 },
            new Prognosis { Id = 42, Date = new DateOnly(2025, 1, 19), Department = Department.Kassa, NeededHours = 16, NeededEmployees = 2.0f, WeekPrognosisId = 2 },

            // Week 4, Day 1 (2024-01-20)
            new Prognosis { Id = 43, Date = new DateOnly(2025, 1, 20), Department = Department.Vers, NeededHours = 42, NeededEmployees = 5.25f, WeekPrognosisId = 3 },
            new Prognosis { Id = 44, Date = new DateOnly(2025, 1, 20), Department = Department.Vakkenvullen, NeededHours = 34, NeededEmployees = 4.25f, WeekPrognosisId = 3 },
            new Prognosis { Id = 45, Date = new DateOnly(2025, 1, 20), Department = Department.Kassa, NeededHours = 22, NeededEmployees = 2.75f, WeekPrognosisId = 3 },

            // Week 4, Day 2 (2024-01-21)
            new Prognosis { Id = 46, Date = new DateOnly(2025, 1, 21), Department = Department.Vers, NeededHours = 34, NeededEmployees = 4.25f, WeekPrognosisId = 3 },
            new Prognosis { Id = 47, Date = new DateOnly(2025, 1, 21), Department = Department.Vakkenvullen, NeededHours = 26, NeededEmployees = 3.25f, WeekPrognosisId = 3 },
            new Prognosis { Id = 48, Date = new DateOnly(2025, 1, 21), Department = Department.Kassa, NeededHours = 18, NeededEmployees = 2.25f, WeekPrognosisId = 3 },

            // Week 4, Day 3 (2024-01-22)
            new Prognosis { Id = 49, Date = new DateOnly(2025, 1, 22), Department = Department.Vers, NeededHours = 46, NeededEmployees = 5.75f, WeekPrognosisId = 3 },
            new Prognosis { Id = 50, Date = new DateOnly(2025, 1, 22), Department = Department.Vakkenvullen, NeededHours = 28, NeededEmployees = 3.5f, WeekPrognosisId = 3 },
            new Prognosis { Id = 51, Date = new DateOnly(2025, 1, 22), Department = Department.Kassa, NeededHours = 24, NeededEmployees = 3.0f, WeekPrognosisId = 3 },

            // Week 4, Day 4 (2024-01-23)
            new Prognosis { Id = 52, Date = new DateOnly(2025, 1, 23), Department = Department.Vers, NeededHours = 50, NeededEmployees = 6.25f, WeekPrognosisId = 3 },
            new Prognosis { Id = 53, Date = new DateOnly(2025, 1, 23), Department = Department.Vakkenvullen, NeededHours = 38, NeededEmployees = 4.75f, WeekPrognosisId = 3 },
            new Prognosis { Id = 54, Date = new DateOnly(2025, 1, 23), Department = Department.Kassa, NeededHours = 18, NeededEmployees = 2.25f, WeekPrognosisId = 3 },

            // Week 4, Day 5 (2024-01-24)
            new Prognosis { Id = 55, Date = new DateOnly(2025, 1, 24), Department = Department.Vers, NeededHours = 42, NeededEmployees = 5.25f, WeekPrognosisId = 3 },
            new Prognosis { Id = 56, Date = new DateOnly(2025, 1, 24), Department = Department.Vakkenvullen, NeededHours = 30, NeededEmployees = 3.75f, WeekPrognosisId = 3 },
            new Prognosis { Id = 57, Date = new DateOnly(2025, 1, 24), Department = Department.Kassa, NeededHours = 14, NeededEmployees = 1.75f, WeekPrognosisId = 3 },

            // Week 4, Day 6 (2024-01-24)
            new Prognosis { Id = 58, Date = new DateOnly(2025, 1, 25), Department = Department.Vers, NeededHours = 32, NeededEmployees = 4.0f, WeekPrognosisId = 3 },
            new Prognosis { Id = 59, Date = new DateOnly(2025, 1, 25), Department = Department.Vakkenvullen, NeededHours = 40, NeededEmployees = 5.0f, WeekPrognosisId = 3 },
            new Prognosis { Id = 60, Date = new DateOnly(2025, 1, 25), Department = Department.Kassa, NeededHours = 24, NeededEmployees = 3.0f, WeekPrognosisId = 3 },

            // Week 4, Day 7 (2024-01-24)
            new Prognosis { Id = 61, Date = new DateOnly(2025, 1, 26), Department = Department.Vers, NeededHours = 48, NeededEmployees = 6.0f, WeekPrognosisId = 3 },
            new Prognosis { Id = 62, Date = new DateOnly(2025, 1, 26), Department = Department.Vakkenvullen, NeededHours = 32, NeededEmployees = 4.0f, WeekPrognosisId = 3 },
            new Prognosis { Id = 63, Date = new DateOnly(2025, 1, 26), Department = Department.Kassa, NeededHours = 16, NeededEmployees = 2.0f, WeekPrognosisId = 3 }
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
            new Expectation { Id = 1, Date = new DateOnly(2025, 1, 6), ExpectedCargo = 30, ExpectedCustomers = 800 },
            new Expectation { Id = 2, Date = new DateOnly(2025, 1, 7), ExpectedCargo = 40, ExpectedCustomers = 900 },
            new Expectation { Id = 3, Date = new DateOnly(2025, 1, 8), ExpectedCargo = 50, ExpectedCustomers = 950 },
            new Expectation { Id = 4, Date = new DateOnly(2025, 1, 9), ExpectedCargo = 60, ExpectedCustomers = 1000 },
            new Expectation { Id = 5, Date = new DateOnly(2025, 1, 10), ExpectedCargo = 45, ExpectedCustomers = 850 },
            new Expectation { Id = 6, Date = new DateOnly(2025, 1, 11), ExpectedCargo = 38, ExpectedCustomers = 780 },
            new Expectation { Id = 7, Date = new DateOnly(2025, 1, 12), ExpectedCargo = 55, ExpectedCustomers = 960 },
            new Expectation { Id = 8, Date = new DateOnly(2025, 1, 13), ExpectedCargo = 35, ExpectedCustomers = 810 },
            new Expectation { Id = 9, Date = new DateOnly(2025, 1, 14), ExpectedCargo = 50, ExpectedCustomers = 900 },
            new Expectation { Id = 10, Date = new DateOnly(2025, 1, 15), ExpectedCargo = 42, ExpectedCustomers = 850 },
            new Expectation { Id = 11, Date = new DateOnly(2025, 1, 16), ExpectedCargo = 60, ExpectedCustomers = 1000 },
            new Expectation { Id = 12, Date = new DateOnly(2025, 1, 17), ExpectedCargo = 37, ExpectedCustomers = 820 },
            new Expectation { Id = 13, Date = new DateOnly(2025, 1, 18), ExpectedCargo = 53, ExpectedCustomers = 940 },
            new Expectation { Id = 14, Date = new DateOnly(2025, 1, 19), ExpectedCargo = 50, ExpectedCustomers = 900 },
            new Expectation { Id = 15, Date = new DateOnly(2025, 1, 20), ExpectedCargo = 36, ExpectedCustomers = 810 },
            new Expectation { Id = 16, Date = new DateOnly(2025, 1, 21), ExpectedCargo = 47, ExpectedCustomers = 870 },
            new Expectation { Id = 17, Date = new DateOnly(2025, 1, 22), ExpectedCargo = 38, ExpectedCustomers = 780 },
            new Expectation { Id = 18, Date = new DateOnly(2025, 1, 23), ExpectedCargo = 55, ExpectedCustomers = 950 },
            new Expectation { Id = 19, Date = new DateOnly(2025, 1, 24), ExpectedCargo = 45, ExpectedCustomers = 840 },
            new Expectation { Id = 20, Date = new DateOnly(2025, 1, 25), ExpectedCargo = 60, ExpectedCustomers = 1000 },
            new Expectation { Id = 21, Date = new DateOnly(2025, 1, 26), ExpectedCargo = 40, ExpectedCustomers = 890 },
            new Expectation { Id = 22, Date = new DateOnly(2025, 1, 27), ExpectedCargo = 50, ExpectedCustomers = 920 },
            new Expectation { Id = 23, Date = new DateOnly(2025, 1, 28), ExpectedCargo = 42, ExpectedCustomers = 860 },
            new Expectation { Id = 24, Date = new DateOnly(2025, 1, 29), ExpectedCargo = 60, ExpectedCustomers = 1000 },
            new Expectation { Id = 25, Date = new DateOnly(2025, 1, 30), ExpectedCargo = 48, ExpectedCustomers = 930 },
            new Expectation { Id = 26, Date = new DateOnly(2025, 1, 31), ExpectedCargo = 35, ExpectedCustomers = 780 },
            new Expectation { Id = 27, Date = new DateOnly(2025, 2, 1), ExpectedCargo = 53, ExpectedCustomers = 950 },
            new Expectation { Id = 28, Date = new DateOnly(2025, 2, 2), ExpectedCargo = 50, ExpectedCustomers = 900 },
            new Expectation { Id = 29, Date = new DateOnly(2025, 2, 3), ExpectedCargo = 37, ExpectedCustomers = 820 },
            new Expectation { Id = 30, Date = new DateOnly(2025, 2, 4), ExpectedCargo = 55, ExpectedCustomers = 960 },
            new Expectation { Id = 31, Date = new DateOnly(2025, 2, 5), ExpectedCargo = 45, ExpectedCustomers = 840 },
            new Expectation { Id = 32, Date = new DateOnly(2025, 2, 6), ExpectedCargo = 38, ExpectedCustomers = 780 },
            new Expectation { Id = 33, Date = new DateOnly(2025, 2, 7), ExpectedCargo = 50, ExpectedCustomers = 900 },
            new Expectation { Id = 34, Date = new DateOnly(2025, 2, 8), ExpectedCargo = 48, ExpectedCustomers = 930 },
            new Expectation { Id = 35, Date = new DateOnly(2025, 2, 9), ExpectedCargo = 60, ExpectedCustomers = 1000 }
        );

        // UniqueDay 
        modelBuilder.Entity<UniqueDay>().HasData(
            new UniqueDay
            {
                Id = 1,
                Name = "Kerstavond",
                StartDate = new DateOnly(2024, 12, 24),
                EndDate = new DateOnly(2024, 12, 24),
                Factor = 1.25f
            },
            new UniqueDay
            {
                Id = 2,
                Name = "Weekend uitverkoop",
                StartDate = new DateOnly(2025, 1, 18),
                EndDate = new DateOnly(2025, 1, 19),
                Factor = 1.8f
            },
            new UniqueDay
            {
                Id = 3,
                Name = "Donderende donderdag korting",
                StartDate = new DateOnly(2025, 1, 23),
                EndDate = new DateOnly(2025, 1, 23),
                Factor = 1.5f
            },
            new UniqueDay
            {
                Id = 4,
                Name = "Blauwe maandag",
                StartDate = new DateOnly(2025, 1, 27),
                EndDate = new DateOnly(2025, 1, 27),
                Factor = 0.8f
            }
        );

        // Availability
        modelBuilder.Entity<Availability>().HasData(
            //Employee 2
            new { Date = new DateOnly(2025, 1, 6), EmployeeNumber = 2, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(17, 0) },
            new { Date = new DateOnly(2025, 1, 7), EmployeeNumber = 2, StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(18, 0) },
            new { Date = new DateOnly(2025, 1, 8), EmployeeNumber = 2, StartTime = new TimeOnly(11, 0), EndTime = new TimeOnly(15, 0) },
            new { Date = new DateOnly(2025, 1, 9), EmployeeNumber = 2, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(16, 0) },
            new { Date = new DateOnly(2025, 1, 10), EmployeeNumber = 2, StartTime = new TimeOnly(12, 0), EndTime = new TimeOnly(20, 0) },
            new { Date = new DateOnly(2025, 1, 11), EmployeeNumber = 2, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(14, 0) },
            new { Date = new DateOnly(2025, 1, 12), EmployeeNumber = 2, StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(16, 0) },
            new { Date = new DateOnly(2025, 1, 13), EmployeeNumber = 2, StartTime = new TimeOnly(11, 0), EndTime = new TimeOnly(19, 0) },
            new { Date = new DateOnly(2025, 1, 14), EmployeeNumber = 2, StartTime = new TimeOnly(13, 0), EndTime = new TimeOnly(17, 0) },
            new { Date = new DateOnly(2025, 1, 15), EmployeeNumber = 2, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(15, 0) },
            new { Date = new DateOnly(2025, 1, 16), EmployeeNumber = 2, StartTime = new TimeOnly(12, 0), EndTime = new TimeOnly(20, 0) },
            new { Date = new DateOnly(2025, 1, 17), EmployeeNumber = 2, StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(18, 0) },
            new { Date = new DateOnly(2025, 1, 18), EmployeeNumber = 2, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(13, 0) },
            new { Date = new DateOnly(2025, 1, 19), EmployeeNumber = 2, StartTime = new TimeOnly(11, 0), EndTime = new TimeOnly(17, 0) },

            //Employee 3
            new { Date = new DateOnly(2025, 1, 6), EmployeeNumber = 3, StartTime = new TimeOnly(9, 30), EndTime = new TimeOnly(17, 30) },
            new { Date = new DateOnly(2025, 1, 7), EmployeeNumber = 3, StartTime = new TimeOnly(10, 30), EndTime = new TimeOnly(18, 30) },
            new { Date = new DateOnly(2025, 1, 8), EmployeeNumber = 3, StartTime = new TimeOnly(11, 30), EndTime = new TimeOnly(15, 30) },
            new { Date = new DateOnly(2025, 1, 9), EmployeeNumber = 3, StartTime = new TimeOnly(8, 30), EndTime = new TimeOnly(16, 30) },
            new { Date = new DateOnly(2025, 1, 10), EmployeeNumber = 3, StartTime = new TimeOnly(12, 30), EndTime = new TimeOnly(20, 30) },
            new { Date = new DateOnly(2025, 1, 11), EmployeeNumber = 3, StartTime = new TimeOnly(9, 30), EndTime = new TimeOnly(14, 30) },
            new { Date = new DateOnly(2025, 1, 12), EmployeeNumber = 3, StartTime = new TimeOnly(10, 30), EndTime = new TimeOnly(16, 30) },
            new { Date = new DateOnly(2025, 1, 13), EmployeeNumber = 3, StartTime = new TimeOnly(11, 30), EndTime = new TimeOnly(19, 30) },
            new { Date = new DateOnly(2025, 1, 14), EmployeeNumber = 3, StartTime = new TimeOnly(13, 30), EndTime = new TimeOnly(17, 30) },
            new { Date = new DateOnly(2025, 1, 15), EmployeeNumber = 3, StartTime = new TimeOnly(9, 30), EndTime = new TimeOnly(15, 30) },
            new { Date = new DateOnly(2025, 1, 16), EmployeeNumber = 3, StartTime = new TimeOnly(12, 30), EndTime = new TimeOnly(20, 30) },
            new { Date = new DateOnly(2025, 1, 17), EmployeeNumber = 3, StartTime = new TimeOnly(10, 30), EndTime = new TimeOnly(18, 30) },
            new { Date = new DateOnly(2025, 1, 18), EmployeeNumber = 3, StartTime = new TimeOnly(9, 30), EndTime = new TimeOnly(13, 30) },
            new { Date = new DateOnly(2025, 1, 19), EmployeeNumber = 3, StartTime = new TimeOnly(11, 30), EndTime = new TimeOnly(17, 30) },

            // Employee 4
            new { Date = new DateOnly(2025, 1, 6), EmployeeNumber = 4, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(17, 0) },
            new { Date = new DateOnly(2025, 1, 7), EmployeeNumber = 4, StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(18, 0) },
            new { Date = new DateOnly(2025, 1, 8), EmployeeNumber = 4, StartTime = new TimeOnly(11, 0), EndTime = new TimeOnly(15, 0) },
            new { Date = new DateOnly(2025, 1, 9), EmployeeNumber = 4, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(16, 0) },
            new { Date = new DateOnly(2025, 1, 10), EmployeeNumber = 4, StartTime = new TimeOnly(12, 0), EndTime = new TimeOnly(20, 0) },
            new { Date = new DateOnly(2025, 1, 11), EmployeeNumber = 4, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(14, 0) },
            new { Date = new DateOnly(2025, 1, 12), EmployeeNumber = 4, StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(16, 0) },
            new { Date = new DateOnly(2025, 1, 13), EmployeeNumber = 4, StartTime = new TimeOnly(11, 0), EndTime = new TimeOnly(19, 0) },
            new { Date = new DateOnly(2025, 1, 14), EmployeeNumber = 4, StartTime = new TimeOnly(13, 0), EndTime = new TimeOnly(17, 0) },
            new { Date = new DateOnly(2025, 1, 15), EmployeeNumber = 4, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(15, 0) },
            new { Date = new DateOnly(2025, 1, 16), EmployeeNumber = 4, StartTime = new TimeOnly(12, 0), EndTime = new TimeOnly(20, 0) },
            new { Date = new DateOnly(2025, 1, 17), EmployeeNumber = 4, StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(18, 0) },
            new { Date = new DateOnly(2025, 1, 18), EmployeeNumber = 4, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(13, 0) },
            new { Date = new DateOnly(2025, 1, 19), EmployeeNumber = 4, StartTime = new TimeOnly(11, 0), EndTime = new TimeOnly(17, 0) },

            // Employee 5
            new { Date = new DateOnly(2025, 1, 6), EmployeeNumber = 5, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(17, 0) },
            new { Date = new DateOnly(2025, 1, 7), EmployeeNumber = 5, StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(18, 0) },
            new { Date = new DateOnly(2025, 1, 8), EmployeeNumber = 5, StartTime = new TimeOnly(11, 0), EndTime = new TimeOnly(15, 0) },
            new { Date = new DateOnly(2025, 1, 9), EmployeeNumber = 5, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(16, 0) },
            new { Date = new DateOnly(2025, 1, 10), EmployeeNumber = 5, StartTime = new TimeOnly(12, 0), EndTime = new TimeOnly(20, 0) },
            new { Date = new DateOnly(2025, 1, 11), EmployeeNumber = 5, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(14, 0) },
            new { Date = new DateOnly(2025, 1, 12), EmployeeNumber = 5, StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(16, 0) },
            new { Date = new DateOnly(2025, 1, 13), EmployeeNumber = 5, StartTime = new TimeOnly(11, 0), EndTime = new TimeOnly(19, 0) },
            new { Date = new DateOnly(2025, 1, 14), EmployeeNumber = 5, StartTime = new TimeOnly(13, 0), EndTime = new TimeOnly(17, 0) },
            new { Date = new DateOnly(2025, 1, 15), EmployeeNumber = 5, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(15, 0) },
            new { Date = new DateOnly(2025, 1, 16), EmployeeNumber = 5, StartTime = new TimeOnly(12, 0), EndTime = new TimeOnly(20, 0) },
            new { Date = new DateOnly(2025, 1, 17), EmployeeNumber = 5, StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(18, 0) },
            new { Date = new DateOnly(2025, 1, 18), EmployeeNumber = 5, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(13, 0) },
            new { Date = new DateOnly(2025, 1, 19), EmployeeNumber = 5, StartTime = new TimeOnly(11, 0), EndTime = new TimeOnly(17, 0) }
        );

        modelBuilder.Entity<SchoolSchedule>().HasData(
            // Employee 2
            new { EmployeeNumber = 2, Date = new DateOnly(2025, 1, 6), DurationInHours = 6.0f },
            new { EmployeeNumber = 2, Date = new DateOnly(2025, 1, 7), DurationInHours = 7.0f },
            new { EmployeeNumber = 2, Date = new DateOnly(2025, 1, 8), DurationInHours = 4.0f },
            new { EmployeeNumber = 2, Date = new DateOnly(2025, 1, 9), DurationInHours = 3.0f },
            new { EmployeeNumber = 2, Date = new DateOnly(2025, 1, 10), DurationInHours = 5.0f },

            new { EmployeeNumber = 2, Date = new DateOnly(2025, 1, 13), DurationInHours = 6.5f },
            new { EmployeeNumber = 2, Date = new DateOnly(2025, 1, 14), DurationInHours = 7.5f },
            new { EmployeeNumber = 2, Date = new DateOnly(2025, 1, 15), DurationInHours = 4.5f },
            new { EmployeeNumber = 2, Date = new DateOnly(2025, 1, 16), DurationInHours = 3.5f },
            new { EmployeeNumber = 2, Date = new DateOnly(2025, 1, 17), DurationInHours = 5.5f },

            // Employee 3
            new { EmployeeNumber = 3, Date = new DateOnly(2025, 1, 6), DurationInHours = 6.0f },
            new { EmployeeNumber = 3, Date = new DateOnly(2025, 1, 7), DurationInHours = 4.0f },
            new { EmployeeNumber = 3, Date = new DateOnly(2025, 1, 8), DurationInHours = 3.0f },
            new { EmployeeNumber = 3, Date = new DateOnly(2025, 1, 9), DurationInHours = 5.0f },
            new { EmployeeNumber = 3, Date = new DateOnly(2025, 1, 10), DurationInHours = 7.0f },

            new { EmployeeNumber = 3, Date = new DateOnly(2025, 1, 13), DurationInHours = 6.5f },
            new { EmployeeNumber = 3, Date = new DateOnly(2025, 1, 14), DurationInHours = 4.5f },
            new { EmployeeNumber = 3, Date = new DateOnly(2025, 1, 15), DurationInHours = 3.5f },
            new { EmployeeNumber = 3, Date = new DateOnly(2025, 1, 16), DurationInHours = 5.5f },
            new { EmployeeNumber = 3, Date = new DateOnly(2025, 1, 17), DurationInHours = 7.5f },

            // Employee 4
            new { EmployeeNumber = 4, Date = new DateOnly(2025, 1, 6), DurationInHours = 6.0f },
            new { EmployeeNumber = 4, Date = new DateOnly(2025, 1, 7), DurationInHours = 7.0f },
            new { EmployeeNumber = 4, Date = new DateOnly(2025, 1, 8), DurationInHours = 4.0f },
            new { EmployeeNumber = 4, Date = new DateOnly(2025, 1, 9), DurationInHours = 3.0f },
            new { EmployeeNumber = 4, Date = new DateOnly(2025, 1, 10), DurationInHours = 5.0f },

            new { EmployeeNumber = 4, Date = new DateOnly(2025, 1, 13), DurationInHours = 6.5f },
            new { EmployeeNumber = 4, Date = new DateOnly(2025, 1, 14), DurationInHours = 7.5f },
            new { EmployeeNumber = 4, Date = new DateOnly(2025, 1, 15), DurationInHours = 4.5f },
            new { EmployeeNumber = 4, Date = new DateOnly(2025, 1, 16), DurationInHours = 3.5f },
            new { EmployeeNumber = 4, Date = new DateOnly(2025, 1, 17), DurationInHours = 5.5f }
        );

        modelBuilder.Entity<LeaveRequest>().HasData(
            new
            {
                Id = 1,
                Status = Status.Geaccepteerd,
                StartDate = new DateTime(2025, 1, 21),
                EndDate = new DateTime(2025, 1, 22),
                Reason = "Bruiloft",
                EmployeeNumber = 2
            },
            new
            {
                Id = 2,
                Status = Status.Aangevraagd,
                StartDate = new DateTime(2025, 2, 1),
                EndDate = new DateTime(2025, 2, 2),
                Reason = "Weekendje weg",
                EmployeeNumber = 2
            },
            new
            {
                Id = 3,
                Status = Status.Aangevraagd,
                StartDate = new DateTime(2025, 1, 24),
                EndDate = new DateTime(2025, 1, 24),
                Reason = "Weekend vakantie",
                EmployeeNumber = 3
            },
            new
            {
                Id = 4,
                Status = Status.Afgewezen,
                StartDate = new DateTime(2025, 1, 21),
                EndDate = new DateTime(2025, 1, 22),
                Reason = "Geen zin om te werken dan",
                EmployeeNumber = 3
            }
        );

        modelBuilder.Entity<Notification>().HasData(
            new
            {
                Id = 1,
                EmployeeNumber = 1,
                Title = "Nieuwe verlofaanvraag",
                Description = "Er is een nieuwe verlofaanvraag om te beoordelen",
                SentAt = new DateTime(2024, 12, 7, 9, 0, 0),
                HasBeenRead = true
            },
            new
            {
                Id = 2,
                EmployeeNumber = 2,
                Title = "Nieuwe verlofaanvraag status",
                Description = "Je verlofaanvraag is beoordeeld",
                SentAt = new DateTime(2024, 12, 8, 15, 30, 0),
                HasBeenRead = false
            },
            new
            {
                Id = 3,
                EmployeeNumber = 3,
                Title = "Nieuwe verlofaanvraag status",
                Description = "Je verlofaanvraag is beoordeeld",
                SentAt = new DateTime(2024, 12, 8, 15, 30, 0),
                HasBeenRead = true
            }
        );

        modelBuilder.Entity<Shift>().HasData(
            // Employee 2
            new { Id = 1, Start = new DateTime(2025, 1, 6, 9, 0, 0), End = new DateTime(2025, 1, 6, 17, 0, 0), Department = Department.Kassa, EmployeeNumber = 2, IsFinal = true },
            new { Id = 2, Start = new DateTime(2025, 1, 7, 10, 0, 0), End = new DateTime(2025, 1, 7, 18, 0, 0), Department = Department.Vers, EmployeeNumber = 2, IsFinal = true },
            new { Id = 3, Start = new DateTime(2025, 1, 8, 11, 0, 0), End = new DateTime(2025, 1, 8, 15, 0, 0), Department = Department.Kassa, EmployeeNumber = 2, IsFinal = true },
            new { Id = 4, Start = new DateTime(2025, 1, 9, 8, 0, 0), End = new DateTime(2025, 1, 9, 16, 0, 0), Department = Department.Vakkenvullen, EmployeeNumber = 2, IsFinal = true },
            new { Id = 5, Start = new DateTime(2025, 1, 10, 12, 0, 0), End = new DateTime(2025, 1, 10, 20, 0, 0), Department = Department.Vers, EmployeeNumber = 2, IsFinal = true },
            new { Id = 6, Start = new DateTime(2025, 1, 11, 9, 0, 0), End = new DateTime(2025, 1, 11, 14, 0, 0), Department = Department.Vakkenvullen, EmployeeNumber = 2, IsFinal = true },
            new { Id = 7, Start = new DateTime(2025, 1, 12, 10, 0, 0), End = new DateTime(2025, 1, 12, 16, 0, 0), Department = Department.Kassa, EmployeeNumber = 2, IsFinal = true },
            new { Id = 8, Start = new DateTime(2025, 1, 13, 11, 0, 0), End = new DateTime(2025, 1, 13, 19, 0, 0), Department = Department.Vakkenvullen, EmployeeNumber = 2, IsFinal = false },
            new { Id = 9, Start = new DateTime(2025, 1, 14, 13, 0, 0), End = new DateTime(2025, 1, 14, 17, 0, 0), Department = Department.Vers, EmployeeNumber = 2, IsFinal = false },
            new { Id = 10, Start = new DateTime(2025, 1, 15, 9, 0, 0), End = new DateTime(2025, 1, 15, 15, 0, 0), Department = Department.Vakkenvullen, EmployeeNumber = 2, IsFinal = false },
            new { Id = 11, Start = new DateTime(2025, 1, 16, 12, 0, 0), End = new DateTime(2025, 1, 16, 20, 0, 0), Department = Department.Kassa, EmployeeNumber = 2, IsFinal = false },
            new { Id = 12, Start = new DateTime(2025, 1, 17, 10, 0, 0), End = new DateTime(2025, 1, 17, 18, 0, 0), Department = Department.Vers, EmployeeNumber = 2, IsFinal = false },

            // Employee 3
            new { Id = 13, Start = new DateTime(2025, 1, 6, 9, 0, 0), End = new DateTime(2025, 1, 6, 17, 0, 0), Department = Department.Vers, EmployeeNumber = 3, IsFinal = true },
            new { Id = 14, Start = new DateTime(2025, 1, 7, 10, 0, 0), End = new DateTime(2025, 1, 7, 18, 0, 0), Department = Department.Kassa, EmployeeNumber = 3, IsFinal = true },
            new { Id = 15, Start = new DateTime(2025, 1, 8, 11, 0, 0), End = new DateTime(2025, 1, 8, 15, 0, 0), Department = Department.Kassa, EmployeeNumber = 3, IsFinal = true },
            new { Id = 16, Start = new DateTime(2025, 1, 9, 8, 0, 0), End = new DateTime(2025, 1, 9, 16, 0, 0), Department = Department.Vakkenvullen, EmployeeNumber = 3, IsFinal = true },
            new { Id = 17, Start = new DateTime(2025, 1, 10, 12, 0, 0), End = new DateTime(2025, 1, 10, 20, 0, 0), Department = Department.Kassa, EmployeeNumber = 3, IsFinal = true },
            new { Id = 18, Start = new DateTime(2025, 1, 11, 9, 0, 0), End = new DateTime(2025, 1, 11, 14, 0, 0), Department = Department.Vakkenvullen, EmployeeNumber = 3, IsFinal = true },
            new { Id = 19, Start = new DateTime(2025, 1, 12, 10, 0, 0), End = new DateTime(2025, 1, 12, 16, 0, 0), Department = Department.Vers, EmployeeNumber = 3, IsFinal = true },
            new { Id = 20, Start = new DateTime(2025, 1, 13, 11, 0, 0), End = new DateTime(2025, 1, 13, 19, 0, 0), Department = Department.Vakkenvullen, EmployeeNumber = 3, IsFinal = false },
            new { Id = 21, Start = new DateTime(2025, 1, 14, 13, 0, 0), End = new DateTime(2025, 1, 14, 17, 0, 0), Department = Department.Kassa, EmployeeNumber = 3, IsFinal = false },
            new { Id = 22, Start = new DateTime(2025, 1, 15, 9, 0, 0), End = new DateTime(2025, 1, 15, 15, 0, 0), Department = Department.Vers, EmployeeNumber = 3, IsFinal = false },
            new { Id = 23, Start = new DateTime(2025, 1, 16, 12, 0, 0), End = new DateTime(2025, 1, 16, 20, 0, 0), Department = Department.Kassa, EmployeeNumber = 3, IsFinal = false },
            new { Id = 24, Start = new DateTime(2025, 1, 17, 10, 0, 0), End = new DateTime(2025, 1, 17, 18, 0, 0), Department = Department.Kassa, EmployeeNumber = 3, IsFinal = false },

            // Employee 4
            new { Id = 25, Start = new DateTime(2025, 1, 6, 10, 0, 0), End = new DateTime(2025, 1, 6, 16, 0, 0), Department = Department.Vers, EmployeeNumber = 4, IsFinal = true },
            new { Id = 26, Start = new DateTime(2025, 1, 7, 11, 0, 0), End = new DateTime(2025, 1, 7, 17, 0, 0), Department = Department.Kassa, EmployeeNumber = 4, IsFinal = true },
            new { Id = 27, Start = new DateTime(2025, 1, 8, 12, 0, 0), End = new DateTime(2025, 1, 8, 18, 0, 0), Department = Department.Vakkenvullen, EmployeeNumber = 4, IsFinal = true },
            new { Id = 28, Start = new DateTime(2025, 1, 9, 10, 0, 0), End = new DateTime(2025, 1, 9, 16, 0, 0), Department = Department.Vers, EmployeeNumber = 4, IsFinal = true },
            new { Id = 29, Start = new DateTime(2025, 1, 10, 14, 0, 0), End = new DateTime(2025, 1, 10, 20, 0, 0), Department = Department.Kassa, EmployeeNumber = 4, IsFinal = true },
            new { Id = 30, Start = new DateTime(2025, 1, 11, 12, 0, 0), End = new DateTime(2025, 1, 11, 18, 0, 0), Department = Department.Vers, EmployeeNumber = 4, IsFinal = true },
            new { Id = 31, Start = new DateTime(2025, 1, 12, 11, 0, 0), End = new DateTime(2025, 1, 12, 17, 0, 0), Department = Department.Kassa, EmployeeNumber = 4, IsFinal = true },
            new { Id = 32, Start = new DateTime(2025, 1, 13, 9, 0, 0), End = new DateTime(2025, 1, 13, 15, 0, 0), Department = Department.Vakkenvullen, EmployeeNumber = 4, IsFinal = false },
            new { Id = 33, Start = new DateTime(2025, 1, 14, 13, 0, 0), End = new DateTime(2025, 1, 14, 19, 0, 0), Department = Department.Kassa, EmployeeNumber = 4, IsFinal = false },
            new { Id = 34, Start = new DateTime(2025, 1, 15, 10, 0, 0), End = new DateTime(2025, 1, 15, 16, 0, 0), Department = Department.Vers, EmployeeNumber = 4, IsFinal = false },

            // Employee 5
            new { Id = 35, Start = new DateTime(2025, 1, 6, 13, 0, 0), End = new DateTime(2025, 1, 6, 19, 0, 0), Department = Department.Vakkenvullen, EmployeeNumber = 5, IsFinal = true },
            new { Id = 36, Start = new DateTime(2025, 1, 7, 12, 0, 0), End = new DateTime(2025, 1, 7, 18, 0, 0), Department = Department.Kassa, EmployeeNumber = 5, IsFinal = true },
            new { Id = 37, Start = new DateTime(2025, 1, 8, 10, 0, 0), End = new DateTime(2025, 1, 8, 16, 0, 0), Department = Department.Vers, EmployeeNumber = 5, IsFinal = true },
            new { Id = 38, Start = new DateTime(2025, 1, 9, 14, 0, 0), End = new DateTime(2025, 1, 9, 20, 0, 0), Department = Department.Kassa, EmployeeNumber = 5, IsFinal = true },
            new { Id = 39, Start = new DateTime(2025, 1, 10, 15, 0, 0), End = new DateTime(2025, 1, 10, 21, 0, 0), Department = Department.Vakkenvullen, EmployeeNumber = 5, IsFinal = true },
            new { Id = 40, Start = new DateTime(2025, 1, 11, 13, 0, 0), End = new DateTime(2025, 1, 11, 19, 0, 0), Department = Department.Kassa, EmployeeNumber = 5, IsFinal = true },
            new { Id = 41, Start = new DateTime(2025, 1, 12, 12, 0, 0), End = new DateTime(2025, 1, 12, 18, 0, 0), Department = Department.Vers, EmployeeNumber = 5, IsFinal = true },
            new { Id = 42, Start = new DateTime(2025, 1, 13, 14, 0, 0), End = new DateTime(2025, 1, 13, 20, 0, 0), Department = Department.Kassa, EmployeeNumber = 5, IsFinal = false },
            new { Id = 43, Start = new DateTime(2025, 1, 14, 15, 0, 0), End = new DateTime(2025, 1, 14, 21, 0, 0), Department = Department.Vers, EmployeeNumber = 5, IsFinal = false },
            new { Id = 44, Start = new DateTime(2025, 1, 15, 13, 0, 0), End = new DateTime(2025, 1, 15, 19, 0, 0), Department = Department.Kassa, EmployeeNumber = 5, IsFinal = false },
            new { Id = 45, Start = new DateTime(2025, 1, 17, 13, 0, 0), End = new DateTime(2025, 1, 17, 19, 0, 0), Department = Department.Kassa, EmployeeNumber = 5, IsFinal = false },
            new { Id = 46, Start = new DateTime(2025, 1, 19, 13, 0, 0), End = new DateTime(2025, 1, 19, 19, 0, 0), Department = Department.Kassa, EmployeeNumber = 5, IsFinal = false },

            new { Id = 47, Start = new DateTime(2025, 1, 18, 9, 0, 0), End = new DateTime(2025, 1, 18, 15, 0, 0), Department = Department.Kassa, IsFinal = false },
            new { Id = 48, Start = new DateTime(2025, 1, 18, 15, 0, 0), End = new DateTime(2025, 1, 18, 21, 0, 0), Department = Department.Kassa, IsFinal = false },
            new { Id = 49, Start = new DateTime(2025, 1, 19, 9, 0, 0), End = new DateTime(2025, 1, 19, 12, 0, 0), Department = Department.Kassa, IsFinal = false }
        );

        modelBuilder.Entity<ShiftTakeOver>().HasData(
            new
            {
                ShiftId = 2,
                EmployeeTakingOverEmployeeNumber = 3,
                Status = Status.Aangevraagd,
            },
            new
            {
                ShiftId = 20,
                EmployeeTakingOverEmployeeNumber = 2,
                Status = Status.Afgewezen,
            },
            new
            {
                ShiftId = 21,
                Status = Status.Aangevraagd,
            }
        );

        modelBuilder.Entity<Shift>().HasData(
            new { Id = 101, Start = new DateTime(2024, 12, 2, 9, 0, 0), End = new DateTime(2024, 12, 2, 17, 0, 0), Department = Department.Vers, EmployeeNumber = 2, IsFinal = true },
            new { Id = 102, Start = new DateTime(2024, 12, 3, 10, 0, 0), End = new DateTime(2024, 12, 3, 18, 0, 0), Department = Department.Kassa, EmployeeNumber = 2, IsFinal = true },
            new { Id = 103, Start = new DateTime(2024, 12, 4, 8, 0, 0), End = new DateTime(2024, 12, 4, 16, 0, 0), Department = Department.Vakkenvullen, EmployeeNumber = 2, IsFinal = true },
            new { Id = 104, Start = new DateTime(2024, 12, 5, 11, 0, 0), End = new DateTime(2024, 12, 5, 19, 0, 0), Department = Department.Vers, EmployeeNumber = 2, IsFinal = true },
            new { Id = 105, Start = new DateTime(2024, 12, 6, 9, 0, 0), End = new DateTime(2024, 12, 6, 17, 0, 0), Department = Department.Kassa, EmployeeNumber = 2, IsFinal = true },
            new { Id = 106, Start = new DateTime(2024, 12, 2, 9, 0, 0), End = new DateTime(2024, 12, 2, 17, 0, 0), Department = Department.Vakkenvullen, EmployeeNumber = 3, IsFinal = true },
            new { Id = 107, Start = new DateTime(2024, 12, 3, 10, 0, 0), End = new DateTime(2024, 12, 3, 18, 0, 0), Department = Department.Kassa, EmployeeNumber = 3, IsFinal = true },
            new { Id = 108, Start = new DateTime(2024, 12, 4, 8, 0, 0), End = new DateTime(2024, 12, 4, 16, 0, 0), Department = Department.Vers, EmployeeNumber = 3, IsFinal = true },
            new { Id = 109, Start = new DateTime(2024, 12, 5, 11, 0, 0), End = new DateTime(2024, 12, 5, 19, 0, 0), Department = Department.Kassa, EmployeeNumber = 3, IsFinal = true },
            new { Id = 110, Start = new DateTime(2024, 12, 6, 9, 0, 0), End = new DateTime(2024, 12, 6, 17, 0, 0), Department = Department.Vakkenvullen, EmployeeNumber = 3, IsFinal = true },

            new { Id = 111, Start = new DateTime(2024, 12, 9, 9, 0, 0), End = new DateTime(2024, 12, 9, 17, 0, 0), Department = Department.Vers, EmployeeNumber = 2, IsFinal = true },
            new { Id = 112, Start = new DateTime(2024, 12, 10, 10, 0, 0), End = new DateTime(2024, 12, 10, 18, 0, 0), Department = Department.Kassa, EmployeeNumber = 2, IsFinal = true },
            new { Id = 113, Start = new DateTime(2024, 12, 11, 8, 0, 0), End = new DateTime(2024, 12, 11, 16, 0, 0), Department = Department.Vakkenvullen, EmployeeNumber = 2, IsFinal = true },
            new { Id = 114, Start = new DateTime(2024, 12, 12, 11, 0, 0), End = new DateTime(2024, 12, 12, 19, 0, 0), Department = Department.Vers, EmployeeNumber = 2, IsFinal = true },
            new { Id = 115, Start = new DateTime(2024, 12, 13, 9, 0, 0), End = new DateTime(2024, 12, 13, 17, 0, 0), Department = Department.Kassa, EmployeeNumber = 2, IsFinal = true },
            new { Id = 116, Start = new DateTime(2024, 12, 9, 9, 0, 0), End = new DateTime(2024, 12, 9, 17, 0, 0), Department = Department.Vakkenvullen, EmployeeNumber = 3, IsFinal = true },
            new { Id = 117, Start = new DateTime(2024, 12, 10, 10, 0, 0), End = new DateTime(2024, 12, 10, 18, 0, 0), Department = Department.Kassa, EmployeeNumber = 3, IsFinal = true },
            new { Id = 118, Start = new DateTime(2024, 12, 11, 8, 0, 0), End = new DateTime(2024, 12, 11, 16, 0, 0), Department = Department.Vers, EmployeeNumber = 3, IsFinal = true },
            new { Id = 119, Start = new DateTime(2024, 12, 12, 11, 0, 0), End = new DateTime(2024, 12, 12, 19, 0, 0), Department = Department.Kassa, EmployeeNumber = 3, IsFinal = true },
            new { Id = 120, Start = new DateTime(2024, 12, 13, 9, 0, 0), End = new DateTime(2024, 12, 13, 17, 0, 0), Department = Department.Vakkenvullen, EmployeeNumber = 3, IsFinal = true }
        );

        modelBuilder.Entity<WorkedHour>().HasData(
            new { Id = 1, DateOnly = new DateOnly(2024, 12, 2), StartTime = new TimeOnly(9, 15), EndTime = new TimeOnly(17, 10), Status = HourStatus.Final, EmployeeNumber = 2 },
            new { Id = 2, DateOnly = new DateOnly(2024, 12, 3), StartTime = new TimeOnly(10, 5), EndTime = new TimeOnly(18, 5), Status = HourStatus.Final, EmployeeNumber = 2 },
            new { Id = 3, DateOnly = new DateOnly(2024, 12, 4), StartTime = new TimeOnly(8, 10), EndTime = new TimeOnly(16, 5), Status = HourStatus.Final, EmployeeNumber = 2 },
            new { Id = 4, DateOnly = new DateOnly(2024, 12, 5), StartTime = new TimeOnly(11, 10), EndTime = new TimeOnly(19, 15), Status = HourStatus.Final, EmployeeNumber = 2 },
            new { Id = 5, DateOnly = new DateOnly(2024, 12, 6), StartTime = new TimeOnly(9, 5), EndTime = new TimeOnly(17, 5), Status = HourStatus.Final, EmployeeNumber = 2 },
            new { Id = 6, DateOnly = new DateOnly(2024, 12, 2), StartTime = new TimeOnly(9, 10), EndTime = new TimeOnly(17, 20), Status = HourStatus.Final, EmployeeNumber = 3 },
            new { Id = 7, DateOnly = new DateOnly(2024, 12, 3), StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(18, 5), Status = HourStatus.Final, EmployeeNumber = 3 },
            new { Id = 8, DateOnly = new DateOnly(2024, 12, 4), StartTime = new TimeOnly(8, 5), EndTime = new TimeOnly(16, 10), Status = HourStatus.Final, EmployeeNumber = 3 },
            new { Id = 9, DateOnly = new DateOnly(2024, 12, 5), StartTime = new TimeOnly(11, 10), EndTime = new TimeOnly(19, 0), Status = HourStatus.Final, EmployeeNumber = 3 },
            new { Id = 10, DateOnly = new DateOnly(2024, 12, 6), StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(17, 10), Status = HourStatus.Final, EmployeeNumber = 3 },

            new { Id = 11, DateOnly = new DateOnly(2024, 12, 9), StartTime = new TimeOnly(9, 20), EndTime = new TimeOnly(17, 15), Status = HourStatus.Concept, EmployeeNumber = 2 },
            new { Id = 12, DateOnly = new DateOnly(2024, 12, 10), StartTime = new TimeOnly(10, 10), EndTime = new TimeOnly(18, 10), Status = HourStatus.Concept, EmployeeNumber = 2 },
            new { Id = 13, DateOnly = new DateOnly(2024, 12, 11), StartTime = new TimeOnly(8, 20), EndTime = new TimeOnly(16, 10), Status = HourStatus.Concept, EmployeeNumber = 2 },
            new { Id = 14, DateOnly = new DateOnly(2024, 12, 12), StartTime = new TimeOnly(11, 15), EndTime = new TimeOnly(19, 20), Status = HourStatus.Concept, EmployeeNumber = 2 },
            new { Id = 15, DateOnly = new DateOnly(2024, 12, 13), StartTime = new TimeOnly(9, 15), EndTime = new TimeOnly(17, 10), Status = HourStatus.Concept, EmployeeNumber = 2 },
            new { Id = 16, DateOnly = new DateOnly(2024, 12, 9), StartTime = new TimeOnly(9, 15), EndTime = new TimeOnly(17, 20), Status = HourStatus.Concept, EmployeeNumber = 3 },
            new { Id = 17, DateOnly = new DateOnly(2024, 12, 10), StartTime = new TimeOnly(10, 15), EndTime = new TimeOnly(18, 10), Status = HourStatus.Concept, EmployeeNumber = 3 },
            new { Id = 18, DateOnly = new DateOnly(2024, 12, 11), StartTime = new TimeOnly(8, 10), EndTime = new TimeOnly(16, 15), Status = HourStatus.Concept, EmployeeNumber = 3 },
            new { Id = 19, DateOnly = new DateOnly(2024, 12, 12), StartTime = new TimeOnly(11, 20), EndTime = new TimeOnly(19, 10), Status = HourStatus.Concept, EmployeeNumber = 3 },
            new { Id = 20, DateOnly = new DateOnly(2024, 12, 13), StartTime = new TimeOnly(9, 10), EndTime = new TimeOnly(17, 20), Status = HourStatus.Concept, EmployeeNumber = 3 }
        );

        modelBuilder.Entity<Break>().HasData(
            new { Id = 1, StartTime = new TimeOnly(12, 0), EndTime = new TimeOnly(12, 30), WorkedHourId = 1 },
            new { Id = 2, StartTime = new TimeOnly(13, 0), EndTime = new TimeOnly(13, 30), WorkedHourId = 2 },
            new { Id = 3, StartTime = new TimeOnly(12, 15), EndTime = new TimeOnly(12, 45), WorkedHourId = 3 },
            new { Id = 4, StartTime = new TimeOnly(14, 30), EndTime = new TimeOnly(15, 0), WorkedHourId = 4 },
            new { Id = 5, StartTime = new TimeOnly(12, 0), EndTime = new TimeOnly(12, 30), WorkedHourId = 5 },
            new { Id = 6, StartTime = new TimeOnly(12, 15), EndTime = new TimeOnly(12, 45), WorkedHourId = 6 },
            new { Id = 7, StartTime = new TimeOnly(13, 30), EndTime = new TimeOnly(14, 0), WorkedHourId = 7 },
            new { Id = 8, StartTime = new TimeOnly(11, 45), EndTime = new TimeOnly(12, 15), WorkedHourId = 8 },
            new { Id = 9, StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(14, 30), WorkedHourId = 9 },
            new { Id = 10, StartTime = new TimeOnly(12, 15), EndTime = new TimeOnly(12, 45), WorkedHourId = 10 },

            new { Id = 21, WorkedHourId = 11, StartTime = new TimeOnly(12, 15), EndTime = new TimeOnly(12, 45) },
            new { Id = 22, WorkedHourId = 12, StartTime = new TimeOnly(13, 0), EndTime = new TimeOnly(13, 30) },
            new { Id = 23, WorkedHourId = 13, StartTime = new TimeOnly(12, 30), EndTime = new TimeOnly(13, 0) },
            new { Id = 24, WorkedHourId = 14, StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(14, 30) },
            new { Id = 25, WorkedHourId = 15, StartTime = new TimeOnly(12, 0), EndTime = new TimeOnly(12, 30) },
            new { Id = 26, WorkedHourId = 16, StartTime = new TimeOnly(12, 15), EndTime = new TimeOnly(12, 45) },
            new { Id = 27, WorkedHourId = 17, StartTime = new TimeOnly(13, 15), EndTime = new TimeOnly(13, 45) },
            new { Id = 28, WorkedHourId = 18, StartTime = new TimeOnly(11, 45), EndTime = new TimeOnly(12, 15) },
            new { Id = 29, WorkedHourId = 19, StartTime = new TimeOnly(14, 15), EndTime = new TimeOnly(14, 45) },
            new { Id = 30, WorkedHourId = 20, StartTime = new TimeOnly(12, 30), EndTime = new TimeOnly(13, 0) }
        );
    }
}
