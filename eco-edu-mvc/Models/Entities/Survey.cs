using System;
using System.Collections.Generic;

namespace eco_edu_mvc.Models.Entities;

public partial class Survey
{
    public int SurveyId { get; set; }

    public int AccessId { get; set; }

    public string Title { get; set; } = null!;

    public string Topic { get; set; } = null!;

    public string? CreatedBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime EndDate { get; set; }

    public string TargetAudience { get; set; } = null!;

    public bool Active { get; set; }

    public virtual User Access { get; set; } = null!;

    public virtual ICollection<SurveyQuestion> SurveyQuestions { get; set; } = new List<SurveyQuestion>();
}
