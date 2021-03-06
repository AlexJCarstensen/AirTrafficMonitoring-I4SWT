﻿using System;
using ATMModel.Converters;
using ATMModel.Data;
using NUnit.Framework;

namespace AirTrafficMonitoring.Unit.Test.ConvertersUnitTest
{
    [TestFixture]
    public class ATMAngleConverterUnitTest
    {
        private IATMAngleConverter _uut;
        private IATMCoordinate _atmCoordinate;

        [SetUp]
        public void Setup()
        {
            _uut = new ATMAngleConverter();
            _atmCoordinate = new ATMCoordinate(522, 375, 5000);
        }

        [Test]
        public void Convert_ConvertTestNorth0_return0()
        {
            var newTestCoordinate = new ATMCoordinate(522, 4812, 15453);
            var angle = _uut.Convert(_atmCoordinate, newTestCoordinate);
            Assert.That(angle, Is.EqualTo(0));
        }
        [Test]
        public void Convert_ConvertTestSouth180_return180()
        {
            var newTestCoordinate = new ATMCoordinate(522, 145, 15453);
            var angle = _uut.Convert(_atmCoordinate, newTestCoordinate);
            Assert.That(angle, Is.EqualTo(180));
        }

        [Test]
        public void ATMAngleConverter_ConvertInFirstKvadrant_return338point69()
        {
            var angle = _uut.Convert(_atmCoordinate, new ATMCoordinate(2253, 4812, 5000));
            Assert.That(angle, Is.EqualTo(338.69));
        }

        [Test]
        public void ATMAngleConverter_ConvertInSecondKvadrant_return32point02()
        {
            var angle = _uut.Convert(_atmCoordinate, new ATMCoordinate(-2253, 4812, 5000));
            Assert.That(angle, Is.EqualTo(32.02));
        }

        [Test]
        public void ATMAngleConverter_ConvertInThirdKvadrant_return151point85()
        {
            var angle = _uut.Convert(_atmCoordinate, new ATMCoordinate(-2253, -4812, 5000));
            Assert.That(angle, Is.EqualTo(151.85));
        }

        [Test]
        public void ATMAngleConverter_ConvertInForthKvadrant_return198point45()
        {
            var angle = _uut.Convert(_atmCoordinate, new ATMCoordinate(2253, -4812, 5000));
            Assert.That(angle, Is.EqualTo(198.45));
        }

        [Test]
        public void ATMAngleConverter_Convert2NegativeCoordinates_return193point03()
        {
            var angle = _uut.Convert(new ATMCoordinate(-2975, -3408, 5624), new ATMCoordinate(-2301, -6320, 93283));
            Assert.That(angle, Is.EqualTo(193.03));
        }

        [Test]
        public void Convert_ParameterNull_ThrowsArgNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _uut.Convert(null, null));
        }

        [Test]
        public void Convert_FirstParameterNull_ThrowsArgNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _uut.Convert(null, new ATMCoordinate(12,12,23)));
        }

        [Test]
        public void Convert_SecondParameterNull_ThrowsArgNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _uut.Convert(new ATMCoordinate(12, 12, 23), null));
        }
    }
}