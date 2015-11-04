using System.Collections.Generic;
using ATMModel.Events;
using NSubstitute;
using NUnit.Framework;

namespace AirTrafficMonitoring.Integration.Test.EventsIntegrationTest
{
    [TestFixture]
    public class ATMEventHandlerIntegrationTest
    {
        private ATMEventHandler _atmEventHandler;
        private Separation _separation;
        private TrackEnteredAirspace _trackEnteredAirspace;
        private TrackLeftAirspace _trackLeftAirspace;
        private IATMLogEvent _atmLogEvent;

        [SetUp]
        public void Setup()
        {
            _atmLogEvent = Substitute.For<IATMLogEvent>();
            _separation = new Separation(_atmLogEvent);
            _trackEnteredAirspace = new TrackEnteredAirspace(_atmLogEvent);
            _trackLeftAirspace = new TrackLeftAirspace(_atmLogEvent);
            _atmEventHandler = new ATMEventHandler(new List<ATMWarning> {_separation}, new List<ATMNotification> {_trackEnteredAirspace, _trackLeftAirspace});
        }
        
    }
}