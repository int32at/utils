using System;
using int32.Utils.Tests;
using NUnit.Framework;

namespace Tests.Core
{
    public abstract class BaseTest
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            MakeSure.Setup(
                Assert.AreEqual,
                Assert.AreNotEqual,
                (a, b) => Assert.Less((IComparable)a, (IComparable)b),
                 (a, b) => Assert.Greater((IComparable)a, (IComparable)b)
                );
        }
    }
}
