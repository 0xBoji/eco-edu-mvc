using System;
using System.Collections.Generic;

namespace eco_edu_mvc.Models.Entities;

public partial class Message
{
    public Guid Id { get; set; }

    public int UserId { get; set; }

    public string Content { get; set; } = null!;

    public string? Type { get; set; }

    public DateTime? CreatedAt { get; set; }

    public Guid? GroupId { get; set; }

    public virtual FileMsg? FileMsg { get; set; }

    public virtual User User { get; set; } = null!;
}
