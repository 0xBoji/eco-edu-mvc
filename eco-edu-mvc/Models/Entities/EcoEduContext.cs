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

    public virtual DbSet<Response> Responses { get; set; }

    public virtual DbSet<Seminar> Seminars { get; set; }

    public virtual DbSet<Survey> Surveys { get; set; }

    public virtual DbSet<SurveyQuestion> SurveyQuestions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-SMPR88L; Initial Catalog=eco-edu;Persist Security Info=True;User ID=pich;Password=123456;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Competition>(entity =>
        {
            entity.HasKey(e => e.CompetitionId).HasName("PK__Competit__87D31213AA357345");

            entity.Property(e => e.CompetitionId).HasColumnName("Competition_Id");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("End_Date");
            entity.Property(e => e.Prizes).HasColumnType("text");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("Start_Date");
            entity.Property(e => e.Title).HasColumnType("text");
        });

        modelBuilder.Entity<CompetitionEntry>(entity =>
        {
            entity.HasKey(e => e.EntryId).HasName("PK__Competit__41CE7C8C32213C61");

            entity.ToTable("Competition_Entries");

            entity.Property(e => e.EntryId).HasColumnName("Entry_Id");
            entity.Property(e => e.CompetitionId).HasColumnName("Competition_Id");
            entity.Property(e => e.Score).HasColumnType("decimal(10, 2)");
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
                .HasConstraintName("FK__Competiti__Compe__4CA06362");

            entity.HasOne(d => d.User).WithMany(p => p.CompetitionEntries)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Competiti__UserI__4BAC3F29");
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.ContactId).HasName("PK__Contacts__82ACC1EDD09B4555");

            entity.Property(e => e.ContactId).HasColumnName("Contact_Id");
            entity.Property(e => e.Content).HasColumnType("text");
            entity.Property(e => e.LastestUpdate)
                .HasColumnType("datetime")
                .HasColumnName("Lastest_Update");
        });

        modelBuilder.Entity<Faq>(entity =>
        {
            entity.HasKey(e => e.FaqId).HasName("PK__FAQs__83815494094BCC30");

            entity.ToTable("FAQs");

            entity.Property(e => e.FaqId).HasColumnName("FAQ_Id");
            entity.Property(e => e.Answer).HasColumnType("text");
            entity.Property(e => e.Question).IsUnicode(false);
        });

        modelBuilder.Entity<Response>(entity =>
        {
            entity.HasKey(e => e.ResponseId).HasName("PK__Response__B736E9340D04D0E9");

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
            entity.HasKey(e => e.SeminarId).HasName("PK__Seminars__E0812679B93775B9");

            entity.Property(e => e.SeminarId).HasColumnName("Seminar_Id");
            entity.Property(e => e.Location).HasColumnType("text");
            entity.Property(e => e.OccursDate)
                .HasColumnType("datetime")
                .HasColumnName("Occurs_Date");
            entity.Property(e => e.Title).HasColumnType("text");

            entity.HasOne(d => d.User).WithMany(p => p.Seminars)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Seminars__Occurs__534D60F1");
        });

        modelBuilder.Entity<Survey>(entity =>
        {
            entity.HasKey(e => e.SurveyId).HasName("PK__Surveys__6C04F454E2D8E6C1");

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

        modelBuilder.Entity<SurveyQuestion>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK__Survey_Q__B0B2E4E6C7AC1FA1");

            entity.ToTable("Survey_Questions");

            entity.Property(e => e.QuestionId).HasColumnName("Question_Id");
            entity.Property(e => e.Question).IsUnicode(false);
            entity.Property(e => e.QuestionType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Question_Type");
            entity.Property(e => e.SurveyId).HasColumnName("Survey_Id");

            entity.HasOne(d => d.Survey).WithMany(p => p.SurveyQuestions)
                .HasForeignKey(d => d.SurveyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Survey_Qu__Quest__4316F928");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C67569BF0");

            entity.HasIndex(e => e.UserCode, "UQ__Users__3E6D1F340663A2CC").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4BA3E4B24").IsUnique();

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
            entity.Property(e => e.IsAccept).HasColumnName("Is_Accept");
            entity.Property(e => e.Password)
                .HasMaxLength(256)
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
