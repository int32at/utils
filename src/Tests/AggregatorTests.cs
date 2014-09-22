using System;
using int32.Utils.Aggregator;
using NUnit.Framework;
using Tests.Samples;

namespace Tests
{
    [TestFixture]
    public class AggregatorTests
    {
        [TestCase]
        public void Aggregator_CreateObject()
        {
            var aggregator = new Aggregator();
            Assert.IsNotNull(aggregator);
        }

        [TestCase]
        public void Aggregator_Add_Subscription()
        {
            var aggregator = new Aggregator();
            aggregator.Subscribe<SampleAddEvent>(@event => { });
            Assert.IsTrue(aggregator.IsSubscribed<SampleAddEvent>());
        }

        [TestCase]
        public void Aggregator_Add_SubscriptionAndPublish()
        {
            var aggregator = new Aggregator();
            aggregator.Subscribe<SampleAddEvent>(e => Assert.AreEqual(3, Convert.ToInt32(e.Data)));
            aggregator.Publish(new SampleAddEvent { Data = 3 });
        }

        [TestCase]
        public void Aggregator_NullChecks()
        {
            var aggregator = new Aggregator();
            Assert.Throws<ArgumentNullException>(() => aggregator.Subscribe<SampleAddEvent>(null));
        }
    }
}
