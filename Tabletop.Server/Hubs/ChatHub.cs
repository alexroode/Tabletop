using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tabletop.Core.Chat;

namespace Tabletop.Server.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatService _chatService;

        public ChatHub(ChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task SendMessage(string user, string messageText)
        {
            var message = new ChatMessage() { Date = DateTime.UtcNow, Author = user, Text = messageText };
            _chatService.AddMessage(message);
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public override async Task OnConnectedAsync()
        {
            var messages = _chatService.GetLatestMessages();
            await Clients.Caller.SendAsync("ReceiveMessages", messages);
            await base.OnConnectedAsync();
        }
    }
}
