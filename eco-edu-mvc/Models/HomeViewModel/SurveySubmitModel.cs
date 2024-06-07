namespace eco_edu_mvc.Models.HomeViewModel;
public class SurveySubmitModel
{
    public int ResponseId { get; set; }
    public required int QuestionId { get; set; }
    public required int UserId { get; set; }
    public string? Answer { get; set; }
}
