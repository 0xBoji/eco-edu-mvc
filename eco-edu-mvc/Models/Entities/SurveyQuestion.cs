using System;
using System.Collections.Generic;

namespace eco_edu_mvc.Models.Entities;

public partial class SurveyQuestion
{
    public int QuestionId { get; set; }

    public int SurveyId { get; set; }

    public string Question { get; set; } = null!;

    public string? QuestionType { get; set; }

    public virtual ICollection<Response> Responses { get; set; } = new List<Response>();

    public virtual Survey Survey { get; set; } = null!;
}
