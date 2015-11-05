using ATMModel.Data;
using NSubstitute;
using NUnit.Framework;

namespace AirTrafficMonitoring.Unit.Test.DataUnitTest
{
    [TestFixture]
    public class ATMTransponderDataUnitTest
    {
        private ATMTransponderData _uut;
        private IATMCoordinate _atmCoordinate;

        [SetUp]
        public void Setup()
        {
            _atmCoordinate = Substitute.For<IATMCoordinate>();
            _uut = new ATMTransponderData("F22", _atmCoordinate, "20150512145712542", 25, 54);
        }

        [Test]
        public void Tag_get_ReturnF22()
        {
            _uut = new ATMTransponderData("F22", 3,4,5, "20150512145712542", 25, 54);
            Assert.That(_uut.Tag, Is.EqualTo("F22"));
        }

        [Test]
        public void Coordinate_X_get_Return2()
        {
            _atmCoordinate.X.Returns(2);
            Assert.That(_uut.Coordinate.X, Is.EqualTo(2));
        }
        [Test]
        public void Coordinate_Y_get_Return3()
        {
            _atmCoordinate.Y.Returns(3);
            Assert.That(_uut.Coordinate.Y, Is.EqualTo(3));
        }
        [Test]
        public void Coordinate_Z_get_Return4()
        {
            _atmCoordinate.Z.Returns(4);
            Assert.That(_uut.Coordinate.Z, Is.EqualTo(4));
        }
        [Test]
        public void TimeStamp_get_Return20150512145712542()
        {
            Assert.That(_uut.Timestamp, Is.EqualTo("20150512145712542"));
        }
        [Test]
        public void CompassCourse_get_Return54()
        {
            Assert.That(_uut.CompassCourse, Is.EqualTo(54));
        }
        [Test]
        public void HorizontalVelocity_get_Return25()
        {
            Assert.That(_uut.HorizontalVelocity, Is.EqualTo(25));
        }

    }
}