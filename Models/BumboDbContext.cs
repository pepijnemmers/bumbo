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
            .Property<string>("password").IsRequired();
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
