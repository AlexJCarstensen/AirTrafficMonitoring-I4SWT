using System;
using System.Collections.Generic;

namespace ATMModel
{
    class ATMEventHandler : IATMEventHandler
    {
        void IATMEventHandler.Handle(IEnumerable<IATMTransponderData> data)
        {
            throw new NotImplementedException();
        }
    }
}