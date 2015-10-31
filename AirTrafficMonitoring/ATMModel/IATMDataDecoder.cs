using System;
using System.Collections.Generic;

namespace ATMModel
{
    public interface IATMDataDecoder
    {
        event EventHandler<IEnumerable<IATMTransponderData>> _event;
        void OnTransponderDataReady(List<string> list);
    }
}