using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace eco_edu_mvc.Models.SurveyModels;
public class ResponseModel
{
    [Key]
    public int ResponseId { get; set; }
    public int QuestionId { get; set; }
    public string? Answer { get; set; }
    public List<SelectListItem> Questions { get; set; } = [];
}
