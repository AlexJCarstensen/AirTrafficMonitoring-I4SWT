using System;
using System.Collections.Generic;
using ATMModel.Data;

namespace ATMModel.Events
{
    public class Separation : ATMWarning
    {
        public override void DetectWarning(List<IATMTransponderData> newTransponderDatas)
        {
            string notifiedCollision = "";
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

                        if (notifiedCollision.Contains(item.Tag)) continue;
                        notifiedCollision += (" " + item.Tag + " " + e.Current.Tag);
                        Notify(new WarningEventArgs(item.Tag, e.Current.Tag, "Separation"));
                    }
                }
            }
        }
    }
}