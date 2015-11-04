using System;
using System.Collections.Generic;
using System.Linq;
using ATMModel.Data;

namespace ATMModel.Events
{
    public class Separation : ATMWarning
    {
        private readonly List<WarningEventArgs> _notifiedEvents = new List<WarningEventArgs>();
        private readonly IATMLogEvent _atmLog;

        public Separation(IATMLogEvent atmLog = null)
        {
            _atmLog = atmLog ?? new ATMLogger();
        }

        public override void DetectWarning(List<IATMTransponderData> newTransponderDatas)
        {
            List<WarningEventArgs> localNotifiedEvents = new List<WarningEventArgs>(_notifiedEvents);
            _notifiedEvents.Clear();
            
            using (var e = newTransponderDatas.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    foreach (var item in newTransponderDatas)
                    {
                        if (!SeparationCheck(e.Current, item) || item.Tag == e.Current.Tag) continue;

                        var currentNotification = new WarningEventArgs(item.Tag, e.Current.Tag, "Separation", item.Timestamp);
                        if (_notifiedEvents.Any(t => t.Tag1 == currentNotification.Tag2 && t.Tag2 == currentNotification.Tag1)) continue;

                        if (
                            localNotifiedEvents.Any(
                                t =>
                                    t.Tag1 == item.Tag
                                    || t.Tag1 == e.Current.Tag
                                    && t.Tag2 == item.Tag
                                    || t.Tag2 == e.Current.Tag))
                        {
                            _notifiedEvents.Add(currentNotification);
                            localNotifiedEvents.Remove(localNotifiedEvents.First(
                                t =>
                                    t.Tag1 == item.Tag
                                    || t.Tag1 == e.Current.Tag
                                    && t.Tag2 == item.Tag
                                    || t.Tag2 == e.Current.Tag));
                            continue;
                        }
                        Notify(currentNotification);
                        _notifiedEvents.Add(currentNotification);
                        _atmLog.Log(item.Timestamp + " Separation Warning " + item.Tag + " " + e.Current.Tag + " Activated");
                    }
                }
            }
            foreach (var t in localNotifiedEvents)
            {
                Notify(new WarningEventArgs(t.Tag1, t.Tag2, "Separation", t.Timestamp, false));
                _atmLog.Log(t.Timestamp + " Separation Warning " + t.Tag1 + " " + t.Tag2 + " Deactivated");
            }
        }

        public bool SeparationCheck(IATMTransponderData data1, IATMTransponderData data2)
        {
            return (data2.Coordinate.Z - data1.Coordinate.Z) < 300 &&
                (data2.Coordinate.Z - data1.Coordinate.Z) > -300 &&
                Math.Sqrt(Math.Pow(data2.Coordinate.Y - data1.Coordinate.Y, 2) + 
                Math.Pow(data2.Coordinate.X - data1.Coordinate.X, 2)) < 5000;
        }
    }
}