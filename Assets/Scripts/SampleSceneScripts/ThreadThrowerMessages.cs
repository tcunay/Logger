using System;
using System.Text;
using UnityEngine;
using Random = System.Random;

namespace LoggerAsset.SampleSceneScripts
{
    public abstract class ThreadThrowerMessages : MonoBehaviour
    {
        private Random _random;
        
        protected abstract string ThreadMessage { get; }
        protected abstract Exception Exception { get; }
        
        private  string Log => $"Log from {ThreadMessage} THREAD";
        private string Warning => $"Warning from {ThreadMessage} THREAD";
        private string Error => $"Error from {ThreadMessage} THREAD";

        private void Awake()
        {
            _random = new Random();
        }

        protected void SendLog()
        {
            int randomValue = _random.Next(0, 5);

            switch (randomValue)
            {
                case 0:
                    Debug.Log(Log);
                    break;
                case 1:
                    Debug.LogWarning(Warning);
                    break;
                case 2:
                    Debug.LogError(Error);
                    break;
                case 3:
                    throw Exception;
                case 4:
                    Debug.Log(GetLongMessage(4000));
                    break;
            }
        }

        private string GetLongMessage(int count)
        {
            var str = new StringBuilder();

            for (int i = 0; i < count; i++)
            {
                str.Append("A");
            }

            return str.ToString();
        }
    }
}