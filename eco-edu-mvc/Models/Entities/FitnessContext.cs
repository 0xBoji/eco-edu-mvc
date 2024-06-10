using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace eco_edu_mvc.Models.Entities;

public partial class FitnessContext : DbContext
{
    public FitnessContext()
    {
    }

    public FitnessContext(DbContextOptions<FitnessContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<AssignedPackage> AssignedPackages { get; set; }

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<CardPay> CardPays { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Package> Packages { get; set; }

    public virtual DbSet<Trainer> Trainers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(local); Initial Catalog=Fitness;Persist Security Info=True;User ID=sa;Password=123;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admins__719FE4E811AA7C6F");

            entity.HasIndex(e => e.Username, "UQ__Admins__536C85E470153118").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Admins__A9D10534C2E88F5C").IsUnique();

            entity.Property(e => e.AdminId).HasColumnName("AdminID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AssignedPackage>(entity =>
        {
            entity.HasKey(e => e.AssignedId).HasName("PK__Assigned__4A3E39390EB73726");

            entity.ToTable("AssignedPackage");

            entity.Property(e => e.AssignedId).HasColumnName("AssignedID");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.PackId).HasColumnName("PackID");
            entity.Property(e => e.StartDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MemberNavigation).WithMany(p => p.AssignedPackages)
                .HasForeignKey(d => d.Member)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AssignedP__Membe__59063A47");

            entity.HasOne(d => d.Pack).WithMany(p => p.AssignedPackages)
                .HasForeignKey(d => d.PackId)
                .HasConstraintName("FK__AssignedP__PackI__5812160E");
        });

        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasKey(e => e.AttendanceId).HasName("PK__Attendan__8B69263C7C69320A");

            entity.Property(e => e.AttendanceId).HasColumnName("AttendanceID");
            entity.Property(e => e.CheckIn).HasColumnType("datetime");

            entity.HasOne(d => d.AttendersNavigation).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.Attenders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Attendanc__Atten__4CA06362");

            entity.HasOne(d => d.ClassNavigation).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.Class)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Attendanc__Class__4BAC3F29");
        });

        modelBuilder.Entity<CardPay>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__CardPay__9B556A589941B009");

            entity.ToTable("CardPay");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.BankName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CardNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ExpDate).HasColumnType("datetime");
            entity.Property(e => e.PackageId).HasColumnName("PackageID");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Package).WithMany(p => p.CardPays)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CardPay__Package__5CD6CB2B");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PK__Classes__CB1927A0385B08A7");

            entity.Property(e => e.ClassId).HasColumnName("ClassID");
            entity.Property(e => e.ClassName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.TrainerNavigation).WithMany(p => p.Classes)
                .HasForeignKey(d => d.Trainer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Classes__Trainer__48CFD27E");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__6A4BEDF6E14F1BC6");

            entity.Property(e => e.FeedbackId).HasColumnName("FeedbackID");
            entity.Property(e => e.Comments).HasColumnType("text");

            entity.HasOne(d => d.TrainerNavigation).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.Trainer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedbacks__Train__5070F446");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PK__Members__0CF04B38CDC7C81B");

            entity.HasIndex(e => e.Username, "UQ__Members__536C85E4DE0E9C93").IsUnique();

            entity.HasIndex(e => e.Phone, "UQ__Members__5C7E359EF703F4E5").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Members__A9D105348987863E").IsUnique();

            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Package>(entity =>
        {
            entity.HasKey(e => e.PackId).HasName("PK__Packages__FA676549B6B258DF");

            entity.HasIndex(e => e.PackName, "UQ__Packages__01212744E85E387A").IsUnique();

            entity.Property(e => e.PackId).HasColumnName("PackID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Discount).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Duration).HasColumnType("datetime");
            entity.Property(e => e.PackName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Trainer>(entity =>
        {
            entity.HasKey(e => e.TrainerId).HasName("PK__Trainers__366A1B9C7832716C");

            entity.HasIndex(e => e.Username, "UQ__Trainers__536C85E43C8E193B").IsUnique();

            entity.HasIndex(e => e.Phone, "UQ__Trainers__5C7E359EC2AF8037").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Trainers__A9D10534E36A65A0").IsUnique();

            entity.Property(e => e.TrainerId).HasColumnName("TrainerID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
