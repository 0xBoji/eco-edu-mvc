using System;
using System.Collections.Generic;

namespace eco_edu_mvc.Models.Entities;

public partial class Competition
{
    public int CompetitionId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool? Active { get; set; }

    public string? Prizes { get; set; }

    public string? Images { get; set; }

    public int CreatorId { get; set; }

    public virtual ICollection<CompetitionEntry> CompetitionEntries { get; set; } = new List<CompetitionEntry>();

    public virtual User Creator { get; set; } = null!;
}
