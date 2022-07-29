using System;
using UnityEngine;
using Random = System.Random;

namespace LoggerAsset.SampleSceneScripts
{
    public class MainThreadThrower : ThreadThrowerMessages
    {
        private const float TimeBetweenMessages = 0.5f;
        
        private float _timer;

        protected override string ThreadMessage => "MAIN";
        protected override Exception Exception => new NullReferenceException();

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer > TimeBetweenMessages)
            {
                _timer = 0;
                SendLog();
            }
        }
    }
}