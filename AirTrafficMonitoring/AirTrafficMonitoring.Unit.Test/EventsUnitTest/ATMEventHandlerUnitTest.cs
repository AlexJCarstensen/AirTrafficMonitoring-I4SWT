using System.Collections.Generic;
using ATMModel.Data;
using ATMModel.Events;
using NSubstitute;
using NUnit.Framework;

namespace AirTrafficMonitoring.Unit.Test.EventsUnitTest
{
    [TestFixture]
    public class ATMEventHandlerUnitTest
    {
        private ATMEventHandler _uut;
        private ATMWarning _atmWarning;
        private ATMNotification _atmNotification;
        private List<IATMTransponderData> _oldAtmTransponderDatas;

        [SetUp]
        public void Setup()
        {
            _atmWarning = Substitute.For<ATMWarning>();
            _atmNotification = Substitute.For<ATMNotification>();
            _uut = new ATMEventHandler(new List<ATMWarning> {_atmWarning}, new List<ATMNotification> {_atmNotification});
            _oldAtmTransponderDatas = new List<IATMTransponderData> {Substitute.For<IATMTransponderData>()};
            _uut.Handle(_oldAtmTransponderDatas);
        }

        [Test]
        public void Handle_CallsATMWarningDetectWarning_withNewData()
        {
            var locList = new List<IATMTransponderData>
            {
                Substitute.For<IATMTransponderData>(),
                Substitute.For<IATMTransponderData>(),
                Substitute.For<IATMTransponderData>()
            };
            _uut.Handle(locList);
            _atmWarning.Received(1).DetectWarning(locList);
        }

        [Test]
        public void Handle_CallsATMNotificationDetectNotification_WithOldAndNewList()
        {
            var locList = new List<IATMTransponderData>
            {
                Substitute.For<IATMTransponderData>(),
                Substitute.For<IATMTransponderData>(),
                Substitute.For<IATMTransponderData>()
            };
            _uut.Handle(locList);
            _atmNotification.Received(1).DetectNotification(_oldAtmTransponderDatas, locList);
        }
    }
}