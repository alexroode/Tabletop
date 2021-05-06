using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Tabletop.Core.Chat
{
    public class ChatService
    {
        private const int _latestMessageCount = 50;

        private readonly List<ChatMessage> _messages = new();

        public List<ChatMessage> GetLatestMessages()
        {
            return _messages.TakeLast(_latestMessageCount).ToList();
        }

        public void AddMessage(ChatMessage message)
        {
            _messages.Add(message);
        }
    }
}
