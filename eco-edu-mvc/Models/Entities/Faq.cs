using System;
using System.Collections.Generic;

namespace eco_edu_mvc.Models.Entities;

public partial class Faq
{
    public int FaqId { get; set; }

    public string Question { get; set; } = null!;

    public string Answer { get; set; } = null!;
}
