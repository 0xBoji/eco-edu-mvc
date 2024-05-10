using System;
using System.Collections.Generic;

namespace eco_edu_mvc.Models.Entities;

public partial class Contact
{
    public int ContactId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime LastestUpdate { get; set; }
}
