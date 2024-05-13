using System;
using System.Collections.Generic;

namespace eco_edu_mvc.Models.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string UserCode { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public bool IsAccept { get; set; }

    public DateTime EntryDate { get; set; }

    public string? Email { get; set; }

    public bool? EmailVerify { get; set; }

    public string? Role { get; set; }

    public string? Section { get; set; }

    public string? Class { get; set; }

    public string? CitizenId { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? VerificationToken { get; set; }

    public DateTime? TokenExpiry { get; set; }

    public string? Images { get; set; }

    public virtual ICollection<CompetitionEntry> CompetitionEntries { get; set; } = new List<CompetitionEntry>();

    public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();

    public virtual ICollection<Seminar> Seminars { get; set; } = new List<Seminar>();

    public virtual ICollection<Survey> Surveys { get; set; } = new List<Survey>();
}
