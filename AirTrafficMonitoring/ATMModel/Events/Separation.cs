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
                        var vertical = (item.Coordinate.Z - e.Current.Coordinate.Z) < 300 &&
                                        (item.Coordinate.Z - e.Current.Coordinate.Z) > -300;
                        var horizontal = Math.Atan2(item.Coordinate.Y - e.Current.Coordinate.Y, item.Coordinate.X - e.Current.Coordinate.X) < 5000;

                        if (!vertical || !horizontal || item.Tag == e.Current.Tag) continue;

                        if (
                            _notifiedEvents.Any(
                                t =>
                                    t.Tag1 == item.Tag 
                                    || t.Tag1 == e.Current.Tag 
                                    && t.Tag2 == item.Tag 
                                    || t.Tag2 == e.Current.Tag))
                        {
                            _notifiedEvents.Add(new WarningEventArgs(item.Tag, e.Current.Tag, "Separation"));
                            _notifiedEvents.Remove(_notifiedEvents.First(
                                t =>
                                    t.Tag1 == item.Tag 
                                    || t.Tag1 == e.Current.Tag 
                                    && t.Tag2 == item.Tag 
                                    || t.Tag2 == e.Current.Tag));
                            continue;
                        }
                        Notify(new WarningEventArgs(item.Tag, e.Current.Tag, "Separation"));
                        _atmLog.Log(item.Timestamp + " Separation Warning " + item.Tag + " " + e.Current.Tag + " Activated");
                    }
                }
            }
            foreach (var t in localNotifiedEvents)
            {
                Notify(new WarningEventArgs(t.Tag1, t.Tag2, "Separation", false));
            }
        }
    }
}