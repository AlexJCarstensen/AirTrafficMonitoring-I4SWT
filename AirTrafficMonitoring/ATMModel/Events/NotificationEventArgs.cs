using System;
using System.Timers;

namespace ATMModel.Events
{
    public class NotificationEventArgs : EventArgs
    {
        public event EventHandler StopMeEvent;
        private int _delayTime;
        public NotificationEventArgs(string tag, string eventName, string timestamp = "000000000000000", int delayTime = 10000)
        {
            Tag = tag;
            EventName = eventName;
            Timestamp = timestamp;
            _delayTime = delayTime;

            var timer = new Timer {Interval = delayTime};
            timer.Elapsed += (sender, args) =>
            {
                StopMeEvent?.Invoke(this, Empty);
                timer.Stop();
            };
            timer.Start();
        }

        public string Tag { get; }
        public string EventName { get; }
        public string Timestamp { get; }
        
    }
}