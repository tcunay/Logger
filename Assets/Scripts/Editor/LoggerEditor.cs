using UnityEditor;
using UnityEngine;

namespace LoggerAsset.Editor
{
    [CustomEditor(typeof(Logger))]
    public class LoggerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Logger logger = (Logger) target;
            
            if(GUILayout.Button(nameof(logger.OpenFilesDirectory)))
                logger.OpenFilesDirectory();
        }
    }
}