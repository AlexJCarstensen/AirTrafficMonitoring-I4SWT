using System.Collections.Generic;
using ATMModel.Data;
using ATMModel.Events;
using NSubstitute;
using NUnit.Framework;

namespace AirTrafficMonitoring.Unit.Test.EventsUnitTest
{
    public class TrackEnteredAirspaceUnitTest
    {
        private TrackEnteredAirspace _uut;
        private IATMLogEvent _atmLog;
        private IATMTransponderData _atmTransponderData1;
        private IATMTransponderData _atmTransponderData2;

        [SetUp]
        public void Setup()
        {
            _uut = new TrackEnteredAirspace(_atmLog = Substitute.For<IATMLogEvent>());
            _atmTransponderData1 = Substitute.For<IATMTransponderData>();
            _atmTransponderData2 = Substitute.For<IATMTransponderData>();
            _atmTransponderData1.Tag = "item1";
            _atmTransponderData1.Timestamp = "1221";
            _atmTransponderData2.Tag = "item2";
        }

        [Test]
        public void DetectNotification_NoData_NoEvent()
        {
            int notificationEventCalled = 0;
            ATMNotification.NotificationEvent += (sender, args) => notificationEventCalled++;

            _uut.DetectNotification(new List<IATMTransponderData>(), new List<IATMTransponderData>());
            Assert.That(notificationEventCalled, Is.EqualTo(0));
        }

        [Test]
        public void DetectNotification_OneEnters_OneEvent()
        {
            int notificationEventCalled = 0;
            ATMNotification.NotificationEvent += (sender, args) => notificationEventCalled++;

            _uut.DetectNotification(new List<IATMTransponderData>(), new List<IATMTransponderData> {_atmTransponderData1});
            Assert.That(notificationEventCalled, Is.EqualTo(1));
        }

        [Test]
        public void DetectNotification_ThreeEnters_ThreeEvent()
        {
            int notificationEventCalled = 0;
            ATMNotification.NotificationEvent += (sender, args) => notificationEventCalled++;

            _uut.DetectNotification(new List<IATMTransponderData>(), new List<IATMTransponderData> { _atmTransponderData1, _atmTransponderData2, _atmTransponderData1 });
            Assert.That(notificationEventCalled, Is.EqualTo(3));
        }

        [Test]
        public void DetectNotification_ExistingFlight_NoEvent()
        {
            int notificationEventCalled = 0;
            ATMNotification.NotificationEvent += (sender, args) => notificationEventCalled++;

            _uut.DetectNotification(new List<IATMTransponderData> { _atmTransponderData1 }, new List<IATMTransponderData> { _atmTransponderData1 });
            Assert.That(notificationEventCalled, Is.EqualTo(0));
        }

        [Test]
        public void DetectNotification_OneTrackEntereds_EventLoggedOnce()
        {
            _uut.DetectNotification(new List<IATMTransponderData> (), new List<IATMTransponderData> { _atmTransponderData1 });
            _atmLog.Received(1).Log("1221 TrackEnteredAirspace Notification item1");
        }

    }
}