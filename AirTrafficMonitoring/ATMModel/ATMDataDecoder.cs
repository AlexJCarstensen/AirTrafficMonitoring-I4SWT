using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TransponderReceiver;

namespace ATMModel
{
    public class ATMDataDecoder : IATMDataDecoder
    {
        private readonly IATMDataConverter _dataConverter;
        private readonly IATMEventHandler _eventHandler;
        public event EventHandler<IEnumerable<IATMTransponderData>> _event;
        

        public ATMDataDecoder(ITransponderReceiver transponderReceiver, IATMDataConverter dataConverter,
            IATMEventHandler eventHandler)
        {
            _dataConverter = dataConverter;
            _eventHandler = eventHandler;

            transponderReceiver.TransponderDataReady += OnTransponderDataReady;
        }

        public void OnTransponderDataReady(List<string> list)
        {
            _event?.Invoke(this, _dataConverter.Convert(list));
        }
    }
}