using System;
using System.Collections.Generic;

namespace eco_edu_mvc.Models.Entities;

public partial class Survey
{
    public int SurveyId { get; set; }

    public string Title { get; set; } = null!;

    public string Topic { get; set; } = null!;

    public DateTime? CreateDate { get; set; }

    public DateTime EndDate { get; set; }

    public string TargetAudience { get; set; } = null!;

    public string? Images { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
