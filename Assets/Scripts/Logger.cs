using System.IO;
using UnityEngine;

namespace LoggerAsset
{
    public class Logger : MonoBehaviour
    {
        private FileWriter _fileWriter;
        private string _workDirectory;

        private void Awake()
        {
            _workDirectory = $"{Application.persistentDataPath}/Logs";
            
            if (Directory.Exists(_workDirectory) == false)
                Directory.CreateDirectory(_workDirectory);

            _fileWriter = new FileWriter(_workDirectory);
            Application.logMessageReceivedThreaded += OnLogMessageReceived;
        }

        private void OnLogMessageReceived(string condition, string stacktrace, LogType type)
        {
            _fileWriter.Write(condition);
        }

        private void Update()
        {
#if UNITY_EDITOR
            if(Input.GetKeyUp(KeyCode.L))
                UnityEditor.EditorUtility.RevealInFinder(_workDirectory);
#endif
        }
    }
}