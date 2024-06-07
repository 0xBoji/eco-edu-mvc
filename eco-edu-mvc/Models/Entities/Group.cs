using System;
using System.Collections.Generic;

namespace eco_edu_mvc.Models.Entities;

public partial class Group
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
