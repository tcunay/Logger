using System;
using System.Threading;
using UnityEngine;
using Random = System.Random;

namespace LoggerAsset.SampleSceneScripts
{
    public class AnotherThreadThrower : ThreadThrowerMessages
    {
        private Thread _logThread;
        private bool _isThreadAlive;
        
        protected override string ThreadMessage => "ANOTHER";
        protected override Exception Exception => new DivideByZeroException();

        private void Start()
        {
            _isThreadAlive = true;
            _logThread = new Thread(SendLogs)
            {
                IsBackground = true
            };
            _logThread.Start();
        }

        private void OnDestroy()
        {
            _isThreadAlive = false;
        }

        private void SendLogs()
        {
            while (_isThreadAlive)
            {
                SendLog();
                Thread.Sleep(150);
            }
        }
    }
}