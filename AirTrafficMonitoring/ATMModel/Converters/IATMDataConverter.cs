using System.Collections.Generic;
using ATMModel.Data;

namespace ATMModel.Converters
{
    public interface IATMDataConverter
    {
        List<IATMTransponderData> Convert(ICollection<string> list );
    }
}