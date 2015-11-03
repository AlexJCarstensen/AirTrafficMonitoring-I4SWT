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
        IATMDataConverter _atmDataConverter;
        private IATMEventHandler _atmEventHandler;

        [SetUp]
        public void Setup()
        {
            _fakeTransponderDataSource = Substitute.For<ITransponderReceiver>();
            _uut = new ATMDataDecoder(_fakeTransponderDataSource, _atmDataConverter = Substitute.For<IATMDataConverter>(), _atmEventHandler = Substitute.For<IATMEventHandler>());

            _atmDataConverter.Convert(Arg.Any<List<string>>())
                .Returns(new List<IATMTransponderData> { new ATMTransponderData { Tag = "F12" } });
        }

        [Test]
        public void OnTransponderDataReady_EmptyList_EventNotInvoked()
        {
            List<IATMTransponderData> list = null;
            _uut._event += (sender, datas) => list = datas.ToList();

            _fakeTransponderDataSource.TransponderDataReady += Raise.Event<TransponderDataReadyHandler>(new List<string>());
            Assert.That(list, Is.Null);
        }

        [Test]
        public void OnTransponderDataReady_EmptyList_ConvertDataNotCalled()
        {
            _fakeTransponderDataSource.TransponderDataReady += Raise.Event<TransponderDataReadyHandler>(new List<string>());

            _atmDataConverter.Received(0).Convert(Arg.Any<List<string>>());
        }

        [Test]
        public void OnTransponderDataReady_EmptyList_EventHandlerDataNotCalled()
        {
            _fakeTransponderDataSource.TransponderDataReady += Raise.Event<TransponderDataReadyHandler>(new List<string>());

            _atmEventHandler.Received(0).Handle(Arg.Any<List<IATMTransponderData>>());
        }

        [Test]
        public void OnTransponderDataReady_NotEmptyList_EventInvoked()
        {
            List<IATMTransponderData> list = null;
            _uut._event += (sender, datas) => list = datas.ToList();

            _fakeTransponderDataSource.TransponderDataReady += Raise.Event<TransponderDataReadyHandler>(new List<string> {""});
            Assert.That(list, Is.Not.Null);
        }

        [Test]
        public void OnTransponderDataReady_NotEmptyList_ConvertCalled()
        {
            var item = new List<string> {"F12"};
            _fakeTransponderDataSource.TransponderDataReady += Raise.Event<TransponderDataReadyHandler>(item);
            _atmDataConverter.Received(1).Convert(item);
        }

        [Test]
        public void OnTransponderDataReady_NotEmptyList_EventHandlerCalled()
        {
            List<IATMTransponderData> list = null;
            _uut._event += (sender, datas) => list = datas.ToList();

            _fakeTransponderDataSource.TransponderDataReady += Raise.Event<TransponderDataReadyHandler>(new List<string> { "F12" });
            
            _atmEventHandler.Received(1).Handle(Arg.Any<List<IATMTransponderData>>());
        }

        [Test]
        public void OnTransponderDataReady_NotEmptyList_EventInvokedWithDataReturnedFromDataConverter()
        {
            List<IATMTransponderData> list = null;
            _uut._event += (sender, datas) => list = datas.ToList();

            _fakeTransponderDataSource.TransponderDataReady += Raise.Event<TransponderDataReadyHandler>(new List<string> { "" });
            Assert.That(list.ElementAt(0).Tag, Is.EqualTo("F12"));
        }

        //[Test]
        //public void TransponderDataReady_NoData_NodataCreated()
        //{
        //    List<IATMTransponderData> list = null;
        //    _uut._event += (sender, datas) => list = datas.ToList();

        //    _fakeTransponderDataSource.TransponderDataReady += Raise.Event<TransponderDataReadyHandler>(new List<string>());

        //    Assert.That(list?.Count, Is.EqualTo(0));
        //}

        //[Test]
        //public void TransponderDataReady_OneData_OnedataCreated()
        //{
        //    List<IATMTransponderData> list = null;
        //    _uut._event += (sender, datas) => list = datas.ToList();

        //    _fakeTransponderDataSource.TransponderDataReady += Raise.Event<TransponderDataReadyHandler>(new List<string> { "F12;87083;23432;5000;20151012134322345" });

        //    Assert.That(list?.Count, Is.EqualTo(1));
        //}

        //[Test]
        //public void TransponderDataReady_FiveDataTwoInvalid_ThreedataCreated()
        //{
        //    List<IATMTransponderData> list = null;
        //    _uut._event += (sender, datas) => list = datas.ToList();

        //    _fakeTransponderDataSource.TransponderDataReady += Raise.Event<TransponderDataReadyHandler>(new List<string>
        //    {
        //        "F12;87083;23432;5000;20151012134322345",
        //        "F13;87083;23432;5000;20151012134322345",
        //        "F14;87083;23432;5000;20151012134322345",
        //        "F15;87083;23432;499;20151012134322345",
        //        "F16;83;23432;5000;20151012134322345"
        //    });

        //    Assert.That(list?.Count, Is.EqualTo(3));
        //}

        //[Test]
        //public void TransponderDataReady_NoTrackEntered_NoTrackEnteredEvent()
        //{
        //    NotificationEventArgs e = null;
        //    ATMNotification.NotificationEvent += (sender, args) => { e = args; };

        //    _fakeTransponderDataSource.TransponderDataReady += Raise.Event<TransponderDataReadyHandler>(new List<string>
        //    {
        //        "F15;87083;23432;499;20151012134322345",
        //        "F16;83;23432;5000;20151012134322345"
        //    });

        //    Assert.That(e, Is.Null);
        //}

        //[Test]
        //public void TransponderDataReady_OneTrackEntered_OneTrackEnteredEvent()
        //{
        //    NotificationEventArgs e = null;
        //    ATMNotification.NotificationEvent += (sender, args) => { e = args; };

        //    _fakeTransponderDataSource.TransponderDataReady += Raise.Event<TransponderDataReadyHandler>(new List<string>
        //    {
        //        "F15;87083;23432;4999;20151012134322345",
        //        "F16;83;23432;5000;20151012134322345"
        //    });

        //    Assert.That(e?.Tag, Is.EqualTo("F15"));
        //}
    }
}