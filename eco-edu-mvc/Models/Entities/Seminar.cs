using System;
using System.Collections.Generic;

namespace eco_edu_mvc.Models.Entities;

public partial class Seminar
{
    public int SeminarId { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; } = null!;

    public string Location { get; set; } = null!;

    public int? Participants { get; set; }

    public DateTime? OccursDate { get; set; }

    public virtual User User { get; set; } = null!;
}
