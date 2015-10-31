using ATMModel;
using NUnit.Framework;

namespace AirTrafficMonitoring.Unit.Test
{
    [TestFixture]
    public class ATMTransponderDataUnitTest
    {
        private ATMTransponderData _uut;

        [SetUp]
        public void Setup()
        {
            _uut = new ATMTransponderData("F22", 2, 3, 4, "20150512145712542", 25, 54);
        }

        [Test]
        public void Tag_returnTrue()
        {
            Assert.That(_uut.Tag, Is.EqualTo("F22"));
        }
    }
}