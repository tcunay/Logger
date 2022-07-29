using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LoggerAsset.SampleSceneScripts
{
    public class MainThreadThrower : MonoBehaviour
    {
        private const float TimeBetweenMessages = 0.5f;
        
        private float _timer;

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer > TimeBetweenMessages)
            {
                _timer = 0;
                LogMain();
            }
        }

        private void LogMain()
        {
            int randomValue = Random.Range(0, 4);

            switch (randomValue)
            {
                case 0:
                    Debug.Log("Log from MAIN THREAD");
                    break;
                case 1:
                    Debug.LogWarning("Warning from MAIN THREAD");
                    break;
                case 2:
                    Debug.LogError("Error from MAIN THREAD");
                    break;
                case 3:
                    throw new NullReferenceException();
                    break;
            }
        }
    }
}