using System.Collections.Generic;
using System.Linq;
using ATMModel.Data;

namespace ATMModel.Events
{
    public class TrackLeftAirspace : ATMNotification
    {
        private readonly IATMLogEvent _atmLog;

        public TrackLeftAirspace(IATMLogEvent aTMLogEvent = null)
        {
            _atmLog = aTMLogEvent ?? new ATMLogger();
        }

        public override void DetectNotification(ICollection<IATMTransponderData> oldTransponderDatas, ICollection<IATMTransponderData> newTransponderDatas)
        {
            foreach (var item in oldTransponderDatas.Where(item => newTransponderDatas.All(t => t.Tag != item.Tag)))
            {
                const string logString = " TrackLeftAirspace Notification ";
                Notify(new NotificationEventArgs(item.Tag, "TrackLeftAirspace", item.Timestamp));
                _atmLog.Log(item.Timestamp + logString + item.Tag);
            }
        }
    }
}