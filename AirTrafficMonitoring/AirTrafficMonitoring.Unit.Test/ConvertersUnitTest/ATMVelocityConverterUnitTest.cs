using ATMModel.Converters;
using ATMModel.Data;
using NUnit.Framework;

namespace AirTrafficMonitoring.Unit.Test.ConvertersUnitTest
{
    [TestFixture]
    public class ATMVelocityConverterUnitTest
    {
        private IATMVelocityConverter _uut;

        [SetUp]
        public void Setup()
        {
            _uut = new ATMVelocityConverter();
        }

        [Test]
        public void ATMVelocityConverter_Convert_NorthWestToSouthEast()
        {
            var velocity = _uut.Convert(new ATMCoordinate(6078, 4564, 0), new ATMCoordinate(6968, 3600, 0), "211287021526419", "211287021531623");
            Assert.That(velocity, Is.EqualTo(252.12));
        }

        [Test]
        public void ATMVelocityConverter_Convert_SouthWestToNorthEast()
        {
            var velocity = _uut.Convert(new ATMCoordinate(1582, 1744, 0), new ATMCoordinate(12503, 4999, 0), "12876", "45332");
            Assert.That(velocity, Is.EqualTo(351.11));
        }

        [Test]
        public void ATMVelocityConverter_Convert_EastToWest()
        {
            var velocity = _uut.Convert(new ATMCoordinate(8000, 6000, 0), new ATMCoordinate(4657, 6000, 0), "36923", "57490");
            Assert.That(velocity, Is.EqualTo(162.54));
        }

        [Test]
        public void ATMVelocityConverter_Convert_SouthToNorth()
        {
            var velocity = _uut.Convert(new ATMCoordinate(6724, 3971, 0), new ATMCoordinate(6724, 8530, 0), "06923", "29923");
            Assert.That(velocity, Is.EqualTo(198.22));
        }
    }
}