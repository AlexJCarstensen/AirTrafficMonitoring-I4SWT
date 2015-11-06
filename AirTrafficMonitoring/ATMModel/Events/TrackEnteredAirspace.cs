using System.Collections.Generic;
using System.Linq;
using ATMModel.Data;

namespace ATMModel.Events
{
    public class TrackEnteredAirspace : ATMNotification
    {
        private readonly IATMLogEvent _atmLog;

        public TrackEnteredAirspace(IATMLogEvent aTMLogEvent = null)
        {
            _atmLog = aTMLogEvent ?? new ATMLogger();
        }

        public override void DetectNotification(ICollection<IATMTransponderData> oldTransponderDatas, ICollection<IATMTransponderData> newTransponderDatas)
        {
            foreach (var item in newTransponderDatas.Where(item => oldTransponderDatas.All(t => t.Tag != item.Tag)))
            {
                const string logString = " TrackEnteredAirspace Notification ";
                Notify(new NotificationEventArgs(item.Tag, "TrackEnteredAirspace", item.Timestamp));
                _atmLog.Log(item.Timestamp + logString + item.Tag);
            }
        }
    }
}