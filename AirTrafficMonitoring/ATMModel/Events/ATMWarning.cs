using System;
using System.Collections.Generic;

namespace ATMModel.Events
{
    public abstract class ATMWarning
    {
        public void DetectWarning(IEnumerable<IATMTransponderData> oldTransponderDatas, IEnumerable<IATMTransponderData> newTransponderDatas )
        {
            
        }
    }
}