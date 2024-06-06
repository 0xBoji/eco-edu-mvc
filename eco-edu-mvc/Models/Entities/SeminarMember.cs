using System;
using System.Collections.Generic;

namespace eco_edu_mvc.Models.Entities;

public partial class SeminarMember
{
    public int SmId { get; set; }

    public int UserId { get; set; }

    public int? SeminarId { get; set; }

    public virtual ICollection<Seminar> Seminars { get; set; } = new List<Seminar>();

    public virtual Seminar Seminar { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
