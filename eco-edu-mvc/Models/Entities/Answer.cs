using System;
using System.Collections.Generic;

namespace eco_edu_mvc.Models.Entities;

public partial class Answer
{
    public int AnswerId { get; set; }

    public int? ResponseId { get; set; }

    public string? Answer1 { get; set; }

    public virtual Response? Response { get; set; }
}
