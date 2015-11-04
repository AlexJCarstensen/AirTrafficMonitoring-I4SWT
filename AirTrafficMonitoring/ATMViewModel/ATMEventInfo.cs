namespace ATMViewModel
{
    public class ATMEventInfo
    {
        public ATMEventInfo(string tag, string eventName, string timestamp, string eventCategory)
        {
            Tag = tag;
            EventName = eventName;
            Timestamp = timestamp;
            EventCategory = eventCategory;
        }

        public string Tag { get; }
        public string EventName { get; }
        public string Timestamp { get; }
        public string EventCategory { get; }
    }
}