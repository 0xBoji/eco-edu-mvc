using System;
using System.Collections.Generic;

namespace eco_edu_mvc.Models.Entities;

public partial class CardPay
{
    public int PaymentId { get; set; }

    public int PackageId { get; set; }

    public string BankName { get; set; } = null!;

    public string CardNumber { get; set; } = null!;

    public DateTime ExpDate { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public virtual AssignedPackage Package { get; set; } = null!;
}
