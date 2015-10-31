using System.Collections.Generic;
using ATMModel;
using NUnit.Framework;

namespace AirTrafficMonitoring.Unit.Test
{
    [TestFixture]
    public class ATMDataConverterUnitTest
    {
        private IATMDataConverter _uut;
        private IATMTransponderData _atmTransponderData;
        private IATMAngleConverter _atmAngleConverter;
        private IATMVelocityConverter _atmVelocityConverter;
        private List<string> _list;
        private List<IATMTransponderData> _atmTransponderDataList;

        [SetUp]
        public void Setup()
        {

            _uut = new ATMDataConverter(_atmTransponderData = new ATMTransponderData("F22", 2, 3, 4, "20150512145712542", 25, 54),
                _atmAngleConverter = new ATMAngleConverter(),
                _atmVelocityConverter = new ATMVelocityConverter());
            _list = new List<string>();
            _atmTransponderDataList = new List<IATMTransponderData>();
            _list.Add("F12;234;324;5000;20151012134322345");
            _list.Add("AB34;758;243;4321;20151012134322345");
            _list.Add("ABKH2;99083;324432;32423;20151012134322345");
            _atmTransponderDataList = _uut.Convert(_list);
        }

        [Test]
        public void Convert_something()
        {
            Assert.That(_atmTransponderDataList.Count, Is.EqualTo(3));
        }

    }
}