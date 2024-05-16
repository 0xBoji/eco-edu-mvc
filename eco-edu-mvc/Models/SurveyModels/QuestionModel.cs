using System.ComponentModel.DataAnnotations;

namespace eco_edu_mvc.Models.SurveyModels;
public class QuestionModel
{
	[Key]
	public int QuestionId { get; set; }
	public int SurveyId { get; set; }
	public string QuestionText { get; set; } = null!;
	public string? QuestionType { get; set; }
}
