using System;
using System.Collections.Generic;
using ATMModel.Data;

namespace ATMModel.Events
{
    /// <summary>
    /// This class notify all warnings with a static event
    /// </summary>
    public abstract class ATMWarning
    {
        public static event EventHandler<WarningEventArgs> WarningEvent;
        public abstract void DetectWarning(ICollection<IATMTransponderData> newTransponderDatas);
        
        protected virtual void Notify(WarningEventArgs e)
        {
            WarningEvent?.Invoke(this, e);
        }
    }
}