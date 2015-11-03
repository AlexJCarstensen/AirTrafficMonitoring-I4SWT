using System;
using System.Threading;
using ATMModel.Events;
using NUnit.Framework;

namespace AirTrafficMonitoring.Unit.Test.EventsUnitTest
{
    public class WarningEventArgsUnitTest
    {
        private WarningEventArgs _warningEventArgs;
        private AutoResetEvent EventRaised;

        [SetUp]
        public void Setup()
        {
            _warningEventArgs = new WarningEventArgs("F12", "F13", "testEvent");
            EventRaised = new AutoResetEvent(false);
            _warningEventArgs.StopMeEvent += (sender, eventArgs) => { EventRaised.Set(); };
        }

        [Test]
        public void StopMeEvent_NotCalled_BeforeActiveSet()
        {
            Assert.IsFalse(EventRaised.WaitOne(100));
        }

        [Test]
        public void StopMeEvent_IsCalled_After10Second()
        {
            _warningEventArgs.Active = false;
            Assert.IsTrue(EventRaised.WaitOne(100));
        }
    }
}