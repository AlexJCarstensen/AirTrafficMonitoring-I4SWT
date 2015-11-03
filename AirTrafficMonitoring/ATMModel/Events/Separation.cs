using System;

namespace ATMModel.Events
{
    public class Separation : ATMWarning
    {
        private static event EventHandler<string> WarningEvent;
    }
}