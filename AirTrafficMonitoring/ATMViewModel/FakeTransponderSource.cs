using System;
using System.Collections.Generic;
using System.Windows.Threading;
using TransponderReceiver;

namespace ATMViewModel
{
    public class FakeTransponderSource : ITransponderReceiver
    {
        public event TransponderDataReadyHandler TransponderDataReady;
        int x = 10000;
        int y = 10000;
        private int x1 = 20000;
        public FakeTransponderSource()
        {
            DispatcherTimer _timer = new DispatcherTimer();
            _timer.Tick += (sender, args) =>
            {

                var list = new List<string>
                {
                    "E15;" + (x += 100) + ";" + (y += 100) + ";5000;20151012124523456",
                    "E16;" + (x1 -= 100) + ";" + (y += 100) + ";5000;20151012124523456"
                };
                TransponderDataReady?.Invoke(list);
            };
            _timer.Interval = new TimeSpan(0,0,1);
            _timer.Start();
        }
    }
}