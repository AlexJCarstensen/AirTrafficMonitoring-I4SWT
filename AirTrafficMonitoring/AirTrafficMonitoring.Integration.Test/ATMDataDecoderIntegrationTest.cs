using ATMModel;
using ATMModel.Converters;
using ATMModel.Events;
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
            //_atmDataDecoder = new ATMDataDecoder(_transponderReceiver =, );
        }
    }
}