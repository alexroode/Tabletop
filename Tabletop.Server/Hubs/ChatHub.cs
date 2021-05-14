using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tabletop.Core;
using Tabletop.Core.Chat;
using Tabletop.Core.Users;

#nullable enable
namespace Tabletop.Server.Hubs
{
    public interface IChatClient
    {
        Task ReceiveMessage(ChatMessage message);
        Task UserJoined(User user);
        Task UserLeft(User user);
        Task UpdateMessages(IList<ChatMessage> messages);
        Task UpdateUsers(IList<User> user);
    }

    [Authorize]
    public class ChatHub : Hub<IChatClient>
    {
        private readonly ChatService _chatService;
        private readonly UserService _userService;

        public ChatHub(ChatService chatService, UserService userService)
        {
            _chatService = chatService;
            _userService = userService;
        }

        public async Task SendMessage(string messageText)
        {
            if (messageText.Length > Constants.MaxMessageLength)
            {
                return;
            }

            var message = new ChatMessage() { Date = DateTime.UtcNow, Author = Context.User!.Identity!.Name, Text = messageText };
            _chatService.AddMessage(message);
            await Clients.All.ReceiveMessage(message);
        }

        public async Task JoinChat()
        {
            var connectionId = Context.ConnectionId;
            var user = _userService.GetOrAddUser(Context.User!);
            _chatService.AddActiveUser(connectionId, user);
            await Clients.All.UserJoined(user);
        }

        public override async Task OnConnectedAsync()
        {
            var messages = _chatService.GetLatestMessages();
            await Clients.Caller.UpdateMessages(messages);

            var users = _chatService.GetActiveUsers();
            await Clients.Caller.UpdateUsers(users);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var user = _chatService.RemoveActiveUser(Context.ConnectionId);
            if (user != null)
            {
                await Clients.Others.UserLeft(user);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
