﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabletop.Core.Chat
{
    public class ChatMessage
    {
        public DateTimeOffset Date { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        public bool System { get; set; }
    }
}
