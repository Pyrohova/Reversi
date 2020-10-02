using ReversiCore;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.View
{
    /*
     * Timer that delays robot move
     */
    public class DelayRobotMoveTimer
    {
        private float maxTime;
        private float elapsedTime;

        public bool IsRunning { get; private set; } //if timer is currently running
        public bool HasReachedMaxTime { get; private set; } //if timer has reached its maximum time

        public DelayRobotMoveTimer()
        {
            maxTime = 0;
            elapsedTime = 0;
            HasReachedMaxTime = false;
            IsRunning = false;
        }

        public void Restart(float seconds)
        {
            maxTime = seconds;
            elapsedTime = 0;
            HasReachedMaxTime = false;
            IsRunning = true;
        }

        public void Increase(float deltaTime)
        {
            elapsedTime += deltaTime;

            if (elapsedTime >= maxTime)
            {
                HasReachedMaxTime = true;
            }
        }

        public void Stop()
        {
            IsRunning = false;
        }
    }
}
