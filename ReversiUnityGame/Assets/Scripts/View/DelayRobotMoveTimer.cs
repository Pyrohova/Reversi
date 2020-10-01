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

        public delegate void DelayableDelegate();

        public DelayableDelegate delayedDelegates;

        public bool IsRunning { get; private set; } //if timer is currently running
        public bool HasReachedMaxTime { get; private set; } //if timer has reached its maximum time

        public DelayRobotMoveTimer()
        {
            maxTime = 0;
            elapsedTime = 0;
            HasReachedMaxTime = false;
            IsRunning = false;
            delayedDelegates = null;
        }

        public void Restart(float seconds)
        {
            maxTime = seconds;
            elapsedTime = 0;
            HasReachedMaxTime = false;
            IsRunning = true;
            delayedDelegates = null;
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


        /*
         * Method adds new delegate do delay
         * ------------------------------
         * newDelayedDelegate - delegate that has to be delayed
         */
        public void Delay(DelayableDelegate newDelayedDelegate)
        {
            delayedDelegates += newDelayedDelegate;
        }


        /*
         * Method calls all delayed delegates
         */
        public void CallDelayedDelegates()
        {
            delayedDelegates?.Invoke();
        }
    }
}
