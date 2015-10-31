using System.Collections.Generic;
using System.Linq;

namespace ATMModel
{
    public class ATMDataConverter : IATMDataConverter
    {
        private static readonly List<IATMTransponderData> LocaDataItems = new List<IATMTransponderData>();
        public ATMDataConverter(IATMTransponderData data, IATMAngleConverter angle, IATMVelocityConverter velocity)
        {
        }
        public List<IATMTransponderData> Convert(List<string> list) 
        {
            if(LocaDataItems.Count > 0) 
                LocaDataItems.Clear();
            foreach (var data in list.Select(item => item.Split(';')))
            {
                LocaDataItems.Add(new ATMTransponderData(
                    data[0],
                    int.Parse(data[1]),
                    int.Parse(data[2]),
                    int.Parse(data[3]),
                    data[4], 0, 0
                    ));
            }
            return LocaDataItems;
            
        }
    }
}