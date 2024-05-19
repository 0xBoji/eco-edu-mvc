using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace eco_edu_mvc.Models.SurveyModels;
public class QuestionModel
{
	[Key]
	public int QuestionId { get; set; }
	public int SurveyId { get; set; }
	public string QuestionText { get; set; } = null!;
	public string? QuestionType { get; set; }
	public string? Answer1 { get; set; }
    public string? Answer2 { get; set; }
    public string? Answer3 { get; set; }
    public string? CorrectAnswer { get; set; }

    public List<SelectListItem> Surveys { get; set; } = [];
}
