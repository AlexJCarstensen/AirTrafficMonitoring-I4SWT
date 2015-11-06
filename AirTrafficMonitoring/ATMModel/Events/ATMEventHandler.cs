using System.Collections.Generic;
using System.Linq;
using ATMModel.Data;

namespace ATMModel.Events
{
    /// <summary>
    /// This class delegates data to all warning and notification collection
    /// </summary>
    public class ATMEventHandler : IATMEventHandler
    {
        private readonly IEnumerable<ATMWarning> _atmWarnings;
        private readonly IEnumerable<ATMNotification> _atmNotifications;
        private ICollection<IATMTransponderData> _atmTransponderDatas = new List<IATMTransponderData>();

        public ATMEventHandler(IEnumerable<ATMWarning> atmWarnings = null, IEnumerable<ATMNotification> atmNotifications = null)
        {
            _atmWarnings = atmWarnings ?? new List<ATMWarning> {new Separation()};
            _atmNotifications = atmNotifications ?? new List<ATMNotification> { new TrackEnteredAirspace(), new TrackLeftAirspace() };
        }

        /// <summary>
        /// Delelegates data to all warning and notification 
        /// </summary>
        /// <param name="atmTransponderDatas"></param>
        public void Handle(ICollection<IATMTransponderData> atmTransponderDatas)
        {
            foreach (var warning in _atmWarnings)
            {
                warning.DetectWarning(atmTransponderDatas);
            }

            foreach (var notification in _atmNotifications)
            {
                notification.DetectNotification(_atmTransponderDatas, atmTransponderDatas);
            }

            _atmTransponderDatas = atmTransponderDatas;
        }
    }
}