using System;
using System.Collections.Concurrent;
using System.Threading;

namespace LoggerAsset
{
    public class FileWriter : IDisposable
    {
        private const string DateFormat = "yyyy-MM-dd";
        private const string LogTimeFormat = "{0:dd/MM/yyyy HH:mm:ss:ffff} [{1}]: {2}\r";
        private const int ThreadSleepTime = 5;
        private const int CheckDateThreadSlipTime = 1000;
        private const int ManualResetEventWaitValue = 500;
        private const int MaxMessageLenght = 3500;

        private readonly ConcurrentQueue<LogMessage> _messages = new();
        private readonly Thread _workingThread;
        private readonly string _folder;
        private readonly ManualResetEvent _manualResetEvent = new ManualResetEvent(true);

        private FileAppender _appender;
        private Thread _checkNewDateThread;
        private DateTime _previousDate;
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
            _checkNewDateThread = new Thread(CheckNewDay)
            {
                IsBackground = true,
                Priority = ThreadPriority.BelowNormal
            };
            
            _workingThread.Start();
            _checkNewDateThread.Start();
        }

        private void CheckNewDay()
        {
            while (_isDisposing == false)
            {
                DateTime currentDate = DateTime.UtcNow;
                if (currentDate.Day != _previousDate.Day)
                {
                    _previousDate = currentDate;
                    ManagePath();
                }
                
                Thread.Sleep(CheckDateThreadSlipTime);
            }
        }

        private void ManagePath()
        {
            _previousDate = DateTime.UtcNow;
            _filePath = $"{_folder}/{DateTime.UtcNow.ToString(DateFormat)}.log";
        }

        public void Write(LogMessage message)
        {
            try
            {
                if (message.Message.Length > MaxMessageLenght)
                {
                    var preview = message.Message.Substring(0, MaxMessageLenght);
                    message = new LogMessage(message.Type,
                        $"Message us too long {message.Message.Length}. Preview: {preview}", message.Time);
                }
                
                _messages.Enqueue(message);
                _manualResetEvent.Set();
            }
            catch
            {
                // ignored
            }
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

                _manualResetEvent.Reset();
                _manualResetEvent.WaitOne(ManualResetEventWaitValue);
            }
        }

        public void Dispose()
        {
            _isDisposing = true;
            _workingThread?.Abort();
            _checkNewDateThread?.Abort();
            _manualResetEvent?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}