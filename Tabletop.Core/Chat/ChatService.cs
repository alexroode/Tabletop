using System;
using System.Collections.Generic;
using System.Linq;
using Tabletop.Core.Users;

#nullable enable
namespace Tabletop.Core.Chat
{
    public class ChatService
    {
        private const int _latestMessageCount = 50;

        private readonly List<ChatMessage> _messages = new();
        private readonly Dictionary<string, User> _activeUsers = new();

        public List<ChatMessage> GetLatestMessages()
        {
            return _messages.TakeLast(_latestMessageCount).ToList();
        }

        public void AddMessage(ChatMessage message)
        {
            _messages.Add(message);
        }

        public List<User> GetActiveUsers()
        {
            return _activeUsers.Values.GroupBy(p => p.Id).Select(g => g.First()).ToList();
        }

        public void AddActiveUser(string connectionId, User user)
        {
            _activeUsers[connectionId] = user;
        }

        public User? RemoveActiveUser(string connectionId)
        {
            if (_activeUsers.ContainsKey(connectionId))
            {
                var user = _activeUsers[connectionId];
                _activeUsers.Remove(connectionId);

                // If there are no other connections containing the same user, the user has really left
                if (!_activeUsers.Any(u => u.Value.Id == user.Id))
                {
                    return user;
                }
            }
            return null;
        }
    }
}
