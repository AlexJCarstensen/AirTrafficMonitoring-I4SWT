using System;

namespace ATMModel.Events
{
    public class WarningEventArgs : EventArgs
    {
        public event EventHandler StopMeEvent;
        public WarningEventArgs(string tag1, string tag2, string eventName, bool active = true)
        {
            Tag1 = tag1;
            Tag2 = tag2;
            EventName = eventName;
            _active = active;
        }

        public string Tag1 { get; }
        public string Tag2 { get; }
        public string EventName { get; }
        private bool _active;
        public bool Active { get { return _active; } set { _active = value; if(!value) StopMeEvent?.Invoke(this, Empty);} }
    }
}