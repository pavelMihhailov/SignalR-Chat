using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR.Chat.Data;
using SignalR.Chat.Data.Models;
using SignalR.Chat.Hubs;
using SignalR.Chat.Models.Chat;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Chat.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext db;

        public ChatController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Room(int id)
        {
            var room = this.db.ChatRooms.FirstOrDefault(x => x.Id == id);

            if (room == null)
            {
                return this.NotFound();
            }

            var viewModel = new RoomViewModel()
            {
                Id = room.Id,
                Name = room.Name,
                Messages = this.db.Messages.Where(x => x.ChatRoomId == room.Id).ToList(),
            };

            return View(viewModel);
        }

        public IActionResult Rooms()
        {
            var rooms = db.ChatRooms.ToList();

            return View(rooms);
        }

        public IActionResult CreateRoom()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom(string roomName)
        {
            if (!string.IsNullOrWhiteSpace(roomName))
            {
                var user = db.Users.FirstOrDefault(x => x.UserName == this.User.Identity.Name);

                db.ChatRooms.Add(new ChatRoom()
                {
                    Name = roomName,
                    Users = new List<IdentityUser>() { user },
                });
                await db.SaveChangesAsync();
            }

            return this.RedirectToAction(nameof(Rooms));
        }
    }
}