using System;
using System.Collections.Generic;

namespace eco_edu_mvc.Models.Entities;

public partial class FileMsg
{
    public Guid MessageId { get; set; }

    public string FileName { get; set; } = null!;

    public virtual Message Message { get; set; } = null!;
}
