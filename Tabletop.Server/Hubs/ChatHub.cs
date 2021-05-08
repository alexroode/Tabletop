using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ChatService _chatService;

        public ChatHub(ChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task SendMessage(string messageText)
        {
            if (messageText.Length > Constants.MaxMessageLength)
            {
                return;
            }

            var message = new ChatMessage() { Date = DateTime.UtcNow, Author = Context.User!.Identity!.Name, Text = messageText };
            _chatService.AddMessage(message);
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task JoinChat()
        {
            var connectionId = Context.ConnectionId;
            var user = new User() { DisplayName = Context.User!.Identity!.Name, Id = Context.UserIdentifier };
            _chatService.AddActiveUser(connectionId, user);
            await Clients.All.SendAsync("UserJoined", user);
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
            var user = _chatService.RemoveActiveUser(Context.ConnectionId);
            if (user != null)
            {
                await Clients.Others.SendAsync("UserLeft", user);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
