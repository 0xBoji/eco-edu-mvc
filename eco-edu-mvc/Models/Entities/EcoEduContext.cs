using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace eco_edu_mvc.Models.Entities;

public partial class EcoEduContext : DbContext
{
    public EcoEduContext()
    {
    }

    public EcoEduContext(DbContextOptions<EcoEduContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Competition> Competitions { get; set; }

    public virtual DbSet<CompetitionEntry> CompetitionEntries { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Faq> Faqs { get; set; }

    public virtual DbSet<GradeTest> GradeTests { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Response> Responses { get; set; }

    public virtual DbSet<Seminar> Seminars { get; set; }

    public virtual DbSet<Survey> Surveys { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer("Data Source=(local); Initial Catalog=eco_edu;Persist Security Info=True;User ID=sa;Password=123;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Competition>(entity =>
        {
            entity.HasKey(e => e.CompetitionId).HasName("PK__Competit__87D31213D3BBBBD5");

            entity.Property(e => e.CompetitionId).HasColumnName("Competition_Id");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("End_Date");
            entity.Property(e => e.Prizes).HasColumnType("text");
            entity.Property(e => e.StartDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Start_Date");
            entity.Property(e => e.Title).HasColumnType("text");
        });

        modelBuilder.Entity<CompetitionEntry>(entity =>
        {
            entity.HasKey(e => e.EntryId).HasName("PK__Competit__41CE7C8C2B641C46");

            entity.ToTable("Competition_Entries");

            entity.Property(e => e.EntryId).HasColumnName("Entry_Id");
            entity.Property(e => e.CompetitionId).HasColumnName("Competition_Id");
            entity.Property(e => e.SubmissionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Submission_Date");
            entity.Property(e => e.SubmissionText)
                .HasColumnType("text")
                .HasColumnName("Submission_Text");

            entity.HasOne(d => d.Competition).WithMany(p => p.CompetitionEntries)
                .HasForeignKey(d => d.CompetitionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Competiti__Compe__4D94879B");

            entity.HasOne(d => d.User).WithMany(p => p.CompetitionEntries)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Competiti__UserI__4CA06362");
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.ContactId).HasName("PK__Contacts__82ACC1EDBC8456DC");

            entity.Property(e => e.ContactId).HasColumnName("Contact_Id");
            entity.Property(e => e.Content).HasColumnType("text");
            entity.Property(e => e.LastestUpdate)
                .HasColumnType("datetime")
                .HasColumnName("Lastest_Update");

            entity.HasOne(d => d.User).WithMany(p => p.Contacts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Contacts__UserId__5629CD9C");
        });

        modelBuilder.Entity<Faq>(entity =>
        {
            entity.HasKey(e => e.FaqId).HasName("PK__FAQs__838154940A3D1AD6");

            entity.ToTable("FAQs");

            entity.Property(e => e.FaqId).HasColumnName("FAQ_Id");
            entity.Property(e => e.Answer).HasColumnType("text");
            entity.Property(e => e.Question).IsUnicode(false);
        });

        modelBuilder.Entity<GradeTest>(entity =>
        {
            entity.HasKey(e => e.GradeId).HasName("PK__Grade_Te__D4437133D2D6B704");

            entity.ToTable("Grade_Test");

            entity.Property(e => e.GradeId).HasColumnName("Grade_Id");
            entity.Property(e => e.EntryId).HasColumnName("Entry_Id");
            entity.Property(e => e.GradeDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Grade_Date");
            entity.Property(e => e.Score).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Entry).WithMany(p => p.GradeTests)
                .HasForeignKey(d => d.EntryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Grade_Tes__Entry__5165187F");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK__Question__B0B2E4E6CBE73B6F");

            entity.Property(e => e.QuestionId).HasColumnName("Question_Id");
            entity.Property(e => e.QuestionText)
                .IsUnicode(false)
                .HasColumnName("Question_Text");
            entity.Property(e => e.QuestionType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Question_Type");
            entity.Property(e => e.SurveyId).HasColumnName("Survey_Id");

            entity.HasOne(d => d.Survey).WithMany(p => p.Questions)
                .HasForeignKey(d => d.SurveyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Questions__Surve__4316F928");
        });

        modelBuilder.Entity<Response>(entity =>
        {
            entity.HasKey(e => e.ResponseId).HasName("PK__Response__B736E934689B1F01");

            entity.ToTable("Response");

            entity.Property(e => e.ResponseId).HasColumnName("Response_Id");
            entity.Property(e => e.Answer).HasColumnType("text");
            entity.Property(e => e.QuestionId).HasColumnName("Question_Id");

            entity.HasOne(d => d.Question).WithMany(p => p.Responses)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Response__Questi__45F365D3");
        });

        modelBuilder.Entity<Seminar>(entity =>
        {
            entity.HasKey(e => e.SeminarId).HasName("PK__Seminars__E0812679E03B2560");

            entity.Property(e => e.SeminarId).HasColumnName("Seminar_Id");
            entity.Property(e => e.Location).HasColumnType("text");
            entity.Property(e => e.OccursDate)
                .HasColumnType("datetime")
                .HasColumnName("Occurs_Date");
            entity.Property(e => e.Title).HasColumnType("text");

            entity.HasOne(d => d.User).WithMany(p => p.Seminars)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Seminars__UserId__59063A47");
        });

        modelBuilder.Entity<Survey>(entity =>
        {
            entity.HasKey(e => e.SurveyId).HasName("PK__Surveys__6C04F4549381B20E");

            entity.Property(e => e.SurveyId).HasColumnName("Survey_Id");
            entity.Property(e => e.AccessId).HasColumnName("Access_Id");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Create_Date");
            entity.Property(e => e.CreatedBy)
                .IsUnicode(false)
                .HasColumnName("Created_By");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("End_Date");
            entity.Property(e => e.TargetAudience)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("Target_Audience");
            entity.Property(e => e.Title).HasColumnType("text");
            entity.Property(e => e.Topic)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Access).WithMany(p => p.Surveys)
                .HasForeignKey(d => d.AccessId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Surveys__Access___3E52440B");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CB81C4BB4");

            entity.HasIndex(e => e.UserCode, "UQ__Users__3E6D1F346E0F7741").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4FEF08231").IsUnique();

            entity.Property(e => e.CitizenId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Citizen_Id");
            entity.Property(e => e.Class)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Create_Date");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmailVerify).HasColumnName("Email_Verify");
            entity.Property(e => e.EntryDate)
                .HasColumnType("datetime")
                .HasColumnName("Entry_Date");
            entity.Property(e => e.Fullname)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Images)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IsAccept).HasColumnName("Is_Accept");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(7)
                .IsUnicode(false);
            entity.Property(e => e.Section)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TokenExpiry)
                .HasColumnType("datetime")
                .HasColumnName("Token_Expiry");
            entity.Property(e => e.UserCode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("User_Code");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VerificationToken)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Verification_Token");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
