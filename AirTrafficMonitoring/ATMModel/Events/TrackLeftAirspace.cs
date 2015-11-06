using System.Collections.Generic;
using System.Linq;
using ATMModel.Data;

namespace ATMModel.Events
{
    /// <summary>
    /// this class implement notification
    /// Handling track left event
    /// </summary>
    public class TrackLeftAirspace : ATMNotification
    {
        private readonly IATMLogEvent _atmLog;

        /// <summary>
        /// If special implementation og atm log needed, can this be set from constructor
        /// </summary>
        /// <param name="atmLogEvent"></param>
        public TrackLeftAirspace(IATMLogEvent atmLogEvent = null)
        {
            _atmLog = atmLogEvent ?? new ATMLogger();
        }

        /// <summary>
        /// Checks for any tracks left airspace
        /// track left event raised if any tracks leaves the airspace
        /// new event logged to the file
        /// </summary>
        /// <param name="oldTransponderDatas"></param>
        /// <param name="newTransponderDatas"></param>
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