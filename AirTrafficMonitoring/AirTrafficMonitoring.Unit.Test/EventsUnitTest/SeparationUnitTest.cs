using System.Collections.Generic;
using ATMModel.Data;
using ATMModel.Events;
using NSubstitute;
using NUnit.Framework;

namespace AirTrafficMonitoring.Unit.Test.EventsUnitTest
{
    [TestFixture]
    public class SeparationUnitTest
    {
        private Separation _uut;
        private IATMLogEvent _atmLogEvent;
        private IATMTransponderData data1;
        private IATMTransponderData data2;

        [SetUp]
        public void Setup()
        {
            _uut = new Separation(_atmLogEvent = Substitute.For<IATMLogEvent>());
            data1 = Substitute.For<IATMTransponderData>();
            data1.Tag = "F12";
            data1.Timestamp = "20121103190132619";
            var coordinate = Substitute.For<IATMCoordinate>();
            coordinate.X = 84;
            coordinate.Y = 23;
            coordinate.Z = 45;
            data1.Coordinate = coordinate;
            data2 = Substitute.For<IATMTransponderData>();
            data2.Tag = "F15";
            data2.Timestamp = "20121103190132621";
        }

        [Test]
        public void DetectWarning_NoData_NoWarningEvent()
        {
            int warningEventCalled = 0;
            ATMWarning.WarningEvent += (sender, args) => warningEventCalled ++;
            _uut.DetectWarning(new List<IATMTransponderData>());

            Assert.That(warningEventCalled, Is.EqualTo(0));
        }

        [Test]
        public void DetectWarning_TwoFlightNoSeparation_NoEvent()
        {
            var locCoordinate = Substitute.For<IATMCoordinate>();
            locCoordinate.X = 8400;
            locCoordinate.Y = 2300;
            locCoordinate.Z = 4500;
            data2.Coordinate = locCoordinate;

            int warningEventCalled = 0;
            ATMWarning.WarningEvent += (sender, args) => warningEventCalled++;
            _uut.DetectWarning(new List<IATMTransponderData> {data1, data2});

            Assert.That(warningEventCalled, Is.EqualTo(0));
        }

        [Test]
        public void DetectWarning_TwoFlightNoSeparation_NoLogging()
        {
            var locCoordinate = Substitute.For<IATMCoordinate>();
            locCoordinate.X = 8400;
            locCoordinate.Y = 2300;
            locCoordinate.Z = 4500;
            data2.Coordinate = locCoordinate;

            int warningEventCalled = 0;
            ATMWarning.WarningEvent += (sender, args) => warningEventCalled++;
            _uut.DetectWarning(new List<IATMTransponderData> {data1, data2});

            _atmLogEvent.Received(0).Log(Arg.Any<string>());
        }

        [Test]
        public void DetectWarning_TwoFlightSeparation_EventRaisedOnce()
        {
            var locCoordinate = Substitute.For<IATMCoordinate>();
            locCoordinate.X = 840;
            locCoordinate.Y = 230;
            locCoordinate.Z = 344;
            data2.Coordinate = locCoordinate;

            int warningEventCalled = 0;
            ATMWarning.WarningEvent += (sender, args) => warningEventCalled++;
            _uut.DetectWarning(new List<IATMTransponderData> {data1, data2});

            Assert.That(warningEventCalled, Is.EqualTo(1));
        }

        [Test]
        public void DetectWarning_TwoFlightSeparation_EventRaisedOncePropertyActivatedIsTrue()
        {
            var locCoordinate = Substitute.For<IATMCoordinate>();
            locCoordinate.X = 840;
            locCoordinate.Y = 230;
            locCoordinate.Z = 344;
            data2.Coordinate = locCoordinate;

            bool activePropertyIsTrue = false;
            ATMWarning.WarningEvent += (sender, args) => { if (args.Active) activePropertyIsTrue = true; };
            _uut.DetectWarning(new List<IATMTransponderData> { data1, data2 });

            Assert.That(activePropertyIsTrue, Is.True);
        }

        [Test]
        public void DetectWarning_TwoFlightSeparation_EventLogged()
        {
            var locCoordinate = Substitute.For<IATMCoordinate>();
            locCoordinate.X = 840;
            locCoordinate.Y = 230;
            locCoordinate.Z = 344;
            data2.Coordinate = locCoordinate;
            _uut.DetectWarning(new List<IATMTransponderData> { data1, data2 });

            _atmLogEvent.Received(1).Log("20121103190132621 Separation Warning F15 F12 Activated");
        }

        [Test]
        public void DetectWarning_TwoFlightSeparation_EventRaisedAndNotRaisedSecondTime()
        {
            var locCoordinate = Substitute.For<IATMCoordinate>();
            locCoordinate.X = 840;
            locCoordinate.Y = 230;
            locCoordinate.Z = 344;
            data2.Coordinate = locCoordinate;

            int warningEventCalled = 0;
            ATMWarning.WarningEvent += (sender, args) => warningEventCalled++;
            _uut.DetectWarning(new List<IATMTransponderData> { data1, data2 });

            locCoordinate.Z = 341;
            _uut.DetectWarning(new List<IATMTransponderData> { data1, data2 });

            Assert.That(warningEventCalled, Is.EqualTo(1));
        }

        [Test]
        public void DetectWarning_TwoFlightSeparation_NoMoreSeparationDetecting()
        {
            var locCoordinate = Substitute.For<IATMCoordinate>();
            locCoordinate.X = 840;
            locCoordinate.Y = 230;
            locCoordinate.Z = 344;
            data2.Coordinate = locCoordinate;

            var activeIsFalse = true;
            ATMWarning.WarningEvent += (sender, args) => { if (!args.Active) activeIsFalse = false; };
            _uut.DetectWarning(new List<IATMTransponderData> { data1, data2 });

            locCoordinate.Z = 341;
            _uut.DetectWarning(new List<IATMTransponderData> { data1, data2 });

            locCoordinate.Z = 345;
            _uut.DetectWarning(new List<IATMTransponderData> { data1, data2 });

            Assert.IsFalse(activeIsFalse);
        }
    }
}