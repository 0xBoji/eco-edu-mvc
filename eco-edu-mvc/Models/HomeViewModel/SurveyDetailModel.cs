using eco_edu_mvc.Models.Entities;

namespace eco_edu_mvc.Models.HomeViewModel;

public class SurveyDetailModel
{
    public Survey Survey { get; set; }
    public List<Question> Questions { get; set; }
}
