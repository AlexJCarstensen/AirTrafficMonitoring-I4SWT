using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ATMModel;

namespace AirTrafficMonitoring.Unit.Test
{
    [TestFixture]
    public class ATMCoordinateUnitTest
    {
        private IATMCoordinate _uut;

        [SetUp]
        public void Setup()
        {
            _uut = new ATMCoordinate(12543, 85465, 659);
        }

        // X and Y is valid if values are between 10.000 and 90.000, Z is valid between 500 and 20.000 
        [Test]
        public void Validate_ValidX_ValidY_ValidZ_returnTrue()
        {
            Assert.That(_uut.Validate, Is.EqualTo(true));
        }

        [Test]
        public void Validate_InValidXLow_ValidY_ValidZ_returnFalse()
        {
            _uut.X = 8452;
            Assert.That(_uut.Validate, Is.EqualTo(false));
        }
        [Test]
        public void Validate_InValidXHigh_ValidY_ValidZ_returnFalse()
        {
            _uut.X = 95452;
            Assert.That(_uut.Validate, Is.EqualTo(false));
        }
        [Test]
        public void Validate_ValidX_InValidYLow_ValidZ_returnFalse()
        {
            _uut.Y = 9999;
            Assert.That(_uut.Validate, Is.EqualTo(false));
        }
        [Test]
        public void Validate_ValidX_InValidYHigh_ValidZ_returnFalse()
        {
            _uut.Y = 120352;
            Assert.That(_uut.Validate, Is.EqualTo(false));
        }
        [Test]
        public void Validate_ValidX_ValidY_InValidZLow_returnFalse()
        {
            _uut.Z = 452;
            Assert.That(_uut.Validate, Is.EqualTo(false));
        }
        [Test]
        public void Validate_ValidX_ValidY_InValidZHigh_returnFalse()
        {
            _uut.Z = 22542;
            Assert.That(_uut.Validate, Is.EqualTo(false));
        }
    }
}
