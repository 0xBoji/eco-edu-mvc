using System;
using System.Collections.Generic;

namespace eco_edu_mvc.Models.Entities;

public partial class Member
{
    public int MemberId { get; set; }

    public string Username { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<AssignedPackage> AssignedPackages { get; set; } = new List<AssignedPackage>();

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
}
