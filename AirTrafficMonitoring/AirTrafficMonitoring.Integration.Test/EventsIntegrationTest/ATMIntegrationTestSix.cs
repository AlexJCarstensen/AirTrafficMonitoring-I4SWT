using ATMModel.Events;
using NSubstitute;
using NUnit.Framework;

namespace AirTrafficMonitoring.Integration.Test.EventsIntegrationTest
{
    [TestFixture]
    public class ATMIntegrationTestSix
    {
        private TrackEnteredAirspace _trackEnteredAirspace;
        private TrackLeftAirspace _trackLeftAirspace;

        [SetUp]
        public void Setup()
        {
            _trackEnteredAirspace = new TrackEnteredAirspace(Substitute.For<IATMLogEvent>());
            _trackLeftAirspace = new TrackLeftAirspace(Substitute.For<IATMLogEvent>());
        }

        //[Test]
        //public void DetectNotification_NoData_NoNotification()
        //{
            
        //}
    }
}