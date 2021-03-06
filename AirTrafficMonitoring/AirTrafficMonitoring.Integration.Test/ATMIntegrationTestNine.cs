﻿using System.Collections.Generic;
using System.Linq;
using ATMModel;
using ATMModel.Converters;
using ATMModel.Data;
using ATMModel.Events;
using NSubstitute;
using NUnit.Framework;
using TransponderReceiver;

namespace AirTrafficMonitoring.Integration.Test
{
    [TestFixture]
    public class ATMIntegrationTestNine
    {
        private IATMDataDecoder _atmDataDecoder;
        private ITransponderReceiver _transponderReceiver;

        [SetUp]
        public void Setup()
        {
            _transponderReceiver = Substitute.For<ITransponderReceiver>();

            _atmDataDecoder = new ATMDataDecoder(_transponderReceiver,
                 new ATMDataConverter(new ATMAngleConverter(), new ATMVelocityConverter()),
                new ATMEventHandler());
        }

        [Test]
        public void OnTransponderDataReady_EmptyList_EventNotInvoked()
        {
            List<IATMTransponderData> localList = null;
            _atmDataDecoder._event += (sender, datas) => localList = datas.ToList();
            _transponderReceiver.TransponderDataReady += Raise.Event<TransponderDataReadyHandler>(new List<string>());
            Assert.That(localList, Is.Null);

        }

        [Test]
        public void OnTransponderDataReady_NotEmptyList_EventInvoked()
        {
            var eventInvoked = false;
            _atmDataDecoder._event += (sender, datas) => eventInvoked = true;
            _transponderReceiver.TransponderDataReady += Raise.Event<TransponderDataReadyHandler>(new List<string> { "F12;14642;13606;5600;20151012134322345" });
            Assert.IsTrue(eventInvoked);
        }
        [Test]
        public void OnTransponderDataReady_3ValidItemsList_EventInvokedWith3Items()
        {
            List <IATMTransponderData> list = null;
            _atmDataDecoder._event += (sender, datas) => list = datas.ToList();


            _transponderReceiver.TransponderDataReady +=
                Raise.Event<TransponderDataReadyHandler>(new List<string>
                {
                    "F12;14642;13606;5600;20151012134322345",
                    "F13;87083;23432;5000;20151012134322345",
                    "F14;87083;23432;5000;20151012134322345"
                });
            Assert.That(list?.Count, Is.EqualTo(3));
        }
        [Test]
        public void OnTransponderDataReady_5Items2InvalidList_EventInvokedWith3Items()
        {
            List<IATMTransponderData> list = null;
            _atmDataDecoder._event += (sender, datas) => list = datas.ToList();


            _transponderReceiver.TransponderDataReady +=
                Raise.Event<TransponderDataReadyHandler>(new List<string>
                {
                    "F12;14642;13606;5600;20151012134322345",
                    "F13;87083;23432;5000;20151012134322345",
                    "F14;87083;23432;5000;20151012134322345",
                    "F15;92000;23432;5000;20151012134322345",
                    "F16;87083;23432;450;20151012134322345"
                });
            Assert.That(list?.Count, Is.EqualTo(3));
        }

        [Test]
        public void Convert_ValidString_returnItem()
        {
            var recievedItemIsValid = false;
            _atmDataDecoder._event += (sender, datas) =>
            {
                if (datas.ToList().Count > 0 && datas.ToList().ElementAt(0).Tag == "FHT3V")
                    recievedItemIsValid = true;
            };
            _transponderReceiver.TransponderDataReady +=
                Raise.Event<TransponderDataReadyHandler>(new List<string> { "FHT3V;14642;13606;5600;20151012134322345"});

            Assert.IsTrue(recievedItemIsValid);
        }

        [Test]
        public void Convert_InvalidString_returnEmptyList()
        {
            var recievedItemIsValid = false;
            _atmDataDecoder._event += (sender, datas) =>
            {
                if (datas.ToList().Count > 0)
                    recievedItemIsValid = true;
            };
            _transponderReceiver.TransponderDataReady +=
                Raise.Event<TransponderDataReadyHandler>(new List<string> { "FHT3V;14642;13606;499;20151012134322345" });

            Assert.IsFalse(recievedItemIsValid);
        }

        [Test]
        public void EventHandler_OneTrackEntered_TrackEnteredEventRaised()
        {
            var recievedItemIsValid = false;
            ATMNotification.NotificationEvent += (sender, args) =>
            {
                if (args.EventName == "TrackEnteredAirspace" && args.Tag == "FHT3V")
                    recievedItemIsValid = true;
            };

            _transponderReceiver.TransponderDataReady +=
                Raise.Event<TransponderDataReadyHandler>(new List<string> { "FHT3V;14642;13606;4999;20151012134322345" });

            Assert.IsTrue(recievedItemIsValid);
        }

        [Test]
        public void EventHandler_OneTrackLeft_TrackLeftEventRaised()
        {
            var recievedItemIsValid = false;
            ATMNotification.NotificationEvent += (sender, args) =>
            {
                if (args.EventName == "TrackLeftAirspace" && args.Tag == "FHT4V")
                    recievedItemIsValid = true;
            };

            _transponderReceiver.TransponderDataReady +=
                Raise.Event<TransponderDataReadyHandler>(new List<string> { "FHT4V;14642;13606;4999;20151012134322345" });
            _transponderReceiver.TransponderDataReady +=
                Raise.Event<TransponderDataReadyHandler>(new List<string> { "FHT4V;14642;13606;499;20151012134322345" });

            Assert.IsTrue(recievedItemIsValid);
        }

        [Test]
        public void EventHandler_OneSeparation_SeparationEventRaised()
        {
            var recievedItemIsValid = false;
            ATMWarning.WarningEvent += (sender, args) =>
            {
                if (args.EventName == "Separation" && args.Tag1 == "FHT5S" && args.Tag2 == "FHT4V" && args.Active)
                    recievedItemIsValid = true;
            };

            _transponderReceiver.TransponderDataReady +=
                Raise.Event<TransponderDataReadyHandler>(new List<string>
                {
                    "FHT4V;14642;13606;4999;20151012134322345",
                    "FHT5S;14642;13606;4799;20151012134322345"
                });

            Assert.IsTrue(recievedItemIsValid);
        }

        [Test]
        public void EventHandler_OneSeparationfromActivToNotActive_SeparationEventRaised()
        {
            var recievedItemIsValid = false;
            ATMWarning.WarningEvent += (sender, args) =>
            {
                if (args.EventName == "Separation" && args.Tag1 == "FHT5S" && args.Tag2 == "FHT4V" && !args.Active)
                    recievedItemIsValid = true;
            };

            _transponderReceiver.TransponderDataReady +=
                Raise.Event<TransponderDataReadyHandler>(new List<string>
                {
                    "FHT4V;14642;13606;4999;20151012134322345",
                    "FHT5S;14642;13606;4799;20151012134322345"
                });
            _transponderReceiver.TransponderDataReady +=
                Raise.Event<TransponderDataReadyHandler>(new List<string>
                {
                    "FHT4V;14642;13606;4999;20151012134322345",
                    "FHT5S;14642;13606;4599;20151012134322345"
                });

            Assert.IsTrue(recievedItemIsValid);
        }

    }
}