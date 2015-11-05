using System.Collections.Generic;
using ATMModel.Data;
using ATMModel.Events;
using NSubstitute;
using NUnit.Framework;

namespace AirTrafficMonitoring.Integration.Test.EventsIntegrationTest
{
    public class ATMIntegrationTestFive
    {
        private Separation _separation;

        [SetUp]
        public void SetUp()
        {
            _separation = new Separation(Substitute.For<IATMLogEvent>());
        }

        [Test]
        public void DetectWarning_NoData_NoWarning()
        {
            var eventTriggered = 0;
            ATMWarning.WarningEvent += (sender, args) =>
            {
                eventTriggered++;
            };

            _separation.DetectWarning(new List<IATMTransponderData>());
            Assert.That(eventTriggered, Is.EqualTo(0));
        }

        [Test]
        public void DetectWarning_HorizontalConflictVerticalNoConflict_NoWarning()
        {
            var eventTriggered = 0;
            ATMWarning.WarningEvent += (sender, args) =>
            {
                eventTriggered++;
            };

            _separation.DetectWarning(new List<IATMTransponderData> {new ATMTransponderData("E15", new ATMCoordinate(10987, 56789, 1024), "20151105134923729"), new ATMTransponderData("E16", new ATMCoordinate(10987, 56789, 1400), "20151105134923729") });
            Assert.That(eventTriggered, Is.EqualTo(0));
        }

        [Test]
        public void DetectWarning_HorizontalNoConflictVerticalConflict_NoWarning()
        {
            var eventTriggered = 0;
            ATMWarning.WarningEvent += (sender, args) =>
            {
                eventTriggered++;
            };

            _separation.DetectWarning(new List<IATMTransponderData> { new ATMTransponderData("E15", new ATMCoordinate(10987, 56789, 1024), "20151105134923729"), new ATMTransponderData("E16", new ATMCoordinate(15365, 52968, 1200), "20151105134923729") });
            Assert.That(eventTriggered, Is.EqualTo(0));
        }

        [Test]
        public void DetectWarning_HorizontalConflictVerticalConflict_OneWarning()
        {
            var eventTriggered = 0;
            ATMWarning.WarningEvent += (sender, args) =>
            {
                eventTriggered++;
            };

            _separation.DetectWarning(new List<IATMTransponderData> { new ATMTransponderData("E15", new ATMCoordinate(10987, 10987, 1024), "20151105134923729"), new ATMTransponderData("E16", new ATMCoordinate(11987, 14987, 1200), "20151105134923729") });
            Assert.That(eventTriggered, Is.EqualTo(1));
        }

        [Test]
        public void DetectWarning_Conflict_OneWarningWithActiveConflict()
        {
            var eventTriggered = false;
            ATMWarning.WarningEvent += (sender, args) =>
            {
                if (args.Active)
                    eventTriggered = true;
            };

            _separation.DetectWarning(new List<IATMTransponderData> { new ATMTransponderData("E15", new ATMCoordinate(10987, 10987, 1024), "20151105134923729"), new ATMTransponderData("E16", new ATMCoordinate(11987, 14987, 1200), "20151105134923729") });
            Assert.That(eventTriggered, Is.True);
        }

        [Test]
        public void DetectWarning_ConflictEnding_OneWarningWithNotActiveConflict()
        {
            var eventTriggered = 0;
            ATMWarning.WarningEvent += (sender, args) =>
            {
                if (!args.Active)
                    eventTriggered ++;
            };

            _separation.DetectWarning(new List<IATMTransponderData> { new ATMTransponderData("E15", new ATMCoordinate(10987, 10987, 1024), "20151105134923729"), new ATMTransponderData("E16", new ATMCoordinate(11987, 14987, 1200), "20151105134923729") });
            _separation.DetectWarning(new List<IATMTransponderData> { new ATMTransponderData("E15", new ATMCoordinate(10987, 10987, 1024), "20151105134923729"), new ATMTransponderData("E16", new ATMCoordinate(11987, 14987, 1500), "20151105134923729") });
            Assert.That(eventTriggered, Is.EqualTo(1));
        }
    }
}