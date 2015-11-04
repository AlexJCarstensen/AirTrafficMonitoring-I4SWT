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
        private List<IATMTransponderData> _list = new List<IATMTransponderData>();
        [SetUp]
        public void Setup()
        {
            _transponderReceiver = Substitute.For<ITransponderReceiver>();

            _atmDataDecoder = new ATMDataDecoder(_transponderReceiver,
                _atmDataConverter = new ATMDataConverter(new ATMAngleConverter(), new ATMVelocityConverter()),
                new ATMEventHandler());

            //_list.Add(new ATMTransponderData("F12", new ATMCoordinate(14642, 13606,5600), "20151012134322345"));
            //_list.Add(new ATMTransponderData("AB34", new ATMCoordinate(88083, 24432,4321), "20151012134323345"));
            //_list.Add(new ATMTransponderData("ABKH2", new ATMCoordinate(89083,25432,3423), "20151012134324345"));
           
            /*_list.Add("F12;14642;13606;5600;20151012134322345");
            _list.Add("AB34;88083;24432;4321;20151012134323345");
            _list.Add("ABKH2;89083;25432;3423;20151012134324345");*/
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

    }
}