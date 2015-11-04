using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using ATMModel.Data;
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

        [Test]
        public void EventHandler_OneSeparation_SeparationRaised()
        {
            var separationRaised = false;
            ATMWarning.WarningEvent += (sender, args) => separationRaised = true;

            _atmEventHandler.Handle(new List<IATMTransponderData> {new ATMTransponderData("F12",17650, 29874, 5000, "2015"),
                new ATMTransponderData("F15",17150, 29274, 5070, "2015")});

            Assert.That(separationRaised, Is.True);
        }

        [Test]
        public void EventHandler_NoSeparation_SeparationNotRaised()
        {
            var separationRaised = false;
            ATMWarning.WarningEvent += (sender, args) =>
            {
                if(args.Active)
                    separationRaised = true;
            };

            _atmEventHandler.Handle(new List<IATMTransponderData> {new ATMTransponderData("F12",17650, 29874, 5000, "2015"),
                new ATMTransponderData("F15",87150, 29274, 5900, "2015")});

            Assert.That(separationRaised, Is.False);
        }

        [Test]
        public void EventHandler_SeparationNotActive_SeparationRaisedWithNotActive()
        {
            var separationRaised = false;
            ATMWarning.WarningEvent += (sender, args) =>
            {
                if (!args.Active)
                    separationRaised = true;
            };

            _atmEventHandler.Handle(new List<IATMTransponderData> {new ATMTransponderData("F12",17650, 29874, 5000, "2015"),
                new ATMTransponderData("F15",17150, 29274, 5070, "2015")});
            _atmEventHandler.Handle(new List<IATMTransponderData> {new ATMTransponderData("F12",17650, 29874, 5000, "2015"),
                new ATMTransponderData("F15",17150, 29274, 5700, "2015")});

            Assert.That(separationRaised, Is.True);
        }

        [Test]
        public void EventHandler_SeparationActive_SeparationRaisedWithNotActiveNotCalled()
        {
            var separationRaised = false;
            ATMWarning.WarningEvent += (sender, args) =>
            {
                if (!args.Active)
                    separationRaised = true;
            };

            _atmEventHandler.Handle(new List<IATMTransponderData> {new ATMTransponderData("F12",17650, 29874, 5000, "2015"),
                new ATMTransponderData("F15",17150, 29274, 5070, "2015")});
            _atmEventHandler.Handle(new List<IATMTransponderData> {new ATMTransponderData("F12",17650, 29874, 5000, "2015"),
                new ATMTransponderData("F15",17150, 29274, 5200, "2015")});

            Assert.That(separationRaised, Is.False);
        }

        [Test]
        public void EventHandler_TwoTrackEntered_TwoEventRaised()
        {
            var eventCounter = 0;
            ATMNotification.NotificationEvent += (sender, args) =>
            {
                if(args.EventName == "TrackEnteredAirspace")
                    eventCounter++;
            };

            _atmEventHandler.Handle(new List<IATMTransponderData> {new ATMTransponderData("F12",17650, 29874, 5000, "2015"),
                new ATMTransponderData("F15",17150, 29274, 5070, "2015")});

            Assert.That(eventCounter, Is.EqualTo(2));
        }

        [Test]
        public void EventHandler_NoTrackEntered_EventNotRaised()
        {
            var eventCounter = 0;
            ATMNotification.NotificationEvent += (sender, args) =>
            {
                if (args.EventName == "TrackEnteredAirspace")
                    eventCounter++;
            };

            _atmEventHandler.Handle(new List<IATMTransponderData>());

            Assert.That(eventCounter, Is.EqualTo(0));
        }

        [Test]
        public void EventHandler_TwoTrackLeft_TwoEventRaised()
        {
            var eventCounter = 0;
            ATMNotification.NotificationEvent += (sender, args) =>
            {
                if (args.EventName == "TrackLeftAirspace")
                    eventCounter++;
            };

            _atmEventHandler.Handle(new List<IATMTransponderData> {new ATMTransponderData("F12",17650, 29874, 5000, "2015"),
                new ATMTransponderData("F15",17150, 29274, 5070, "2015")});
            _atmEventHandler.Handle(new List<IATMTransponderData>());

            Assert.That(eventCounter, Is.EqualTo(2));
        }

        [Test]
        public void EventHandler_NotificatioEventArgs_StopMeEventCalledAfter10Sec()
        {
            AutoResetEvent eventRaised = new AutoResetEvent(false);
            ATMNotification.NotificationEvent += (sender, args) =>
            {
                args.StopMeEvent += (o, eventArgs) =>
                {
                    eventRaised.Set();
                };
            };

            _atmEventHandler.Handle(new List<IATMTransponderData> {new ATMTransponderData("F12",17650, 29874, 5000, "2015")});

            Assert.IsTrue(eventRaised.WaitOne(TimeSpan.FromSeconds(11)));
        }

        [Test]
        public void EventHandler_NotificatioEventArgs_StopMeEventNotCalledBefore10Sec()
        {
            AutoResetEvent eventRaised = new AutoResetEvent(false);
            ATMNotification.NotificationEvent += (sender, args) =>
            {
                args.StopMeEvent += (o, eventArgs) =>
                {
                    eventRaised.Set();
                };
            };

            _atmEventHandler.Handle(new List<IATMTransponderData> { new ATMTransponderData("F12", 17650, 29874, 5000, "2015") });

            Assert.IsFalse(eventRaised.WaitOne(TimeSpan.FromSeconds(9)));
        }

        [Test]
        public void EventHandler_Loggning_SeparationEvent()
        {
            File.WriteAllText(@"ATMLogger.txt", "Cleared");
            var separation = new Separation();
            var atmEventHandler = new ATMEventHandler(new List<ATMWarning> { separation }, new List<ATMNotification> { _trackEnteredAirspace, _trackLeftAirspace });
            atmEventHandler.Handle(new List<IATMTransponderData> {new ATMTransponderData("EventHandler_Log_SeparationEventTest",17650, 29874, 5000, "2015"),
                new ATMTransponderData("F15",17150, 29274, 5070, "2015")});

            var fileConsistOurString = (File.ReadLines(@"ATMLogger.txt").Last()).Contains("EventHandler_Log_SeparationEventTest");

            Assert.IsTrue(fileConsistOurString);
        }

        [Test]
        public void EventHandler_NotLoggning_SeparationEventNotRaised()
        {
            File.WriteAllText(@"ATMLogger.txt", "Cleared");
            var separation = new Separation();
            var atmEventHandler = new ATMEventHandler(new List<ATMWarning> { separation }, new List<ATMNotification> { _trackEnteredAirspace, _trackLeftAirspace });
            atmEventHandler.Handle(new List<IATMTransponderData> {new ATMTransponderData("EventHandler_Log_SeparationEventTest",17650, 29874, 5000, "2015"),
                new ATMTransponderData("F15",17150, 29274, 5500, "2015")});

            var fileConsistOurString = (File.ReadLines(@"ATMLogger.txt").Last()).Contains("EventHandler_Log_SeparationEventTest");

            Assert.IsFalse(fileConsistOurString);
        }

        [Test]
        public void EventHandler_Logging_TrackEnteredAirspaceEvent()
        {
            File.WriteAllText(@"ATMLogger.txt", "Cleared");
            var trackEnteredAirspace = new TrackEnteredAirspace();
            var atmEventHandler = new ATMEventHandler(new List<ATMWarning> { _separation }, new List<ATMNotification> { trackEnteredAirspace, _trackLeftAirspace });
            atmEventHandler.Handle(new List<IATMTransponderData> {new ATMTransponderData("EventHandler_Log_TrackEnteredAirspaceEventTest",17650, 29874, 5000, "2015")});

            var fileConsistOurString = (File.ReadLines(@"ATMLogger.txt").Last()).Contains("EventHandler_Log_TrackEnteredAirspaceEventTest");

            Assert.IsTrue(fileConsistOurString);
        }

        [Test]
        public void EventHandler_NotLogging_TrackEnteredAirspaceEventNotRaised()
        {
            File.WriteAllText(@"ATMLogger.txt", "Cleared");
            var trackEnteredAirspace = new TrackEnteredAirspace();
            var atmEventHandler = new ATMEventHandler(new List<ATMWarning> { _separation }, new List<ATMNotification> { trackEnteredAirspace, _trackLeftAirspace });
            atmEventHandler.Handle(new List<IATMTransponderData>());

            var fileConsistOurString = (File.ReadLines(@"ATMLogger.txt").Last()).Contains("EventHandler_Log_TrackEnteredAirspaceEventTest");

            Assert.False(fileConsistOurString);
        }

        [Test]
        public void EventHandler_Logging_TrackLeftAirspaceEvent()
        {
            File.WriteAllText(@"ATMLogger.txt", "Cleared");
            var trackLeftAirspace = new TrackLeftAirspace();
            var atmEventHandler = new ATMEventHandler(new List<ATMWarning> { _separation }, new List<ATMNotification> { _trackEnteredAirspace, trackLeftAirspace });
            atmEventHandler.Handle(new List<IATMTransponderData> { new ATMTransponderData("EventHandler_Log_TrackLeftAirspaceEventTest", 17650, 29874, 5000, "2015") });
            atmEventHandler.Handle(new List<IATMTransponderData> ());

            var fileConsistOurString = (File.ReadLines(@"ATMLogger.txt").Last()).Contains("EventHandler_Log_TrackLeftAirspaceEventTest");

            Assert.IsTrue(fileConsistOurString);
        }

        [Test]
        public void EventHandler_NotLogging_TrackLeftAirspaceEventNotRaised()
        {
            File.WriteAllText(@"ATMLogger.txt", "Cleared");
            var trackLeftAirspace = new TrackLeftAirspace();
            var atmEventHandler = new ATMEventHandler(new List<ATMWarning> { _separation }, new List<ATMNotification> { _trackEnteredAirspace, trackLeftAirspace });
            atmEventHandler.Handle(new List<IATMTransponderData> { new ATMTransponderData("EventHandler_Log_TrackLeftAirspaceEventTest", 17650, 29874, 5000, "2015") });
            atmEventHandler.Handle(new List<IATMTransponderData> { new ATMTransponderData("EventHandler_Log_TrackLeftAirspaceEventTest", 17650, 29874, 5900, "2015") });

            var fileConsistOurString = (File.ReadLines(@"ATMLogger.txt").Last()).Contains("EventHandler_Log_TrackLeftAirspaceEventTest");

            Assert.False(fileConsistOurString);
        }

        [Test]
        public void EventHandler_Logging_SeparationNotActiveRaisedWithNotActive()
        {
            File.WriteAllText(@"ATMLogger.txt", "Cleared");
            var separation = new Separation();
            var atmEventHandler = new ATMEventHandler(new List<ATMWarning> { separation }, new List<ATMNotification> { _trackEnteredAirspace, _trackLeftAirspace });
            atmEventHandler.Handle(new List<IATMTransponderData> {new ATMTransponderData("F12",17650, 29874, 5000, "2015"),
                new ATMTransponderData("F15",17650, 29874, 5070, "2015")});
            atmEventHandler.Handle(new List<IATMTransponderData> {new ATMTransponderData("F12",17650, 29874, 5000, "2015"),
                new ATMTransponderData("F15",17150, 29274, 5700, "2015")});

            var fileConsistOurString = (File.ReadLines(@"ATMLogger.txt").Last()).Contains("Deactivated");

            Assert.True(fileConsistOurString);

        }

        [Test]
        public void EventHandler_NotLogging_SeparationRaisedWithNotActiveNotCalled()
        {
            File.WriteAllText(@"ATMLogger.txt", "Cleared");
            var separation = new Separation();
            var atmEventHandler = new ATMEventHandler(new List<ATMWarning> { separation }, new List<ATMNotification> { _trackEnteredAirspace, _trackLeftAirspace });
            atmEventHandler.Handle(new List<IATMTransponderData> {new ATMTransponderData("F12",17650, 29874, 5000, "2015"),
                new ATMTransponderData("F15",17150, 29274, 5070, "2015")});
            atmEventHandler.Handle(new List<IATMTransponderData> {new ATMTransponderData("F12",17650, 29874, 5000, "2015"),
                new ATMTransponderData("F15",17150, 29274, 5200, "2015")});

            var fileConsistOurString = (File.ReadLines(@"ATMLogger.txt").Last()).Contains("Deactivated");

            Assert.False(fileConsistOurString);
        }
    }
}