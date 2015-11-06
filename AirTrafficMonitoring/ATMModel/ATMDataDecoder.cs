using System;
using System.Collections.Generic;
using ATMModel.Converters;
using ATMModel.Data;
using ATMModel.Events;
using TransponderReceiver;

namespace ATMModel
{
    /// <summary>
    /// This is our mother class that handles all incomming data and 
    /// delegates to appropriate classes to convert and handle events
    /// then sends the track data to consumers of _event
    /// </summary>
    public class ATMDataDecoder : IATMDataDecoder
    {
        private readonly IATMDataConverter _dataConverter;
        private readonly IATMEventHandler _eventHandler;
        public event EventHandler<IEnumerable<IATMTransponderData>> _event;

        /// <summary>
        /// Constructor expect atleast transponderReceiver to get data
        /// if dataconverter and eventhandler is not provided, default objects will be created
        /// </summary>
        /// <param name="transponderReceiver"></param>
        /// <param name="dataConverter"></param>
        /// <param name="eventHandler"></param>
        public ATMDataDecoder(ITransponderReceiver transponderReceiver, IATMDataConverter dataConverter = null,
            IATMEventHandler eventHandler = null)
        {
            _dataConverter = dataConverter ?? new ATMDataConverter(new ATMAngleConverter(), new ATMVelocityConverter());
            _eventHandler = eventHandler ?? new ATMEventHandler();

            if (transponderReceiver != null) transponderReceiver.TransponderDataReady += OnTransponderDataReady;
        }

        /// <summary>
        /// observe list from ITransponderReceiver
        /// convert to tracks
        /// delegates to oberservers and delegate the same data to eventhandler to handle all events
        /// </summary>
        /// <param name="list"></param>
        public void OnTransponderDataReady(ICollection<string> list)
        {
            if(list?.Count <= 0) return;

            var convertedData = _dataConverter.Convert(list);
            _event?.Invoke(this, convertedData);
            _eventHandler.Handle(convertedData);
        }
    }
}