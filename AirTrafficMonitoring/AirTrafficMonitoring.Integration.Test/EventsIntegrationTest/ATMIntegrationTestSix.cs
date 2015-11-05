using System;
using System.Collections.Generic;
using System.Threading;
using ATMModel.Data;
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

        [Test]
        public void DetectNotification_NoData_NoNotification()
        {
            var notificationDetected = 0;
            EventHandler<NotificationEventArgs> locDelegate = delegate { notificationDetected++; };
            ATMNotification.NotificationEvent += locDelegate;
            var oldList = new List<IATMTransponderData>();
            var newList = new List<IATMTransponderData>();
            _trackEnteredAirspace.DetectNotification(oldList, newList);
            _trackLeftAirspace.DetectNotification(oldList, newList);
            ATMNotification.NotificationEvent -= locDelegate;

            Assert.That(notificationDetected, Is.EqualTo(0));
        }

        [Test]
        public void DetectNotification_TrackEntered_Notified()
        {
            var notificationDetected = 0;
            EventHandler<NotificationEventArgs> locDelegate = delegate (object sender, NotificationEventArgs args) {
                if (((TrackEnteredAirspace)sender) != null && args.Tag == "GYJ824")
                    notificationDetected++;
            };
            ATMNotification.NotificationEvent += locDelegate;
            var oldList = new List<IATMTransponderData>();
            var newList = new List<IATMTransponderData> { new ATMTransponderData("GYJ824", new ATMCoordinate(78653, 94274, 1024), "2015") };
            _trackEnteredAirspace.DetectNotification(oldList, newList);
            _trackLeftAirspace.DetectNotification(oldList, newList);
            ATMNotification.NotificationEvent -= locDelegate;

            Assert.That(notificationDetected, Is.EqualTo(1));
        }

        [Test]
        public void DetectNotification_TrackLeft_Notified()
        {
            var notificationDetected = 0;
            EventHandler<NotificationEventArgs> locDelegate = delegate (object sender, NotificationEventArgs args){
                if (((TrackLeftAirspace)sender) != null && args.Tag == "GYJ824")
                    notificationDetected++;
            };
            ATMNotification.NotificationEvent += locDelegate;
            
            var oldList = new List<IATMTransponderData> { new ATMTransponderData("GYJ824", new ATMCoordinate(78653, 94274, 1024), "2015") };
            var newList = new List<IATMTransponderData>();
            _trackEnteredAirspace.DetectNotification(oldList, newList);
            _trackLeftAirspace.DetectNotification(oldList, newList);
            ATMNotification.NotificationEvent -= locDelegate;

            Assert.That(notificationDetected, Is.EqualTo(1));
        }

        [Test]
        public void DetectNotification_TrackEntered_CalledStopMeAfter11Sec()
        {
            AutoResetEvent stopMeCalled = new AutoResetEvent(false);
            EventHandler<NotificationEventArgs> locDelegate = delegate (object sender, NotificationEventArgs args){
                if (((TrackEnteredAirspace)sender) != null && args.Tag == "GYJ824")
                    args.StopMeEvent += (o, eventArgs) => stopMeCalled.Set();
            };
            ATMNotification.NotificationEvent += locDelegate;
            
            var oldList = new List<IATMTransponderData>();
            var newList = new List<IATMTransponderData> { new ATMTransponderData("GYJ824", new ATMCoordinate(78653, 94274, 1024), "2015") };
            _trackEnteredAirspace.DetectNotification(oldList, newList);
            _trackLeftAirspace.DetectNotification(oldList, newList);
            ATMNotification.NotificationEvent -= locDelegate;

            Assert.IsTrue(stopMeCalled.WaitOne(TimeSpan.FromSeconds(11)));
        }

        [Test]
        public void DetectNotification_TrackEntered_NotCalledStopMeAfter9Sec()
        {
            AutoResetEvent stopMeCalled = new AutoResetEvent(false);
            EventHandler<NotificationEventArgs> locDelegate = delegate (object sender, NotificationEventArgs args){
                if (((TrackEnteredAirspace)sender) != null && args.Timestamp == "2015")
                    args.StopMeEvent += (o, eventArgs) => stopMeCalled.Set();
            };
            ATMNotification.NotificationEvent += locDelegate;
            
            var oldList = new List<IATMTransponderData>();
            var newList = new List<IATMTransponderData> { new ATMTransponderData("GYJ824", new ATMCoordinate(78653, 94274, 1024), "2015") };
            _trackEnteredAirspace.DetectNotification(oldList, newList);
            _trackLeftAirspace.DetectNotification(oldList, newList);
            ATMNotification.NotificationEvent -= locDelegate;

            Assert.IsFalse(stopMeCalled.WaitOne(TimeSpan.FromSeconds(9)));
        }
    }
}