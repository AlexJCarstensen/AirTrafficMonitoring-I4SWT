using System.Collections.Generic;
using System.IO;
using System.Linq;
using ATMModel.Data;
using ATMModel.Events;
using NUnit.Framework;

namespace AirTrafficMonitoring.Integration.Test.EventsIntegrationTest
{
    [TestFixture]
    public class ATMIntegrationTestSeven
    {
        private Separation _separation;
        private TrackEnteredAirspace _trackEnteredAirspace;
        private TrackLeftAirspace _trackLeftAirspace;
        private IATMLogEvent _atmLogEvent;

        [SetUp]
        public void Setup()
        {
            _atmLogEvent = new ATMLogger();
            _separation = new Separation(_atmLogEvent);
            _trackEnteredAirspace = new TrackEnteredAirspace(_atmLogEvent);
            _trackLeftAirspace = new TrackLeftAirspace(_atmLogEvent);
            
        }

        [Test]
        public void Log_separationEvent_returnTrue()
        {
            File.WriteAllText(@"ATMLogger.txt", string.Empty);
            _separation.DetectWarning(new List<IATMTransponderData> { new ATMTransponderData("E15", new ATMCoordinate(10987, 10987, 1024), "20151105134923729"), new ATMTransponderData("E16", new ATMCoordinate(11987, 14987, 1200), "20151105134923729") });
            var textFromFile =
                (File.ReadLines("ATMLogger.txt").Last().Contains("20151105134923729 Separation Warning E16 E15 Activated"));
            Assert.IsTrue(textFromFile);
        }

        [Test]
        public void Log_TrackEnteredEvent_returnTrue()
        {
            File.WriteAllText(@"ATMLogger.txt", string.Empty);
            _trackEnteredAirspace.DetectNotification(new List<IATMTransponderData> (), new List<IATMTransponderData> {new ATMTransponderData("E15", new ATMCoordinate(11987, 14525, 5400), "2015110513492729") } );
            var textFromFile =
                (File.ReadLines("ATMLogger.txt").Last().Contains("2015110513492729 TrackEnteredAirspace Notification E15"));
            Assert.IsTrue(textFromFile);
        }
        [Test]
        public void Log_TrackLeftEvent_returnTrue()
        {
            File.WriteAllText(@"ATMLogger.txt", string.Empty);
            _trackLeftAirspace.DetectNotification(new List<IATMTransponderData> { new ATMTransponderData("E15", new ATMCoordinate(11987, 14525, 5400), "2015110513492729") }, new List<IATMTransponderData>() );
            var textFromFile =
                (File.ReadLines("ATMLogger.txt").Last().Contains("2015110513492729 TrackLeftAirspace Notification E15"));
            Assert.IsTrue(textFromFile);
        }

        [Test]
        public void Log_separationEventNotLogged_returnTrue()
        {
            File.WriteAllText(@"ATMLogger.txt", "Cleared");
            _separation.DetectWarning(new List<IATMTransponderData> { new ATMTransponderData("E15", new ATMCoordinate(10987, 10987, 654), "20151105134923729"), new ATMTransponderData("E16", new ATMCoordinate(11987, 14987, 1200), "20151105134923729") });
            var textFromFile =
                (File.ReadLines("ATMLogger.txt").Last().Contains("Cleared"));
            Assert.IsTrue(textFromFile);
        }

        [Test]
        public void Log_TrackEnteredEventEventNotLogged_returnTrue()
        {
            File.WriteAllText(@"ATMLogger.txt", "Cleared");
            _separation.DetectWarning(new List<IATMTransponderData> { new ATMTransponderData("E15", new ATMCoordinate(10987, 10987, 450), "20151105134923729"), new ATMTransponderData("E16", new ATMCoordinate(11987, 14987, 489), "20151105134923729") });
            var textFromFile =
                (File.ReadLines("ATMLogger.txt").Last().Contains("Cleared"));
            Assert.IsTrue(textFromFile);
        }

        [Test]
        public void Log_TrackLeftEventEventNotLogged_returnTrue()
        {
            File.WriteAllText(@"ATMLogger.txt", "Cleared");
            _separation.DetectWarning(new List<IATMTransponderData> { new ATMTransponderData("E15", new ATMCoordinate(10987, 10987, 5043), "20151105134923729"), new ATMTransponderData("E16", new ATMCoordinate(11987, 14987, 7856), "20151105134923729") });
            var textFromFile =
                (File.ReadLines("ATMLogger.txt").Last().Contains("Cleared"));
            Assert.IsTrue(textFromFile);
        }
    }
}