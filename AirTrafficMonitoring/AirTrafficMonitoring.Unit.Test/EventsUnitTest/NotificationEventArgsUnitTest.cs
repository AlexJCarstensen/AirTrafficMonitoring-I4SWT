using System;
using System.Threading;
using ATMModel.Events;
using NUnit.Framework;

namespace AirTrafficMonitoring.Unit.Test.EventsUnitTest
{
    [TestFixture]
    public class NotificationEventArgsUnitTest
    {
        private NotificationEventArgs _notificationEventArgs;
        
        [SetUp]
        public void Setup()
        {
            _notificationEventArgs = new NotificationEventArgs("F12", "testEvent","20156789",  100);
            
        }

        [Test][Category("Slow Tests")]
        public void StopMeEvent_NotCalled_Before100milliSecond()
        {
            AutoResetEvent eventRaised = new AutoResetEvent(false);
            _notificationEventArgs.StopMeEvent += (sender, eventArgs) => { eventRaised.Set(); };
            Assert.IsFalse(eventRaised.WaitOne(TimeSpan.FromMilliseconds(1)));
        }

        [Test][Category("Slow Tests")]
        public void StopMeEvent_IsCalled_After100milliSecond()
        {
            AutoResetEvent eventRaised = new AutoResetEvent(false);
            _notificationEventArgs.StopMeEvent += (sender, eventArgs) =>
            {
                if(((NotificationEventArgs)sender).Timestamp == "20156789")
                    eventRaised.Set();
            };
            Assert.IsTrue(eventRaised.WaitOne(TimeSpan.FromMilliseconds(300)));
        }
    }
}