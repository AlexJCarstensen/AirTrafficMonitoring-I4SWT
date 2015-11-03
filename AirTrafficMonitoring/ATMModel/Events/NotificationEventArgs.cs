using System;
using System.Timers;

namespace ATMModel.Events
{
    public class NotificationEventArgs : EventArgs
    {
        public event EventHandler StopMeEvent;
        public NotificationEventArgs(string tag, string eventName)
        {
            Tag = tag;
            EventName = eventName;

            var timer = new Timer {Interval = 10000};
            timer.Elapsed += (sender, args) =>
            {
                StopMeEvent?.Invoke(this, Empty);
                timer.Stop();
            };
            timer.Start();
        }

        public string Tag { get; }
        public string EventName { get; }
        
    }
}