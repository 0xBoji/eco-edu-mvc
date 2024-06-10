using System;
using System.Collections.Generic;

namespace eco_edu_mvc.Models.Entities;

public partial class AssignedPackage
{
    public int AssignedId { get; set; }

    public int? PackId { get; set; }

    public int Member { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public virtual ICollection<CardPay> CardPays { get; set; } = new List<CardPay>();

    public virtual Member MemberNavigation { get; set; } = null!;

    public virtual Package? Pack { get; set; }
}
