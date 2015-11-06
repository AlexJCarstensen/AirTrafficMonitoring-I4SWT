using System.Collections.Generic;
using System.Linq;
using ATMModel.Data;

namespace ATMModel.Events
{
    public class TrackLeftAirspace : ATMNotification
    {
        private readonly IATMLogEvent _atmLog;
        private const string LogString = " TrackLeftAirspace Notification ";

        public TrackLeftAirspace(IATMLogEvent atmLogEvent = null)
        {
            _atmLog = atmLogEvent ?? new ATMLogger();
        }

        public override void DetectNotification(ICollection<IATMTransponderData> oldTransponderDatas, ICollection<IATMTransponderData> newTransponderDatas)
        {
            foreach (var item in oldTransponderDatas.Where(item => newTransponderDatas.All(t => t.Tag != item.Tag)))
            {
                Notify(new NotificationEventArgs(item.Tag, "TrackLeftAirspace", item.Timestamp));
                _atmLog.Log(item.Timestamp + LogString + item.Tag);
            }
        }
    }
}