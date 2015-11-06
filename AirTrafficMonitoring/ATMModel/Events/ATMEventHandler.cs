using System.Collections.Generic;
using System.Linq;
using ATMModel.Data;

namespace ATMModel.Events
{
    public class ATMEventHandler : IATMEventHandler
    {
        private readonly IEnumerable<ATMWarning> _atmWarnings;
        private readonly IEnumerable<ATMNotification> _atmNotifications;
        private ICollection<IATMTransponderData> _aTMTransponderDatas = new List<IATMTransponderData>();

        public ATMEventHandler(IEnumerable<ATMWarning> atmWarnings = null, IEnumerable<ATMNotification> atmNotifications = null)
        {
            _atmWarnings = atmWarnings ?? new List<ATMWarning> {new Separation()};
            _atmNotifications = atmNotifications ?? new List<ATMNotification> { new TrackEnteredAirspace(), new TrackLeftAirspace() };
        }

        public void Handle(ICollection<IATMTransponderData> atmTransponderDatas)
        {
            foreach (var warning in _atmWarnings)
            {
                warning.DetectWarning(atmTransponderDatas);
            }

            foreach (var notification in _atmNotifications)
            {
                notification.DetectNotification(_aTMTransponderDatas, atmTransponderDatas);
            }

            _aTMTransponderDatas = atmTransponderDatas;
        }
    }
}