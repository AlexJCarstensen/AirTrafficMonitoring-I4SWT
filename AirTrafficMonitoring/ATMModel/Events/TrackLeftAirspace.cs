using System;
using System.Collections.Generic;
using System.Linq;
using ATMModel.Data;

namespace ATMModel.Events
{
    public class TrackLeftAirspace : ATMNotification
    {
        private readonly IATMLogEvent _atmLog;

        public TrackLeftAirspace(IATMLogEvent atmLog = null)
        {
            _atmLog = atmLog ?? new ATMLogger();
        }

        public override void DetectNotification(List<IATMTransponderData> oldTransponderDatas, List<IATMTransponderData> newTransponderDatas)
        {
            foreach (var item in oldTransponderDatas.Where(item => !newTransponderDatas.Exists(t => t.Tag == item.Tag)))
            {
                Notify(new NotificationEventArgs(item.Tag, "TrackLeftAirspace", item.Timestamp));
                _atmLog.Log(item.Timestamp + " TrackLeftAirspace Notification " + item.Tag);
            }
        }
    }
}