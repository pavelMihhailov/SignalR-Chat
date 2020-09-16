using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalR.Chat.Data;
using SignalR.Chat.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Chat.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext db;

        public ChatHub(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task SendMessage(string roomId, string message)
        {
            var room = db.ChatRooms.FirstOrDefault(x => x.Id == int.Parse(roomId));

            if (room == null)
            {
                return;
            }

            var user = db.Users.FirstOrDefault(x => x.UserName == this.Context.User.Identity.Name);

            db.Messages.Add(new Message
            {
                ChatRoomId = room.Id,
                SentDate = DateTime.UtcNow,
                UserId = user.Id,
                Content = message,
            });

            await db.SaveChangesAsync();

            await Clients.Group(room.Name).SendAsync("ReceiveMessage", user.UserName, message);
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Update", $"{Context.User.Identity.Name} has joined the group {groupName}.");
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Update", $"{Context.User.Identity.Name} has left the group {groupName}.");
        }
    }
}
