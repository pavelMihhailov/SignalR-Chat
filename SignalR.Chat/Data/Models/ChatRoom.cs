using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace SignalR.Chat.Data.Models
{
    public class ChatRoom
    {
        public ChatRoom()
        {
            Users = new List<IdentityUser>();
            Messages = new List<Message>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual IList<Message> Messages { get; set; }

        public virtual IList<IdentityUser> Users { get; set; }
    }
}
