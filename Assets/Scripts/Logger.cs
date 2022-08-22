using System.IO;
using UnityEngine;

namespace LoggerAsset
{
    public class Logger : MonoBehaviour
    {
        private FileWriter _fileWriter;
        private string _workDirectory;

        private void Awake() => Init();

        private void OnDestroy() => Dispose();

        private void Update()
        {
#if UNITY_EDITOR
            if(Input.GetKeyUp(KeyCode.L))
                OpenFilesDirectory();
#endif
        }

        public void OpenFilesDirectory() => UnityEditor.EditorUtility.RevealInFinder(_workDirectory);

        private void Init()
        {
            _workDirectory = $"{Application.persistentDataPath}/Logs";

            if (Directory.Exists(_workDirectory) == false)
                Directory.CreateDirectory(_workDirectory);

            _fileWriter = new FileWriter(_workDirectory);
            Application.logMessageReceivedThreaded += OnLogMessageReceived;
        }

        private void Dispose()
        {
            _fileWriter.Dispose();
            Application.logMessageReceivedThreaded -= OnLogMessageReceived;
        }

        private void OnLogMessageReceived(string condition, string stacktrace, LogType type)
        {
            _fileWriter.Write(new LogMessage(type, condition));
            
            if (type == LogType.Exception)
                _fileWriter.Write(new LogMessage(type, stacktrace));
        }
    }
}