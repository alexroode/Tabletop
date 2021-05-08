using System;
using System.Collections.Generic;
using System.Linq;

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
            return _activeUsers.Values.ToList();
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
                return user;
            }
            return null;
        }
    }
}
