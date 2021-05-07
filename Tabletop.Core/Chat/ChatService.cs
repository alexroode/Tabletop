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
        private readonly Dictionary<string, string> _activeUsers = new();

        public List<ChatMessage> GetLatestMessages()
        {
            return _messages.TakeLast(_latestMessageCount).ToList();
        }

        public void AddMessage(ChatMessage message)
        {
            _messages.Add(message);
        }
        public List<string> GetActiveUsers()
        {
            return _activeUsers.Values.ToList();
        }

        public void AddActiveUser(string connectionId, string username)
        {
            _activeUsers[connectionId] = username;
        }

        public string? RemoveActiveUser(string connectionId)
        {
            if (_activeUsers.ContainsKey(connectionId))
            {
                var username = _activeUsers[connectionId];
                _activeUsers.Remove(connectionId);
                return username;
            }
            return null;
        }
    }
}
