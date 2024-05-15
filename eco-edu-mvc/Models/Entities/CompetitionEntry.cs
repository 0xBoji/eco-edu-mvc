using System;
using System.Collections.Generic;

namespace eco_edu_mvc.Models.Entities;

public partial class CompetitionEntry
{
    public int EntryId { get; set; }

    public int UserId { get; set; }

    public int CompetitionId { get; set; }

    public string? SubmissionText { get; set; }

    public DateTime SubmissionDate { get; set; }

    public virtual Competition Competition { get; set; } = null!;

    public virtual ICollection<GradeTest> GradeTests { get; set; } = new List<GradeTest>();

    public virtual User User { get; set; } = null!;
}
