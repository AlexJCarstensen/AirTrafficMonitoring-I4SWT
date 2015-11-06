using System.Collections.Generic;
using System.Linq;
using ATMModel.Data;

namespace ATMModel.Events
{
    public class TrackEnteredAirspace : ATMNotification
    {
        private readonly IATMLogEvent _atmLog;
        private const string LogString = " TrackEnteredAirspace Notification ";

        public TrackEnteredAirspace(IATMLogEvent atmLogEvent = null)
        {
            _atmLog = atmLogEvent ?? new ATMLogger();
        }

        public override void DetectNotification(ICollection<IATMTransponderData> oldTransponderDatas, ICollection<IATMTransponderData> newTransponderDatas)
        {
            foreach (var item in newTransponderDatas.Where(item => oldTransponderDatas.All(t => t.Tag != item.Tag)))
            {
                Notify(new NotificationEventArgs(item.Tag, "TrackEnteredAirspace", item.Timestamp));
                _atmLog.Log(item.Timestamp + LogString + item.Tag);
            }
        }
    }
}