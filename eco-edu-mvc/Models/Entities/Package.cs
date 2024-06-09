using System;
using System.Collections.Generic;

namespace eco_edu_mvc.Models.Entities;

public partial class Package
{
    public int PackId { get; set; }

    public string PackName { get; set; } = null!;

    public decimal Price { get; set; }

    public decimal? Discount { get; set; }

    public DateTime Duration { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual ICollection<AssignedPackage> AssignedPackages { get; set; } = new List<AssignedPackage>();
}
