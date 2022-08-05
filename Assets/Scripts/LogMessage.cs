using System;
using UnityEngine;

namespace LoggerAsset
{
    public class LogMessage
    {
        public LogType Type { get; }
        public DateTime Time { get; }
        public string Message { get; }

        public LogMessage(LogType type, string message)
        {
            Type = type;
            Message = message;
            Time = DateTime.UtcNow;
        }

        public LogMessage(LogType type, string message, DateTime time)
        {
            Type = type;
            Message = message;
            Time = time;
        }
    }
}