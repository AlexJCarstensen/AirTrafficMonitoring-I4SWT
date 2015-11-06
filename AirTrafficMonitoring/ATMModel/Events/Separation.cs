using System;
using System.Collections.Generic;
using System.Linq;
using ATMModel.Data;

namespace ATMModel.Events
{
    public class Separation : ATMWarning
    {
        private readonly List<WarningEventArgs> _notifiedEventsList = new List<WarningEventArgs>();
        private readonly IATMLogEvent _atmLog;
        private readonly string[] _logString = { " Separation Warning ", " Activated", " Deactivated" };

        public Separation(IATMLogEvent atmLog = null)
        {
            _atmLog = atmLog ?? new ATMLogger();
        }

        public override void DetectWarning(ICollection<IATMTransponderData> newTransponderDatas)
        {
            List<WarningEventArgs> localNotifiedEvents = new List<WarningEventArgs>(_notifiedEventsList);
            _notifiedEventsList.Clear();
            
            using (var e = newTransponderDatas.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    foreach (var item in newTransponderDatas)
                    {
                        // ReSharper disable once PossibleNullReferenceException
                        if (!SeparationCheck(e.Current, item) || item.Tag == e.Current.Tag) continue;

                        var currentNotification = new WarningEventArgs(item.Tag, e.Current.Tag, "Separation", item.Timestamp);
                        if (_notifiedEventsList.Any(t => t.Tag1 == currentNotification.Tag2 && t.Tag2 == currentNotification.Tag1)) continue;

                        if (
                            localNotifiedEvents.Any(
                                t =>
                                    t.Tag1 == item.Tag
                                    || t.Tag1 == e.Current.Tag
                                    && t.Tag2 == item.Tag
                                    || t.Tag2 == e.Current.Tag))
                        {
                            _notifiedEventsList.Add(currentNotification);
                            localNotifiedEvents.Remove(localNotifiedEvents.First(
                                t =>
                                    t.Tag1 == item.Tag
                                    || t.Tag1 == e.Current.Tag
                                    && t.Tag2 == item.Tag
                                    || t.Tag2 == e.Current.Tag));
                            continue;
                        }
                        Notify(currentNotification);
                        _notifiedEventsList.Add(currentNotification);
                        _atmLog.Log(item.Timestamp +_logString[0] + item.Tag + " " + e.Current.Tag + _logString[1]);
                    }
                }
            }
            foreach (var t in localNotifiedEvents)
            {
                Notify(new WarningEventArgs(t.Tag1, t.Tag2, "Separation", t.Timestamp, false));
                _atmLog.Log(t.Timestamp + _logString[0] + t.Tag1 + " " + t.Tag2 + _logString[2]);
            }
        }

        public bool SeparationCheck(IATMTransponderData data1, IATMTransponderData data2)
        {
            return data2 != null && ((data2.Coordinate.Z - data1.Coordinate.Z) < 300 &&
                                     (data2.Coordinate.Z - data1.Coordinate.Z) > -300 &&
                                     Math.Sqrt(Math.Pow(data2.Coordinate.Y - data1.Coordinate.Y, 2) + 
                                               Math.Pow(data2.Coordinate.X - data1.Coordinate.X, 2)) < 5000);
        }
    }
}