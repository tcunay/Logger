using System;
using System.IO;
using System.Text;

namespace LoggerAsset
{
    public class FileWriter
    {
        private const string DateFormat = "yyyy-MM-dd";
        
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

        public void Write(string message)
        {
            using (FileStream fileStream = File.Open(_filePath, FileMode.Append, FileAccess.Write, FileShare.Read))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(message);
                fileStream.Write(bytes, 0, bytes.Length);
            }
        }
        
    }
}