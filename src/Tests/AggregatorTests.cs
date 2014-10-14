using System;
using int32.Utils.Core.Aggregator;
using int32.Utils.Core.Extensions;
using int32.Utils.Tests;
using NUnit.Framework;
using Tests.Core;
using Tests.Samples;

namespace Tests
{
    [TestFixture]
    public class AggregatorTests : BaseTest
    {
        [TestCase]
        public void Aggregator_CreateObject()
        {
            var aggregator = new Aggregator();
            MakeSure.That(aggregator).IsNot(null);
        }

        [TestCase]
        public void Aggregator_Add_Subscription()
        {
            var aggregator = new Aggregator();
            aggregator.Subscribe<SampleAddEvent>(@event => { });
            MakeSure.That(aggregator.IsSubscribed<SampleAddEvent>()).Is(true);
        }

        [TestCase]
        public void Aggregator_Add_SubscriptionAndPublish()
        {
            var aggregator = new Aggregator();
            aggregator.Subscribe<SampleAddEvent>(e => MakeSure.That(Convert.ToInt32(e.Data)).Is(3));
            aggregator.Publish(new SampleAddEvent { Data = 3 });
        }

        [TestCase]
        public void Aggregator_NullChecks()
        {
            var aggregator = new Aggregator();
            MakeSure.That(() => aggregator.Subscribe<SampleAddEvent>(null)).Throws<ArgumentNullException>();
        }

        [TestCase]
        public void Aggregator_Singleton()
        {
            var a = Aggregator.Instance;
            var b = Aggregator.Instance;

            Assert.AreEqual(a, b);

            //since its the same singleton, this should work
            a.Subscribe<SampleAddEvent>(x => MakeSure.That(x.Data.As<int>()).Is(3));
            b.Publish(new SampleAddEvent() { Data = 3 });
        }
    }
}
