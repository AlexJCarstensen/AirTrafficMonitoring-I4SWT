using System;

namespace ATMModel.Events
{
    public class TrackLeftAirspace : ATMNotification
    {
        private static event EventHandler<string> TrackLeftEvent;
    }
}