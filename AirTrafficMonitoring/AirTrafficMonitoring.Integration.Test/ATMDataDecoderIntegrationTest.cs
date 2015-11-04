using System.Collections.Generic;
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
    public class ATMDataDecoderIntegrationTest
    {
        private IATMDataDecoder _atmDataDecoder;
        private IATMDataConverter _atmDataConverter;
        private IATMEventHandler _atmEventHandler;
        private ITransponderReceiver _transponderReceiver;
        [SetUp]
        public void Setup()
        {
            _transponderReceiver = Substitute.For<ITransponderReceiver>();

            _atmDataDecoder = new ATMDataDecoder(_transponderReceiver,
                _atmDataConverter = new ATMDataConverter(new ATMAngleConverter(), new ATMVelocityConverter()),
                new ATMEventHandler());

            var _list = new List<string>();
            _list.Add("F12;14642;13606;5600;20151012134322345");
            _list.Add("AB34;88083;24432;4321;20151012134323345");
            _list.Add("ABKH2;89083;25432;3423;20151012134324345");
            //_atmDataDecoder.OnTransponderDataReady(_list);
            
        }

        [Test]
        public void OnTransponderDataReady_EmptyList_EventNotInvoked()
        {
            List<IATMTransponderData> localList = null;
            _atmDataDecoder._event += (sender, datas) => localList = datas.ToList();
            _transponderReceiver.TransponderDataReady += Raise.Event<TransponderDataReadyHandler>(new List<string>());
            Assert.That(localList, Is.Null);

        }

    }
}