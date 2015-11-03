using System;
using System.Collections.Generic;
using ATMModel.Data;

namespace ATMModel
{
    public interface IATMDataDecoder
    {
        event EventHandler<IEnumerable<IATMTransponderData>> _event;
        void OnTransponderDataReady(List<string> list);
    }
}