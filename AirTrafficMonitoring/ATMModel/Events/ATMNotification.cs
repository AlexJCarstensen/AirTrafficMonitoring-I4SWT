using System;
using System.Collections.Generic;

namespace ATMModel.Events
{
    public abstract class ATMNotification
    {
        public static event EventHandler<NotificationEventArgs> NotificationEvent;

        abstract public void DetectNotification(List<IATMTransponderData> oldTransponderDatas, List<IATMTransponderData> newTransponderDatas);

        protected virtual void Notify(NotificationEventArgs e)
        {
            NotificationEvent?.Invoke(this, e);
        }
    }
}