using System;
using System.Runtime.InteropServices;
using System.Threading;
using ATMModel.Events;
using NUnit.Framework;

namespace AirTrafficMonitoring.Unit.Test.EventsUnitTest
{
    [TestFixture]
    public class NotificationEventArgsUnitTest
    {
        private NotificationEventArgs _notificationEventArgs;
        private AutoResetEvent EventRaised;
        
        [SetUp]
        public void Setup()
        {
            _notificationEventArgs = new NotificationEventArgs("F12", "testEvent", 100);
            EventRaised = new AutoResetEvent(false);
            _notificationEventArgs.StopMeEvent += (sender, eventArgs) => { EventRaised.Set(); };
        }

        [Test]
        public void StopMeEvent_NotCalled_Before100milliSecond()
        {
            Assert.IsFalse(EventRaised.WaitOne(99));
        }

        [Test]
        public void StopMeEvent_IsCalled_After100milliSecond()
        {
            Assert.IsTrue(EventRaised.WaitOne(150));
        }
    }
}