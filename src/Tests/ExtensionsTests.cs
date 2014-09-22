using System;
using int32.Utils.Extensions;
using NUnit.Framework;
using Tests.Samples;

namespace Tests
{
    [TestFixture]
    public class ExtensionsTests
    {
        [TestCase]
        public void Object_ThrowIfNull()
        {
            object o = null;
            Assert.Throws<ArgumentNullException>(() => o.ThrowIfNull("o"));
        }

        [TestCase]
        public void Object_JsonConverters_Simple()
        {
            Assert.AreEqual("3", 3.ToJSON());
            Assert.AreEqual(3, "3".FromJSON<int>());
        }

        [TestCase]
        public void Object_JsonConverters_Class()
        {
            var x = new SampleModel()
            {
                Title = "Cool Object",
                Age = 23,
                Type = ModelType.Sample
            };

            const string expected = "{\"Title\":\"Cool Object\",\"Age\":23,\"Type\":0}";
            Assert.AreEqual(expected, x.ToJSON());

            var json = expected.FromJSON<SampleModel>();
            Assert.AreEqual(x.Title, json.Title);
        }

        [TestCase]
        public void Object_As_Converter()
        {
            Assert.AreEqual(3, "3".As<int>());

            var e = new SampleAddEvent() { Data = new SampleAddEvent { Data = 3 } };
            Assert.AreEqual("Tests.Samples.SampleAddEvent", e.As<string>());
            Assert.AreEqual(3, e.Data.As<SampleAddEvent>().Data.As<int>());
        }

        [TestCase]
        public void Object_Is_Comparisons()
        {
            Assert.IsTrue(3.Is<int>());

            var e = new SampleAddEvent();
            Assert.IsTrue(e.Is<SampleAddEvent>());
            e.Data.IfNull(() => Assert.IsTrue(true));
        }

        [TestCase]
        public void Date_Yesterday_Tomorrow()
        {
            var today = DateTime.Now.Date;
            Assert.AreEqual(today.AddDays(-1), today.Yesterday());
            Assert.AreEqual(today.AddDays(1), today.Tomorrow());
        }

        [TestCase]
        public void Generics_In_List()
        {
            var numbers = new[] { 1, 2, 3, 4, 5 };
            Assert.IsTrue(3.In(numbers));
            Assert.IsTrue(new[] { 3, 5 }.In(numbers));
            Assert.IsFalse(new[] { 7, 1 }.In(numbers));
        }

        [TestCase]
        public void Generics_Between_List()
        {
            Assert.IsTrue(3.Between(1, 4));
            Assert.IsFalse(3.Between(1, 2));
        }

        [TestCase]
        public void Generics_ForEach()
        {
            var numbers = new[] { 1, 3, 5, 7, 9 };
            numbers.ForEach(i => Assert.IsTrue(i.Between(1, 10)));
        }

        [TestCase]
        public void String_IsMatch_Regex()
        {
            const string sampleText = "sample 33 xxx 322";
            Assert.IsTrue(sampleText.Matches(@"\d+"));

            sampleText.Matches(@"\d+", () => Assert.IsTrue(true));
        }

        [TestCase]
        public void Reflection_Object_SetProperty()
        {
            var t = new SampleModel() { Age = 23, Title = "Andreas", Type = ModelType.Example };

            t.Set("Age", 3);
            t.Set("Title", "Example");
            t.Set("Internal", "test");
            t.Set("_test", "b");
            t.Set("Type", ModelType.Test);

            Assert.AreEqual(3, t.Age);
            Assert.AreEqual("Example", t.Title);
            Assert.AreEqual("test", t.Get<string>("Internal"));
            Assert.AreEqual("b", t.Get<string>("_test"));
            Assert.AreEqual(t.Type, ModelType.Test);
        }
    }
}
