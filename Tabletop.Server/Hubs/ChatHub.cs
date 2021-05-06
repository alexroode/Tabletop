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

        public async Task SendMessage(string user, string message)
        {
            _chatService.AddMessage(new ChatMessage() { Author = user, Text = message });
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public override async Task OnConnectedAsync()
        {
            var messages = _chatService.GetLatestMessages();
            await Clients.Caller.SendAsync("ReceiveMessages", messages);
            await base.OnConnectedAsync();
        }
    }
}
