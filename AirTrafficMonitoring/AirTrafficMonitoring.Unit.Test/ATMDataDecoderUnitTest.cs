using System.Collections.Generic;
using System.Linq;
using ATMModel;
using ATMModel.Converters;
using ATMModel.Data;
using ATMModel.Events;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using TransponderReceiver;

namespace AirTrafficMonitoring.Unit.Test
{
    [TestFixture]
    public class ATMDataDecoderUnitTest
    {
        private ITransponderReceiver _fakeTransponderDataSource;
        private IATMDataDecoder _uut;

        [SetUp]
        public void Setup()
        {
            _fakeTransponderDataSource = Substitute.For<ITransponderReceiver>();
            _uut = new ATMDataDecoder(_fakeTransponderDataSource, new ATMDataConverter(new ATMAngleConverter(), new ATMVelocityConverter()), new ATMEventHandler());
        }

        [Test]
        public void TransponderDataReady_NoData_NodataCreated()
        {
            List<IATMTransponderData> list = null;
            _uut._event += (sender, datas) => list = datas.ToList();

            _fakeTransponderDataSource.TransponderDataReady += Raise.Event<TransponderDataReadyHandler>(new List<string>());

            Assert.That(list?.Count, Is.EqualTo(0));
        }

        [Test]
        public void TransponderDataReady_OneData_OnedataCreated()
        {
            List<IATMTransponderData> list = null;
            _uut._event += (sender, datas) => list = datas.ToList();

            _fakeTransponderDataSource.TransponderDataReady += Raise.Event<TransponderDataReadyHandler>(new List<string> { "F12;87083;23432;5000;20151012134322345" });

            Assert.That(list?.Count, Is.EqualTo(1));
        }

        [Test]
        public void TransponderDataReady_FiveDataTwoInvalid_ThreedataCreated()
        {
            List<IATMTransponderData> list = null;
            _uut._event += (sender, datas) => list = datas.ToList();

            _fakeTransponderDataSource.TransponderDataReady += Raise.Event<TransponderDataReadyHandler>(new List<string>
            {
                "F12;87083;23432;5000;20151012134322345",
                "F13;87083;23432;5000;20151012134322345",
                "F14;87083;23432;5000;20151012134322345",
                "F15;87083;23432;499;20151012134322345",
                "F16;83;23432;5000;20151012134322345"
            });

            Assert.That(list?.Count, Is.EqualTo(3));
        }

        [Test]
        public void TransponderDataReady_NoTrackEntered_NoTrackEnteredEvent()
        {
            NotificationEventArgs e = null;
            ATMNotification.NotificationEvent += (sender, args) => { e = args; };

            _fakeTransponderDataSource.TransponderDataReady += Raise.Event<TransponderDataReadyHandler>(new List<string>
            {
                "F15;87083;23432;499;20151012134322345",
                "F16;83;23432;5000;20151012134322345"
            });

            Assert.That(e, Is.Null);
        }

        [Test]
        public void TransponderDataReady_OneTrackEntered_OneTrackEnteredEvent()
        {
            NotificationEventArgs e = null;
            ATMNotification.NotificationEvent += (sender, args) => { e = args; };

            _fakeTransponderDataSource.TransponderDataReady += Raise.Event<TransponderDataReadyHandler>(new List<string>
            {
                "F15;87083;23432;4999;20151012134322345",
                "F16;83;23432;5000;20151012134322345"
            });

            Assert.That(e?.Tag, Is.EqualTo("F15"));
        }
    }
}