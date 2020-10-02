using ReversiCore;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.View
{
    /*
     * Timer that delays robot move
     */
    public class DelayRobotMoveQueue
    {
        public delegate void DelayableDelegate();

        public DelayableDelegate delayedDelegates;

        public DelayRobotMoveQueue()
        {
            delayedDelegates = null;
        }


        /*
         * Method clears delegates queue
         */
        public void Clear()
        {
            delayedDelegates = null;
        }


        /*
         * Method adds new delegate do delay
         * ------------------------------
         * newDelayedDelegate - delegate that has to be delayed
         */
        public void AddDelegate(DelayableDelegate newDelayedDelegate)
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
