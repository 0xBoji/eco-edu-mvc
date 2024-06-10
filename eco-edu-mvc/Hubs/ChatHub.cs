using Microsoft.AspNetCore.SignalR;
using eco_edu_mvc.Models.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;

public class ChatHub : Hub
{
    private readonly EcoEduContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ChatHub(EcoEduContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task SendMessage(string groupName, string content)
    {
        var userId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
        if (userId == null)
        {
            return; // User not logged in
        }

        var user = await _context.Users.FindAsync(int.Parse(userId));
        var username = user.Username;

        var message = new Message
        {
            UserId = int.Parse(userId),
            Content = content,
            CreatedAt = DateTime.Now
        };

        // Save message to the database
        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        await Clients.Group(groupName).SendAsync("ReceiveMessage", username, content);
    }

    public async Task AddToGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).SendAsync("ShowWho", $"{Context.ConnectionId} has joined the group {groupName}.");
    }

    public async Task RemoveFromGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).SendAsync("ShowWho", $"{Context.ConnectionId} has left the group {groupName}.");
    }

    public async Task CreateGroup(string groupName)
    {
        var group = new Group { Name = groupName };
        _context.Groups.Add(group);
        await _context.SaveChangesAsync();

        await Clients.Caller.SendAsync("GroupCreated", groupName);
    }
}
