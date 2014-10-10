using System;
using int32.Utils.Core.Aggregator;
using int32.Utils.Core.Extensions;
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

        [TestCase]
        public void Aggregator_Singleton()
        {
            var a = Aggregator.Instance;
            var b = Aggregator.Instance;

            Assert.AreEqual(a, b);

            //since its the same singleton, this should work
            a.Subscribe<SampleAddEvent>(x => Assert.AreEqual(3, x.Data.As<int>()));
            b.Publish(new SampleAddEvent() { Data = 3 });
        }
    }
}
