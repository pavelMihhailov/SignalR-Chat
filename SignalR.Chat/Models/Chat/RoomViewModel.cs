using SignalR.Chat.Data.Models;
using System;
using System.Collections.Generic;

namespace SignalR.Chat.Models.Chat
{
    public class RoomViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Message> Messages { get; set; }
    }
}
