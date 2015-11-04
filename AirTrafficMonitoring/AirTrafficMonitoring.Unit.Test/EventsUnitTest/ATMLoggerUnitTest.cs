using System.IO;
using System.Linq;
using ATMModel.Events;
using NUnit.Framework;

namespace AirTrafficMonitoring.Unit.Test.EventsUnitTest
{
    [TestFixture]
    public class ATMLoggerUnitTest
    {
        private IATMLogEvent _uut;

        [SetUp]
        public void Setup()
        {
            _uut = new ATMLogger();
        }

        [Test]
        public void Log_CheckFileExists_ReturnTrue()
        {
            if (File.Exists("ATMLogger.txt"))
                File.Delete("ATMLogger.txt");
            _uut.Log("hello");
            var fileExists = (File.Exists("ATMLogger.txt"));
            Assert.That(fileExists, Is.EqualTo(true));
        }

        [Test]
        public void Log_CheckTextAppend_ReturnHello()
        {
            File.WriteAllText("ATMLogger.txt", string.Empty);
            _uut.Log("Hello");
            Assert.That((File.ReadLines("ATMLogger.txt")).Last().Contains("Hello"), Is.True);
        }
    }
}