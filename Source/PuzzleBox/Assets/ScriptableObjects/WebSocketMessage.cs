﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.ScriptableObjects
{
    public enum MessageType
    {
        KEEP_ALIVE = 0,
        KEYPRESS = 1
    }

    public class WebSocketMessage
    {
        public MessageType Type { get; set; }

        public string Data { get; set; }
    }
}
