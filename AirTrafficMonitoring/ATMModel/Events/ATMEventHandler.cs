using System;
using System.Collections.Generic;
using System.Linq;
using ATMModel.Data;

namespace ATMModel.Events
{
    public class ATMEventHandler : IATMEventHandler
    {
        private readonly List<ATMWarning> _atmWarnings = new List<ATMWarning> {new Separation()};
        private readonly List<ATMNotification> _atmNotifications = new List<ATMNotification> {new TrackEnteredAirspace(), new TrackLeftAirspace()};
        private List<IATMTransponderData> _atmTransponderDatas = new List<IATMTransponderData>();
        public void Handle(List<IATMTransponderData> newAtmTransponderDatas)
        {
            foreach (var warning in _atmWarnings)
            {
                warning.DetectWarning(newAtmTransponderDatas);
            }

            foreach (var notification in _atmNotifications)
            {
                notification.DetectNotification(_atmTransponderDatas, newAtmTransponderDatas);
            }

            _atmTransponderDatas = newAtmTransponderDatas;
        }
    }
}