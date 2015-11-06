using System.Collections.Generic;
using System.Linq;
using ATMModel.Data;

namespace ATMModel.Events
{
    /// <summary>
    /// this class implement notification
    /// Handling track entered event
    /// </summary>
    public class TrackEnteredAirspace : ATMNotification
    {
        private readonly IATMLogEvent _atmLog;

        /// <summary>
        /// If special implementation og atm log needed, can this be set from constructor
        /// </summary>
        /// <param name="atmLogEvent"></param>
        public TrackEnteredAirspace(IATMLogEvent atmLogEvent = null)
        {
            _atmLog = atmLogEvent ?? new ATMLogger();
        }

        /// <summary>
        /// Checks for any new tracks entered airspace
        /// track entered event raised if any new tracks exist
        /// new event logged to the file
        /// </summary>
        /// <param name="oldTransponderDatas"></param>
        /// <param name="newTransponderDatas"></param>
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