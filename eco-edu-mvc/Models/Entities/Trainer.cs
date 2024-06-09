using System;
using System.Collections.Generic;

namespace eco_edu_mvc.Models.Entities;

public partial class Trainer
{
    public int TrainerId { get; set; }

    public string Username { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public string? Image { get; set; }

    public string? Status { get; set; }

    public bool? BeTrainer { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
}
