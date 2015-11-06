using System;
using System.Collections.Generic;
using System.Linq;
using ATMModel.Data;

namespace ATMModel.Events
{
    /// <summary>
    /// This class implement our warning
    /// </summary>
    public class Separation : ATMWarning
    {
        private readonly ICollection<WarningEventArgs> _notifiedWarningEventArgses = new List<WarningEventArgs>();
        private readonly IATMLogEvent _atmLog;
        private readonly string[] _logString = { " Separation Warning ", " Activated", " Deactivated", " " };

        /// <summary>
        /// If special implementation og atm log needed, can this be set from constructor
        /// </summary>
        /// <param name="atmLog"></param>
        public Separation(IATMLogEvent atmLog = null)
        {
            _atmLog = atmLog ?? new ATMLogger();
        }

        /// <summary>
        /// Funtion take newTransponderDatas and check for separation events
        /// Function notify sepation event, if exist
        /// </summary>
        /// <param name="newTransponderDatas"></param>
        public override void DetectWarning(ICollection<IATMTransponderData> newTransponderDatas)
        {
            // null reference check
            if(newTransponderDatas == null) throw new ArgumentNullException(nameof(newTransponderDatas), nameof(newTransponderDatas) + " is null");

            // copy all notified separation detail to this local variable and 
            // clear global separation notified collection
            var localNotifiedEvents = new List<WarningEventArgs>(_notifiedWarningEventArgses);
            _notifiedWarningEventArgses.Clear();
            
            // iterate and check for separation
            using (var e = newTransponderDatas?.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    foreach (var item in newTransponderDatas)
                    {
                        // ReSharper disable once PossibleNullReferenceException
                        if (!SeparationCheck(e.Current, item) || item.Tag == e.Current.Tag) continue;
                        // separation detected 
                        var currentNotification = new WarningEventArgs(item.Tag, e.Current.Tag, "Separation", item.Timestamp);
                        // checks for separation have been raised
                        if (_notifiedWarningEventArgses.Any(t => t.Tag1 == currentNotification.Tag2 && t.Tag2 == currentNotification.Tag1)) continue;
                        // check for separation event exist in local raised separation collection
                        if (
                            localNotifiedEvents.Any(
                                t =>
                                    t.Tag1 == item.Tag
                                    || t.Tag1 == e.Current.Tag
                                    && t.Tag2 == item.Tag
                                    || t.Tag2 == e.Current.Tag))
                        {
                            // if separation exist in local collection, 
                            //that separation event being removed here
                            _notifiedWarningEventArgses.Add(currentNotification);
                            localNotifiedEvents.Remove(localNotifiedEvents.First(
                                t =>
                                    t.Tag1 == item.Tag
                                    || t.Tag1 == e.Current.Tag
                                    && t.Tag2 == item.Tag
                                    || t.Tag2 == e.Current.Tag));
                            continue;
                        }
                        // if a separation survives all check before, 
                        //that separation is new one and added to global separation collection
                        // and that separation being logged also
                        Notify(currentNotification);
                        _notifiedWarningEventArgses.Add(currentNotification);
                        _atmLog.Log(item.Timestamp +_logString[0] + item.Tag + _logString[3] + e.Current.Tag + _logString[1]);
                    }
                }
            }
            foreach (var t in localNotifiedEvents)
            {
                // if local notification contains any separation 
                // means that separation is not active more
                // separation not active event raised
                Notify(new WarningEventArgs(t.Tag1, t.Tag2, "Separation", t.Timestamp, false));
                _atmLog.Log(t.Timestamp + _logString[0] + t.Tag1 + _logString[3] + t.Tag2 + _logString[2]);
            }
        }

        /// <summary>
        /// This function calculate about two track is in conflict
        /// </summary>
        /// <param name="data1"></param>
        /// <param name="data2"></param>
        /// <returns>
        /// true if collition exist
        /// false else
        /// </returns>
        public static bool SeparationCheck(IATMTransponderData data1, IATMTransponderData data2)
        {
            if (data1 == null || data2 == null) throw new ArgumentNullException(data1 == null ? nameof(data1) : nameof(data2), nameof(data1) + " or " + nameof(data2) + " is null");
            return (Math.Abs((data2.Coordinate.Z - data1.Coordinate.Z)) < 300 &&
                                     Math.Sqrt(Math.Pow(data2.Coordinate.Y - data1.Coordinate.Y, 2) + 
                                               Math.Pow(data2.Coordinate.X - data1.Coordinate.X, 2)) < 5000);
        }
    }
}