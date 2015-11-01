using System.Collections.Generic;
using System.Linq;
using ATMModel;
using NUnit.Framework;

namespace AirTrafficMonitoring.Unit.Test
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

            _uut = new ATMDataConverter(_atmAngleConverter = new ATMAngleConverter(), _atmVelocityConverter = new ATMVelocityConverter());
            _list = new List<string>();
            _atmTransponderDataList = new List<IATMTransponderData>();
            _list.Add("F12;87083;23432;5000;20151012134322345");
            _list.Add("AB34;88083;24432;4321;20151012134323345");
            _list.Add("ABKH2;89083;25432;3423;20151012134324345");
            _atmTransponderDataList = _uut.Convert(_list);
        }
        [Test]
        public void Convert_Add3TransponderData_return3()
        {
            Assert.That(_atmTransponderDataList.Count, Is.EqualTo(3));
        }

        [Test]
        public void ATMDataConverter_Convert_CheckVelocity()
        {
            var locList = new List<string> {"AB34;89083;25432;4321;20151012134326345"};
            _atmTransponderDataList = _uut.Convert(locList);
            Assert.That(_atmTransponderDataList.ElementAt(0).HorizontalVelocity, Is.EqualTo(471));
        }

        [Test]
        public void ATMDataConverter_Convert_CheckAngle()
        {
            var locList = new List<string> {"AB34;89083;25432;4321;20151012134326345"};
            _atmTransponderDataList = _uut.Convert(locList);
            Assert.That(_atmTransponderDataList.ElementAt(0).CompassCourse, Is.EqualTo(315));
        }
    }
}