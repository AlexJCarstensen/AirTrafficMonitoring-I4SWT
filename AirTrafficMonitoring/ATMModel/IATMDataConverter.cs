using System;
using System.Collections.Generic;

namespace ATMModel
{
    public interface IATMDataConverter
    {
        List<IATMTransponderData> Convert(List<string> list );
    }
}