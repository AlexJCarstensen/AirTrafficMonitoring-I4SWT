using ATMModel;
using NUnit.Framework;

namespace AirTrafficMonitoring.Unit.Test
{
    [TestFixture]
    public class ATMVelocityUnitTest
    {
        private IATMVelocityConverter _uut;
        private IATMCoordinate _atmCoordinate;
        private IATMTransponderData _atmTransponderData;

        [SetUp]
        public void Setup()
        {
            _uut = new ATMVelocityConverter();
            _atmCoordinate = new ATMCoordinate(15765, 55425, 15453);
            _atmTransponderData = new ATMTransponderData { Timestamp = "20150512145712542" };
        }

        [Test]
        public void Convert_newCoordinates_OneSecond_Return246_68()
        {
            var newTestCoordinate = new ATMCoordinate(16000, 55500, 15453);
            var newTime = new ATMTransponderData { Timestamp = "20150512145713542" };
            var velocity = _uut.Convert(_atmCoordinate, newTestCoordinate, _atmTransponderData, newTime);
            Assert.That(velocity, Is.EqualTo(246.68));
        }

        
    }
}