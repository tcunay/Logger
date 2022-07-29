using System;
using System.IO;
using System.Text;

namespace LoggerAsset
{
    public class FileWriter
    {
        private const string DateFormat = "yyyy-MM-dd";
        private const string LogTimeFormat = "{0:dd/MM/yyyy HH:mm:ss:ffff} [{1}]: {2}\r";
        
        private string _folder;
        private string _filePath;

        public FileWriter(string folder)
        {
            _folder = folder;
            ManagePath();
        }

        private void ManagePath()
        {
            _filePath = $"{_folder}/{DateTime.UtcNow.ToString(DateFormat)}.log";
        }

        public void Write(LogMessage message)
        {
            string messageToWrite = string.Format(LogTimeFormat, message.Time, message.Type, message.Message);
            
            using (FileStream fileStream = File.Open(_filePath, FileMode.Append, FileAccess.Write, FileShare.Read))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(messageToWrite);
                fileStream.Write(bytes, 0, bytes.Length);
            }
        }
    }
}