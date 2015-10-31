using ATMModel;
using NUnit.Framework;

namespace AirTrafficMonitoring.Unit.Test
{
    [TestFixture]
    public class ATMAngleConverterUnitTest
    {
        private IATMAngleConverter _uut;
        private IATMCoordinate _atmCoordinate;

        [SetUp]
        public void Setup()
        {
            _uut = new ATMAngleConverter();
            _atmCoordinate = new ATMCoordinate(15765, 55425, 15453);
        }

        [Test]
        public void Convert_NewCoordinates_return287_70()
        {
            var newTestCoordinate = new ATMCoordinate(16000, 55500, 15453);
            var angle = _uut.Convert(_atmCoordinate, newTestCoordinate);
            Assert.That(angle, Is.EqualTo(287.70));
        }
         
    }
}