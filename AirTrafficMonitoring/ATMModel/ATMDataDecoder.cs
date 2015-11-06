using System;
using System.Collections.Generic;
using ATMModel.Converters;
using ATMModel.Data;
using ATMModel.Events;
using TransponderReceiver;

namespace ATMModel
{
    public class ATMDataDecoder : IATMDataDecoder
    {
        private readonly IATMDataConverter _dataConverter;
        private readonly IATMEventHandler _eventHandler;
        public event EventHandler<IEnumerable<IATMTransponderData>> _event;
        

        public ATMDataDecoder(ITransponderReceiver transponderReceiver, IATMDataConverter dataConverter = null,
            IATMEventHandler eventHandler = null)
        {
            _dataConverter = dataConverter ?? new ATMDataConverter(new ATMAngleConverter(), new ATMVelocityConverter());
            _eventHandler = eventHandler ?? new ATMEventHandler();

            if (transponderReceiver != null) transponderReceiver.TransponderDataReady += OnTransponderDataReady;
        }

        public void OnTransponderDataReady(ICollection<string> list)
        {
            if(list?.Count <= 0) return;

            var convertedData = _dataConverter.Convert(list);
            _event?.Invoke(this, convertedData);
            _eventHandler.Handle(convertedData);
        }
    }
}