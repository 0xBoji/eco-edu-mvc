namespace eco_edu_mvc.Models.SurveysViewModel;

public class SurveyModel
{
	public int SurveyId { get; set; }
	public int AccessId { get; set; }
	public string Title { get; set; } = null!;
	public string Topic { get; set; } = null!;
	public string? CreatedBy { get; set; }
	public DateTime? CreateDate { get; set; }
	public DateTime EndDate { get; set; }
	public string TargetAudience { get; set; } = null!;
	public bool Active { get; set; }
}
