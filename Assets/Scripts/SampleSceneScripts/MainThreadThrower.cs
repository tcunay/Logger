using System;
using System.Text;
using UnityEngine;

namespace LoggerAsset.SampleSceneScripts
{
    public class MainThreadThrower : ThreadThrowerMessages
    {
        private const float TimeBetweenMessages = 0.5f;
        
        private float _timer;

        protected override string ThreadMessage => "MAIN";
        protected override Exception Exception => new NullReferenceException(ThreadMessage);

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