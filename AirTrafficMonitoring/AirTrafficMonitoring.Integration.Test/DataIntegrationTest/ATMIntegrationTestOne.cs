using ATMModel.Data;
using NUnit.Framework;

namespace AirTrafficMonitoring.Integration.Test.DataIntegrationTest
{
    [TestFixture]
    public class ATMIntegrationTestOne
    {
        private ATMTransponderData _atmTransponderData;
        
        [SetUp]
        public void Setup()
        {
            _atmTransponderData = new ATMTransponderData("F12", new ATMCoordinate(64251, 89857, 19857), "201505111104253" );
        }

        [Test]
        public void GetCoordinateX_From_ATMCoordinate_return64251()
        {
            Assert.That(_atmTransponderData.Coordinate.X, Is.EqualTo(64251));
        }

        [Test]
        public void GetCoordinateY_From_ATMCoordinate_return89857()
        {
            Assert.That(_atmTransponderData.Coordinate.Y, Is.EqualTo(89857));
        }

        [Test]
        public void GetCoordinateZ_From_ATMCoordinate_return19857()
        {
            Assert.That(_atmTransponderData.Coordinate.Z, Is.EqualTo(19857));
        }

        [Test]
        public void ATMCoordinate_Validate_ReturnTrue()
        {
            Assert.That(_atmTransponderData.Coordinate.Validate, Is.EqualTo(true));
        }

        [Test]
        public void ATMCoordinate_Validate_AllInvalidLow_ReturnFalse()
        {
            _atmTransponderData = new ATMTransponderData {Coordinate = new ATMCoordinate(5000, 5000, 4999)};
            Assert.That(_atmTransponderData.Coordinate.Validate, Is.EqualTo(false));
        }

        [Test]
        public void ATMCoordinate_Validate_AllInvalidHigh_ReturnFalse()
        {
            _atmTransponderData = new ATMTransponderData { Coordinate = new ATMCoordinate(92312, 95643, 25643) };
            Assert.That(_atmTransponderData.Coordinate.Validate, Is.EqualTo(false));
        }

        [Test]
        public void ATMCoordinate_Validate_XInvalidLow_ReturnFalse()
        {
            _atmTransponderData = new ATMTransponderData { Coordinate = new ATMCoordinate(5000, 45325, 18645) };
            Assert.That(_atmTransponderData.Coordinate.Validate, Is.EqualTo(false));
        }

        [Test]
        public void ATMCoordinate_Validate_XInvalidHigh_ReturnFalse()
        {
            _atmTransponderData = new ATMTransponderData { Coordinate = new ATMCoordinate(95684, 45325, 18645) };
            Assert.That(_atmTransponderData.Coordinate.Validate, Is.EqualTo(false));
        }

        [Test]
        public void ATMCoordinate_Validate_YInvalidLow_ReturnFalse()
        {
            _atmTransponderData = new ATMTransponderData { Coordinate = new ATMCoordinate(45325, 5000, 18645) };
            Assert.That(_atmTransponderData.Coordinate.Validate, Is.EqualTo(false));
        }

        [Test]
        public void ATMCoordinate_Validate_YInvalidHigh_ReturnFalse()
        {
            _atmTransponderData = new ATMTransponderData { Coordinate = new ATMCoordinate(45325, 120356, 18645) };
            Assert.That(_atmTransponderData.Coordinate.Validate, Is.EqualTo(false));
        }

        [Test]
        public void ATMCoordinate_Validate_ZInvalidLow_ReturnFalse()
        {
            _atmTransponderData = new ATMTransponderData { Coordinate = new ATMCoordinate(45325, 45325, 123) };
            Assert.That(_atmTransponderData.Coordinate.Validate, Is.EqualTo(false));
        }

        [Test]
        public void ATMCoordinate_Validate_ZInvalidHigh_ReturnFalse()
        {
            _atmTransponderData = new ATMTransponderData { Coordinate = new ATMCoordinate(45325, 45325, 22000) };
            Assert.That(_atmTransponderData.Coordinate.Validate, Is.EqualTo(false));
        }
    }
}