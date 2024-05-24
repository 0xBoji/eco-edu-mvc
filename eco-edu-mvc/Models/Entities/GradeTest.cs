using System;
using System.Collections.Generic;

namespace eco_edu_mvc.Models.Entities;

public partial class GradeTest
{
    public int GradeId { get; set; }

    public int EntryId { get; set; }

    public decimal Score { get; set; }

    public DateTime? GradeDate { get; set; }

    public int GradedBy { get; set; }

    public virtual CompetitionEntry Entry { get; set; } = null!;

    public virtual User GradedByNavigation { get; set; } = null!;
}
