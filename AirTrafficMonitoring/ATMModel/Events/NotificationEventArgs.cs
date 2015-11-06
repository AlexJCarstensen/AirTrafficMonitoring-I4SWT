using System;
using System.Timers;

namespace ATMModel.Events
{
    /// <summary>
    /// This class implements arguments to our notification event
    /// StopMeEvent being raised 10 seconds after creation of this object
    /// </summary>
    public class NotificationEventArgs : EventArgs
    {
        public event EventHandler StopMeEvent;

        public NotificationEventArgs(string tag, string eventName, string timestamp = "000000000000000", int delayTime = 10000)
        {
            Tag = tag;
            EventName = eventName;
            Timestamp = timestamp;

            // our timer
            var timer = new Timer {Interval = delayTime};
            // function that cálled after time elapsed
                timer.Elapsed += (sender, args) =>
                {
                    StopMeEvent?.Invoke(this, Empty);
                    timer.Stop();
                    timer.Dispose();
                };
                timer.Start();
            
        }

        public string Tag { get; }
        public string EventName { get; }
        public string Timestamp { get; }
        
    }
}