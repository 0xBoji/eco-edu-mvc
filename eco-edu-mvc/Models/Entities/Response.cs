using System;
using System.Collections.Generic;

namespace eco_edu_mvc.Models.Entities;

public partial class Response
{
    public int ResponseId { get; set; }

    public required int UserId { get; set; }

    public required int QuestionId { get; set; }

    public string? Answer { get; set; }

    public virtual Question Question { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
