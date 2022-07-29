using System;
using System.Collections.Concurrent;
using System.Threading;

namespace LoggerAsset
{
    public class FileWriter
    {
        private const string DateFormat = "yyyy-MM-dd";
        private const string LogTimeFormat = "{0:dd/MM/yyyy HH:mm:ss:ffff} [{1}]: {2}\r";
        private const int ThreadSleepTime = 5;
        
        private readonly string _folder;
        private readonly Thread _workingThread;

        private FileAppender _appender;
        private readonly ConcurrentQueue<LogMessage> _messages = new();
        private bool _isDisposing;
        private string _filePath;

        public FileWriter(string folder)
        {
            _folder = folder;
            ManagePath();
            _workingThread = new Thread(StoreMessages)
            {
                IsBackground = true,
                Priority = ThreadPriority.Normal
            };
            _workingThread.Start();
        }

        private void ManagePath()
        {
            _filePath = $"{_folder}/{DateTime.UtcNow.ToString(DateFormat)}.log";
        }

        public void Write(LogMessage message)
        {
            _messages.Enqueue(message);
        }

        private void StoreMessages()
        {
            while (_isDisposing == false)
            {
                while (_messages.IsEmpty == false)
                {
                    try
                    {
                        if (_messages.TryPeek(out LogMessage message) == false)
                            Thread.Sleep(ThreadSleepTime);

                        if (_appender == null || _appender.FileName != _filePath)
                            _appender = new FileAppender(_filePath);

                        string messageToWrite =
                            string.Format(LogTimeFormat, message.Time, message.Type, message.Message);

                        if (_appender.Append(messageToWrite))
                        {
                            _messages.TryDequeue(out message);
                        }
                        else
                        {
                            Thread.Sleep(ThreadSleepTime);
                        }
                    }
                    catch
                    {
                        break;
                    }
                }
            }
        }
    }
}