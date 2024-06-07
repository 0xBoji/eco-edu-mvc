using Microsoft.AspNetCore.SignalR;
using eco_edu_mvc.Models.Entities;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    private readonly EcoEduContext _context;

    public ChatHub(EcoEduContext context)
    {
        _context = context;
    }

    public async Task SendMessage(string userId, string username, string content)
    {
        var message = new Message
        {
            UserId = int.Parse(userId),
            Content = content,
            CreatedAt = DateTime.Now
        };

        //_context.Messages.Add(message);
        await _context.SaveChangesAsync();

        await Clients.All.SendAsync("ReceiveMessage", username, content);
    }
}
