using System;
using System.Collections.Generic;
using ATMModel.Data;

namespace ATMModel.Events
{
    /// <summary>
    /// This class notify all notification with a static event
    /// </summary>
    public abstract class ATMNotification
    {
        public static event EventHandler<NotificationEventArgs> NotificationEvent;

        abstract public void DetectNotification(ICollection<IATMTransponderData> oldTransponderDatas, ICollection<IATMTransponderData> newTransponderDatas);

        protected virtual void Notify(NotificationEventArgs e)
        {
            NotificationEvent?.Invoke(this, e);
        }
    }
}