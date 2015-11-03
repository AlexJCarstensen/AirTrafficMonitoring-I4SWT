using System;

namespace ATMModel.Events
{
    public class WarningEventArgs : EventArgs
    {
        public WarningEventArgs(string tag1, string tag2, string eventName, bool active = true)
        {
            Tag1 = tag1;
            Tag2 = tag2;
            EventName = eventName;
            Active = active;
        }

        public string Tag1 { get; }
        public string Tag2 { get; }
        public string EventName { get; }
        public bool Active { get; }
    }
}