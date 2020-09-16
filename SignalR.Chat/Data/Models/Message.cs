using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace SignalR.Chat.Data.Models
{
    public class Message
    {
        public int Id { get; set; }

        [MinLength(1)]
        public string Content { get; set; }

        public DateTime SentDate { get; set; }

        public string UserId { get; set; }

        public virtual IdentityUser User { get; set; }

        public int ChatRoomId { get; set; }

        public virtual ChatRoom ChatRoom { get; set; }
    }
}
