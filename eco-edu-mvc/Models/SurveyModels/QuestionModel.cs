namespace eco_edu_mvc.Models.SurveyModels;

public class QuestionModel
{
	public int QuestionId { get; set; }
	public int SurveyId { get; set; }
	public string QuestionText { get; set; } = null!;
	public string? QuestionType { get; set; }
}
