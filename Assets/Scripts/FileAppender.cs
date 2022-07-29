using System.IO;
using System.Text;

namespace LoggerAsset
{
    public class FileAppender
    {
        private readonly object _lockObject = new object();
        
        public string FileName { get; }

        public FileAppender(string fileName)
        {
            FileName = fileName;
        }

        public bool Append(string content)
        {
            try
            {
                lock (_lockObject)
                {
                    using (FileStream fileStream = File.Open(FileName, FileMode.Append, FileAccess.Write, FileShare.Read))
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(content);
                        fileStream.Write(bytes, 0, bytes.Length);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}