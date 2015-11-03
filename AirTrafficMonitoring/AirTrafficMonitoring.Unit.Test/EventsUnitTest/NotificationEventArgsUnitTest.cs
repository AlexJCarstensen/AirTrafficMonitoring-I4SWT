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
            _notificationEventArgs = new NotificationEventArgs("F12", "testEvent");
            EventRaised = new AutoResetEvent(false);
            _notificationEventArgs.StopMeEvent += (sender, eventArgs) => { EventRaised.Set(); };
        }

        [Test]
        public void StopMeEvent_NotCalled_Before10Second()
        {
            Assert.IsFalse(EventRaised.WaitOne(TimeSpan.FromSeconds(1)));
        }

        [Test]
        public void StopMeEvent_IsCalled_After10Second()
        {
            Assert.IsTrue(EventRaised.WaitOne(TimeSpan.FromSeconds(11)));
        }
    }
}