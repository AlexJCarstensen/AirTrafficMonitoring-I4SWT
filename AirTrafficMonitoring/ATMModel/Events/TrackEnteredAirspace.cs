using System;
using System.Collections.Generic;
using System.Linq;
using ATMModel.Data;

namespace ATMModel.Events
{
    public class TrackEnteredAirspace : ATMNotification
    {
        private readonly IATMLogEvent _atmLog;

        public TrackEnteredAirspace(IATMLogEvent atmLog = null)
        {
            _atmLog = atmLog ?? new ATMLogger();
        }

        public override void DetectNotification(List<IATMTransponderData> oldTransponderDatas, List<IATMTransponderData> newTransponderDatas)
        {
            foreach (var item in newTransponderDatas.Where(item => !oldTransponderDatas.Exists(t => t.Tag == item.Tag)))
            {
                Notify(new NotificationEventArgs(item.Tag, "TrackEnteredAirspace"));
                _atmLog.Log(item.Timestamp + " TrackEnteredAirspace Notification " + item.Tag);
            }
        }
    }
}