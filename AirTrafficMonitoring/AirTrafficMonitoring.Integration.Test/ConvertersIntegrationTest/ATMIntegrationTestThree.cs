using ATMModel.Converters;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace AirTrafficMonitoring.Integration.Test.ConvertersIntegrationTest
{
    [TestFixture]
    public class ATMIntegrationTestThree
    {
        private IATMDataConverter _atmDataConverter;
        

        [SetUp]
        public void Setup()
        {
            _atmDataConverter = new ATMDataConverter(Substitute.For<IATMAngleConverter>(),  new ATMVelocityConverter());
            _atmDataConverter.Convert(new List<string> { "F12;20453;46569;15203;20150511110425345" });
        }

        [Test]
        public void Velocity_Convert357m1sec_return357()
        {
            var item = _atmDataConverter.Convert(new List<string> { "F12;20710;46817;15203;20150511110426345"});
            Assert.That(item.ElementAt(0).HorizontalVelocity, Is.EqualTo(357));
        }

        [Test]
        public void Velocity_Convert357m5sec_return71()
        {
            var item = _atmDataConverter.Convert(new List<string> { "F12;20710;46817;15203;20150511110430345" });
            Assert.That(item.ElementAt(0).HorizontalVelocity, Is.EqualTo(71));
        }

        [Test]
        public void Velocity_Convert727m2sec_return363()
        {
            var item = _atmDataConverter.Convert(new List<string> { "F12;20000;46000;15203;20150511110427345" });
            Assert.That(item.ElementAt(0).HorizontalVelocity, Is.EqualTo(363));
        }

        [Test]
        public void Velocity_Convert98m345msec_return284()
        {
            var item = _atmDataConverter.Convert(new List<string> { "F12;20511;46490;15203;20150511110425690" });
            Assert.That(item.ElementAt(0).HorizontalVelocity, Is.EqualTo(284));
        }

        [Test]
        public void Velocity_Convert98m0msec_return0()
        {
            var item = _atmDataConverter.Convert(new List<string> { "F12;20511;46490;15203;20150511110425345" });
            Assert.That(item.ElementAt(0).HorizontalVelocity, Is.EqualTo(0));
        }

        [Test]
        public void Velocity_Convert98mminus1msec_return0()
        {
            var item = _atmDataConverter.Convert(new List<string> { "F12;20511;46490;15203;20150511110424345" });
            Assert.That(item.ElementAt(0).HorizontalVelocity, Is.EqualTo(0));
        }
    }
}