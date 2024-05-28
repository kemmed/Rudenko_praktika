using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Rudenko_praktika.Models;

public partial class PraktikaContext : DbContext
{
    public PraktikaContext()
    {
    }

    public PraktikaContext(DbContextOptions<PraktikaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Fountain> Fountains { get; set; }

    public virtual DbSet<Park> Parks { get; set; }

    public virtual DbSet<Pavilion> Pavilions { get; set; }

    public virtual DbSet<Planting> Plantings { get; set; }

    public virtual DbSet<PlantingPark> PlantingParks { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=KEMMED\\SQLEXPRESS;Initial Catalog=paktika;Integrated Security=True;Connect Timeout=30;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Fountain>(entity =>
        {
            entity.ToTable("FOUNTAIN");

            entity.Property(e => e.FountainId).HasColumnName("fountain_ID");
            entity.Property(e => e.ConstructionDate).HasColumnName("construction_date");
            entity.Property(e => e.FountainArea)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("fountain_area");
            entity.Property(e => e.FountainCipher).HasColumnName("fountain_cipher");
            entity.Property(e => e.MaximumWaterConsumption)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("maximum_water_consumption");
            entity.Property(e => e.NormalWaterConsumption)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("normal_water_consumption");
            entity.Property(e => e.ParkId).HasColumnName("park_ID");

            entity.HasOne(d => d.Park).WithMany(p => p.Fountains)
                .HasForeignKey(d => d.ParkId)
                .HasConstraintName("FK_FOUNTAIN_PARK");
        });

        modelBuilder.Entity<Park>(entity =>
        {
            entity.ToTable("PARK");

            entity.Property(e => e.ParkId).HasColumnName("park_ID");
            entity.Property(e => e.Building)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("building");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("city");
            entity.Property(e => e.Index).HasColumnName("index");
            entity.Property(e => e.ParkArea)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("park_area");
            entity.Property(e => e.ParkClosure).HasColumnName("park_closure");
            entity.Property(e => e.ParkName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("park_name");
            entity.Property(e => e.ParkOpening).HasColumnName("park_opening");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("phone_number");
            entity.Property(e => e.PlantingDensity)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("planting_density");
            entity.Property(e => e.Street)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("street");
            entity.Property(e => e.Website)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("website");
        });

        modelBuilder.Entity<Pavilion>(entity =>
        {
            entity.ToTable("PAVILION");

            entity.Property(e => e.PavilionId).HasColumnName("pavilion_ID");
            entity.Property(e => e.ParkId).HasColumnName("park_ID");
            entity.Property(e => e.PavilionArea)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("pavilion_area");
            entity.Property(e => e.PavilionName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("pavilion_name");
            entity.Property(e => e.PavilionType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("pavilion_type");

            entity.HasOne(d => d.Park).WithMany(p => p.Pavilions)
                .HasForeignKey(d => d.ParkId)
                .HasConstraintName("FK_PAVILION_PARK");
        });

        modelBuilder.Entity<Planting>(entity =>
        {
            entity.ToTable("PLANTING");

            entity.Property(e => e.PlantingId).HasColumnName("planting_ID");
            entity.Property(e => e.AverageLifeExpectancy)
                .HasColumnType("decimal(5, 1)")
                .HasColumnName("average_life_expectancy");
            entity.Property(e => e.CultureType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("culture_type");
            entity.Property(e => e.PlantingName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("planting_name");
        });

        modelBuilder.Entity<PlantingPark>(entity =>
        {
            entity.HasKey(e => e.PlParkId);

            entity.ToTable("PLANTING_PARK");

            entity.Property(e => e.PlParkId).HasColumnName("pl_park_ID");
            entity.Property(e => e.ParkId).HasColumnName("park_ID");
            entity.Property(e => e.PlantingId).HasColumnName("planting_ID");
            entity.Property(e => e.PlantingsNumber).HasColumnName("plantings_number");

            entity.HasOne(d => d.Park).WithMany(p => p.PlantingParks)
                .HasForeignKey(d => d.ParkId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PLANTING_PARK_PARK");

            entity.HasOne(d => d.Planting).WithMany(p => p.PlantingParks)
                .HasForeignKey(d => d.PlantingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PLANTING_PARK_PLANTING");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("ROLE");

            entity.Property(e => e.RoleId).HasColumnName("role_ID");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("USER");

            entity.HasIndex(e => e.RoleId, "IX_USER_RoleId");

            entity.Property(e => e.UserId).HasColumnName("user_ID");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("role_ID");
            entity.Property(e => e.UserLogin)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("user_login");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_USER_ROLE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
