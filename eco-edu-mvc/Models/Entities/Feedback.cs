using System;
using System.Collections.Generic;

namespace eco_edu_mvc.Models.Entities;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int Trainer { get; set; }

    public string Comments { get; set; } = null!;

    public int? Rating { get; set; }

    public virtual Trainer TrainerNavigation { get; set; } = null!;
}
