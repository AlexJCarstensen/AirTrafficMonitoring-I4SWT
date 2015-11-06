using System.Collections.Generic;
using ATMModel.Data;

namespace ATMModel.Events
{
    public class ATMEventHandler : IATMEventHandler
    {
        private readonly List<ATMWarning> _atmWarnings;
        private readonly List<ATMNotification> _atmNotifications;
        private List<IATMTransponderData> _atmTransponderDatas = new List<IATMTransponderData>();

        public ATMEventHandler(List<ATMWarning> atmWarnings = null, List<ATMNotification> atmNotifications = null)
        {
            _atmWarnings = atmWarnings ?? new List<ATMWarning> {new Separation()};
            _atmNotifications = atmNotifications ?? new List<ATMNotification> { new TrackEnteredAirspace(), new TrackLeftAirspace() };
        }

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