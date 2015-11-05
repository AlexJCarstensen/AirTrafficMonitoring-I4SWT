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

        [SetUp]
        public void Setup()
        {
            _atmDataConverter = new ATMDataConverter(_atmAngleConverter = new ATMAngleConverter(), Substitute.For<IATMVelocityConverter>());
            _atmDataConverter.Convert(new List<string> {"F12;20453;46569;15203;201505111104253"});
        }
        
        [Test]
        public void ConvertAngle_CheckNorth_return0()
        {
            var item = _atmDataConverter.Convert(new List<string> {"F12;20453;56569;15203;201505111104253"});
            Assert.That(item.ElementAt(0).CompassCourse, Is.EqualTo(0));
        }

        [Test]
        public void ConvertAngle_CheckSouth_return180()
        {
            var item = _atmDataConverter.Convert(new List<string> { "F12;20453;36569;15203;201505111104253" });
            Assert.That(item.ElementAt(0).CompassCourse, Is.EqualTo(180));
        }

        [Test]
        public void ConvertAngle_CheckWest_return90()
        {
            var item = _atmDataConverter.Convert(new List<string> { "F12;10453;46569;15203;201505111104253" });
            Assert.That(item.ElementAt(0).CompassCourse, Is.EqualTo(90));
        }

        [Test]
        public void ConvertAngle_CheckEast_return270()
        {
            var item = _atmDataConverter.Convert(new List<string> { "F12;40453;46569;15203;201505111104253" });
            Assert.That(item.ElementAt(0).CompassCourse, Is.EqualTo(270));
        }

        [Test]
        public void ConvertAngle_CheckNorthEast_return315()
        {
            var item = _atmDataConverter.Convert(new List<string> { "F12;50152;76268;15203;201505111104253" });
            Assert.That(item.ElementAt(0).CompassCourse, Is.EqualTo(315));
        }
    }
    //
}