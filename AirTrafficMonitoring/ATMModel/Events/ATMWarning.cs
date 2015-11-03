using System;
using System.Collections.Generic;

namespace ATMModel.Events
{
    public abstract class ATMWarning
    {
        private static event EventHandler<WarningEventArgs> WarningEvent;
        public abstract void DetectWarning(List<IATMTransponderData> newTransponderDatas);

        protected virtual void Notify(WarningEventArgs e)
        {
            WarningEvent?.Invoke(this, e);
        }
    }
}