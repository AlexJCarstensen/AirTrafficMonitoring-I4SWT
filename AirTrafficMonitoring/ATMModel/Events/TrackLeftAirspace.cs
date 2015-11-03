using System;
using System.Collections.Generic;
using System.Linq;
using ATMModel.Data;

namespace ATMModel.Events
{
    public class TrackLeftAirspace : ATMNotification
    {
        public override void DetectNotification(List<IATMTransponderData> oldTransponderDatas, List<IATMTransponderData> newTransponderDatas)
        {
            foreach (var item in oldTransponderDatas.Where(item => !newTransponderDatas.Exists(t => t.Tag == item.Tag)))
            {
                Notify(new NotificationEventArgs(item.Tag, "TrackLeftAirspace"));
            }
        }
    }
}