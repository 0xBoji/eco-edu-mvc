using eco_edu_mvc.Models.Entities; // Ensure this namespace is included
namespace eco_edu_mvc.Models.HomeViewModel;


public class ChatViewModel
{
    public Guid GroupId { get; set; }
    public string GroupName { get; set; }
    public List<Message> Messages { get; set; } = new List<Message>();
}