using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tabletop.Core;
using Tabletop.Core.Chat;

#nullable enable
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
            if (messageText.Length > Constants.MaxMessageLength)
            {
                return;
            }

            var message = new ChatMessage() { Date = DateTime.UtcNow, Author = user, Text = messageText };
            _chatService.AddMessage(message);
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task JoinChat(string username)
        {
            var connectionId = Context.ConnectionId;
            _chatService.AddActiveUser(connectionId, username);
            await Clients.All.SendAsync("UserJoined", username);
        }

        public override async Task OnConnectedAsync()
        {
            var messages = _chatService.GetLatestMessages();
            await Clients.Caller.SendAsync("ReceiveMessages", messages);

            var users = _chatService.GetActiveUsers();
            await Clients.Caller.SendAsync("CurrentUsers", users);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var username = _chatService.RemoveActiveUser(Context.ConnectionId);
            if (username != null)
            {
                await Clients.Others.SendAsync("UserLeft", username);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
