namespace eco_edu_mvc.Models.SurveyModels;

public class ResponseModel
{
	public int ResponseId { get; set; }
	public int QuestionId { get; set; }
	public string? Answer { get; set; }
}
