using System.Collections.Generic;
using System.Linq;
using ATMModel.Converters;
using ATMModel.Data;
using NSubstitute;
using NUnit.Framework;

namespace AirTrafficMonitoring.Integration.Test.ConvertersIntegrationTest
{
    [TestFixture]
    public class ATMIntegrationTestTwo
    {
        private IATMDataConverter _atmDataConverter;
        private IATMAngleConverter _atmAngleConverter;
        private IATMTransponderData _atmTransponderData;
        private List<string> _list;
        private List<IATMTransponderData> _atmTransponderDataList;

        [SetUp]
        public void Setup()
        {
            _atmDataConverter = new ATMDataConverter(_atmAngleConverter = new ATMAngleConverter(), Substitute.For<IATMVelocityConverter>());
            _atmTransponderData = new ATMTransponderData("F12", new ATMCoordinate(20453, 46569, 15203), "201505111104253");

            _list = new List<string>
            {
                "F12;87083;23432;5000;20151012134322345",
                "AB34;88083;24432;4321;20151012134323345",
                "ABKH2;89083;25432;3423;20151012134324345"
            };
            _atmTransponderDataList = new List<IATMTransponderData>();
            
        }
        /*
        [Test]
        public void ConvertAngle_CheckNorth_return0()
        {
            _atmTransponderDataList = _atmDataConverter.Convert(_list);
            Assert.That(_atmTransponderDataList.ElementAt(0).CompassCourse, Is.EqualTo(20));
        }*/
    }
}