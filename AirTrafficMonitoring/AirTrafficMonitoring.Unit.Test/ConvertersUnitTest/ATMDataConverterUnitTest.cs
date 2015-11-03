using System.Collections.Generic;
using System.Linq;
using ATMModel.Converters;
using ATMModel.Data;
using NSubstitute;
using NUnit.Framework;

namespace AirTrafficMonitoring.Unit.Test.ConvertersUnitTest
{
    [TestFixture]
    public class ATMDataConverterUnitTest
    {
        private IATMDataConverter _uut;
        private IATMAngleConverter _atmAngleConverter;
        private IATMVelocityConverter _atmVelocityConverter;

        private List<string> _list;
        private List<IATMTransponderData> _atmTransponderDataList;

        [SetUp]
        public void Setup()
        {
            _uut = new ATMDataConverter(_atmAngleConverter = Substitute.For<IATMAngleConverter>(),
                _atmVelocityConverter = Substitute.For<IATMVelocityConverter>());

            _atmVelocityConverter.Convert(Arg.Any<IATMCoordinate>(), Arg.Any<IATMCoordinate>(), Arg.Any<string>(),
                Arg.Any<string>()).Returns(15);
            _atmAngleConverter.Convert(Arg.Any<IATMCoordinate>(), Arg.Any<IATMCoordinate>()).Returns(20);
           
            _list = new List<string>();
            _atmTransponderDataList = new List<IATMTransponderData>();
            _list.Add("F12;87083;23432;5000;20151012134322345");
            _list.Add("AB34;88083;24432;4321;20151012134323345");
            _list.Add("ABKH2;89083;25432;3423;20151012134324345");
        }

        [Test]
        public void Convert_Add3TransponderData_return3()
        {
            _atmTransponderDataList = _uut.Convert(_list);
            Assert.That(_atmTransponderDataList.Count, Is.EqualTo(3));
        }

        [Test]
        public void Convert_Tag_ReturnF12()
        {
            _atmTransponderDataList = _uut.Convert(_list);
            Assert.That(_atmTransponderDataList.ElementAt(0).Tag, Is.EqualTo("F12"));
        }

        [Test]
        public void Convert_CoordX_Return87083()
        {
            _atmTransponderDataList = _uut.Convert(_list);
            Assert.That(_atmTransponderDataList.ElementAt(0).Coordinate.X, Is.EqualTo(87083));
        }

        [Test]
        public void Convert_CoordY_Return24432()
        {
            _atmTransponderDataList = _uut.Convert(_list);
            Assert.That(_atmTransponderDataList.ElementAt(1).Coordinate.Y, Is.EqualTo(24432));
        }

       [Test]
        public void Convert_CoordZ_Return3423()
        {
            _atmTransponderDataList = _uut.Convert(_list);
            Assert.That(_atmTransponderDataList.ElementAt(2).Coordinate.Z, Is.EqualTo(3423));
        }

        [Test]
        public void Convert_CoordTimeStamp_Return20151012134324345()
        {
            _atmTransponderDataList = _uut.Convert(_list);
            Assert.That(_atmTransponderDataList.ElementAt(2).Timestamp, Is.EqualTo("20151012134324345"));
        }

        [Test]
        public void Convert_Velocity_ConvertReturn15()
        {
            var velocityCheck = _uut.Convert(_list);
            Assert.That(velocityCheck.ElementAt(0).HorizontalVelocity, Is.EqualTo(15));
        }

        [Test]
        public void Convert_Angle_ConvertReturn20()
        {
            var angleCheck = _uut.Convert(_list);
            Assert.That(angleCheck.ElementAt(0).CompassCourse, Is.EqualTo(20));
        }
    }
}
