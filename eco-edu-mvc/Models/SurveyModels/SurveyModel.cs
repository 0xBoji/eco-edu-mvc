using System.ComponentModel.DataAnnotations;

namespace eco_edu_mvc.Models.SurveyModels;
public class SurveyModel
{
    [Key]
    public int SurveyId { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; } = null!;
    public string Topic { get; set; } = null!;
    public DateTime CreateDate { get; set; }
    public DateTime EndDate { get; set; }
    public string TargetAudience { get; set; } = null!;
    public bool Active { get; set; }
}