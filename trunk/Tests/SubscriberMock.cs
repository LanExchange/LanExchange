using System;
using LanExchange.Model;

namespace Tests
{
    class SubscriberMock : ISubscriber
    {
        public bool IsEventFired;
        public DataChangedEventArgs e = new DataChangedEventArgs();

        public SubscriberMock()
        {

        }

        public void DataChanged(ISubscription sender, DataChangedEventArgs e)
        {
            IsEventFired = true;
            this.e = e;
        }
    }
}
