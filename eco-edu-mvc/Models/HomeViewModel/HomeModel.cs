using eco_edu_mvc.Models.Entities;

namespace eco_edu_mvc.Models.HomeViewModel;

public class HomeModel
{
    public List<Survey> Surveys { get; set; }
    public List<Competition> Competitions { get; set; }
    public string Username { get; set; }
    public string UserId { get; set; }
}
