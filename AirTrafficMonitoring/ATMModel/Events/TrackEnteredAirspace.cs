using System;

namespace ATMModel.Events
{
    public class TrackEnteredAirspace : ATMNotification
    {
        private static event EventHandler<string> TrackEnterEvent;

      
    }
}