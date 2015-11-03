using System;

namespace ATMModel.Events
{
    public class NotificationEventArgs : EventArgs
    {
        public NotificationEventArgs(string tag, string eventName)
        {
            Tag = tag;
            EventName = eventName;
        }

        public string Tag { get; }
        public string EventName { get; }
        
    }
}