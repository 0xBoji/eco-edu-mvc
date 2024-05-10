using System;
using System.Collections.Generic;

namespace eco_edu_mvc.Models.Entities;

public partial class Response
{
    public int ResponseId { get; set; }

    public int QuestionId { get; set; }

    public string? Answer { get; set; }

    public virtual SurveyQuestion Question { get; set; } = null!;
}
