using System;
using System.Collections.Generic;

namespace eco_edu_mvc.Models.Entities;

public partial class Attendance
{
    public int AttendanceId { get; set; }

    public int Class { get; set; }

    public int Attenders { get; set; }

    public DateTime? CheckIn { get; set; }

    public virtual Member AttendersNavigation { get; set; } = null!;

    public virtual Class ClassNavigation { get; set; } = null!;
}
