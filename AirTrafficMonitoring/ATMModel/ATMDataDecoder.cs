using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TransponderReceiver;

namespace ATMModel
{
    class ATMDataDecoder : IATMDataDecoder
    {
        public event EventHandler<IEnumerable<IATMTransponderData>> _event;
        

        public ATMDataDecoder(ITransponderReceiver transponderReceiver, IATMDataConverter dataConverter,
            IATMEventHandler eventHandler)
        {
             
        }
        public void OnTransponderDataReady(List<string> list)
        {
            
            var transponderDataItem = new List<IATMTransponderData>();
            transponderDataItem = ((IATMDataConverter) transponderDataItem).Convert(list).FindAll(t => t.Coordinate.Validate);
            //??

        }
    }
}