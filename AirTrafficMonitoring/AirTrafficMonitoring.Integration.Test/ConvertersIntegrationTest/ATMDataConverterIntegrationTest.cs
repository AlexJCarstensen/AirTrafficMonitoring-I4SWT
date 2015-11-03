using System.Collections.Generic;
using System.Linq;
using ATMModel.Converters;
using ATMModel.Data;
using NUnit.Framework;

namespace AirTrafficMonitoring.Integration.Test.ConvertersIntegrationTest
{
    [TestFixture]
    public class ATMDataConverterIntegrationTest
    {
        private IATMDataConverter _atmDataConverter;
        private IATMAngleConverter _atmAngleConverter;
        private IATMVelocityConverter _atmVelocityConverter;

        private List<string> _list;
        private List<IATMTransponderData> _atmTransponderDataList;
        [SetUp]
        public void Setup()
        {
            _atmDataConverter = new ATMDataConverter(_atmAngleConverter = new ATMAngleConverter(), _atmVelocityConverter = new ATMVelocityConverter());
            _list = new List<string>();
            _atmTransponderDataList = new List<IATMTransponderData>();
            _list.Add("F12;14642;13606;5600;20151012134322345");
            _list.Add("AB34;88083;24432;4321;20151012134323345");
            _list.Add("ABKH2;89083;25432;3423;20151012134324345");
            _atmTransponderDataList = _atmDataConverter.Convert(_list);
        }
        
         [Test]
         public void ATMDataConverter_Convert_CheckVelocity()
         {
             var locList = new List<string> {"AB34;89083;25432;4321;20151012134326345"};
             _atmTransponderDataList = _atmDataConverter.Convert(locList);
             Assert.That(_atmTransponderDataList.ElementAt(0).HorizontalVelocity, Is.EqualTo(471));
         }

         [Test]
         public void ATMDataConverter_Convert_CheckAngle()
         {
             var locList = new List<string> {"F12;15864;14314;4321;20151012134326345"};
             _atmTransponderDataList = _atmDataConverter.Convert(locList);
             Assert.That(_atmTransponderDataList.ElementAt(0).CompassCourse, Is.EqualTo(300));
         }

        [Test]
        public void ATMDataConverter_Convert_CheckTag()
        {
            var locList = new List<string> { "F12;15864;14314;4321;20151012134326345" };
            _atmTransponderDataList = _atmDataConverter.Convert(locList);
            Assert.That(_atmTransponderDataList.ElementAt(0).Tag, Is.EqualTo("F12"));
        }

        [Test]
        public void ATMDataConverter_Convert_CheckTimeStamp()
        {
            var locList = new List<string> { "F12;15864;14314;4321;20151012134326345" };
            _atmTransponderDataList = _atmDataConverter.Convert(locList);
            Assert.That(_atmTransponderDataList.ElementAt(0).Timestamp, Is.EqualTo("20151012134326345"));
        }

        [Test]
        public void ATMDataConverter_Convert_CheckCoordinateX()
        {
            var locList = new List<string> { "F12;15864;14314;4321;20151012134326345" };
            _atmTransponderDataList = _atmDataConverter.Convert(locList);
            Assert.That(_atmTransponderDataList.ElementAt(0).Coordinate.X, Is.EqualTo(15864));
        }

        [Test]
        public void ATMDataConverter_Convert_CheckCoordinateY()
        {
            var locList = new List<string> { "F12;15864;14314;4321;20151012134326345" };
            _atmTransponderDataList = _atmDataConverter.Convert(locList);
            Assert.That(_atmTransponderDataList.ElementAt(0).Coordinate.Y, Is.EqualTo(14314));
        }

        [Test]
        public void ATMDataConverter_Convert_CheckCoordinateZ()
        {
            var locList = new List<string> { "F12;15864;14314;4321;20151012134326345" };
            _atmTransponderDataList = _atmDataConverter.Convert(locList);
            Assert.That(_atmTransponderDataList.ElementAt(0).Coordinate.Z, Is.EqualTo(4321));
        }

        [Test]
        public void ATMDataConverter_Convert_ExcludeInvalidX()
        {
            var locList = new List<string>
            {
                "F12;15864;14314;4321;20151012134326345",
                "F15;158;14314;4321;20151012134326345",
                "F13;15864;19314;4321;20151012134326345"
            };
            _atmTransponderDataList = _atmDataConverter.Convert(locList);
            Assert.That(_atmTransponderDataList.Count, Is.EqualTo(2));
        }

        [Test]
        public void ATMDataConverter_Convert_ExcludeInvalidY()
        {
            var locList = new List<string>
            {
                "F12;15864;1414;4321;20151012134326345",
                "F15;15928;1314;4321;20151012134326345",
                "F13;15864;19314;4321;20151012134326345"
            };
            _atmTransponderDataList = _atmDataConverter.Convert(locList);
            Assert.That(_atmTransponderDataList.Count, Is.EqualTo(1));
        }

        [Test]
        public void ATMDataConverter_Convert_ExcludeInvalidZ()
        {
            var locList = new List<string>
            {
                "F12;15864;15414;41;20151012134326345",
                "F15;15928;13014;43;20151012134326345",
                "F13;15864;19314;21;20151012134326345"
            };
            _atmTransponderDataList = _atmDataConverter.Convert(locList);
            Assert.That(_atmTransponderDataList.Count, Is.EqualTo(0));
        }
    }
}