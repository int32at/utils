using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using int32.Utils.Core.Extensions;
using int32.Utils.Windows.Files;
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

            const string expected = "{\"Title\":\"Cool Object\",\"Age\":23,\"Type\":\"Sample\"}";
            Assert.AreEqual(expected, x.ToJSON());

            var json = expected.FromJSON<SampleModel>();
            Assert.AreEqual(x.Title, json.Title);
        }

        [TestCase]
        public void Object_As_Converter()
        {
            Assert.AreEqual(3, "3".As<int>());

            var e = new SampleAddEvent() {Data = new SampleAddEvent {Data = 3}};
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
            var numbers = new[] {1, 2, 3, 4, 5};
            Assert.IsTrue(3.In(numbers));
            Assert.IsTrue(new[] {3, 5}.In(numbers));
            Assert.IsFalse(new[] {7, 1}.In(numbers));
        }

        [TestCase]
        public void Generic_Between_List()
        {
            Assert.IsTrue(3.Between(1, 4));
            Assert.IsFalse(3.Between(1, 2));
        }

        [TestCase]
        public void Generic_ForEach()
        {
            var numbers = new[] {1, 3, 5, 7, 9};
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
            var t = new SampleModel() {Age = 23, Title = "Andreas", Type = ModelType.Example};

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

        [TestCase]
        public void Generic_RemoveFromList_Predicate()
        {
            var numbers = new List<int> {1, 3, 5, 7, 9};
            numbers.Remove(i => i == 7);

            Assert.IsFalse(numbers.Contains(7));
        }

        [TestCase]
        public void Generic_RemoveFromList_Predicate_Class()
        {
            var models = new List<SampleModel>
            {
                new SampleModel() {Age = 23},
                new SampleModel() {Age = 12},
                new SampleModel() {Age = 33}
            };

            models.Remove(i => i.Age < 20);

            Assert.IsNull(models.FirstOrDefault(i => i.Age < 20));
        }


        [TestCase]
        public void Generic_UpdateList_Predicate_Class()
        {
            var models = new List<SampleModel>
            {
                new SampleModel() {Age = 23},
                new SampleModel() {Age = 12},
                new SampleModel() {Age = 33}
            };

            Assert.Throws<ArgumentNullException>(() => models.FirstOrDefault(i => i.Age == 34).Safe());

            models.ForEach(i => Assert.AreEqual(ModelType.Sample, i.Type));

            var x = GetModel().ThrowIfNull("model").And().IfNotNull(model => model.Age = 23);

            var y = GetModelNull().IfNull(model => new SampleModel() {Age = 17});

            Assert.AreEqual(x.Age, 23);
            Assert.AreEqual(y.Age, 17);
        }

        [TestCase]
        public void Generic_IfNull_IfNotNull_Chaining()
        {
            //just check if not null
            Assert.AreEqual(22, GetModel().IfNotNull(() => Assert.IsTrue(true)).Age);

            //reset the age
            Assert.AreEqual(27, GetModel().IfNotNull(() => new SampleModel() {Age = 27}).Age);

            //check for null
            Assert.IsNull(GetModelNull().IfNull(() => Assert.IsTrue(true)));

            //create new object when null
            Assert.AreEqual(27, GetModelNull().IfNull(() => new SampleModel() {Age = 27}).Age);

            //create ne wobject and check directly
            GetModelNull().IfNull(() => new SampleModel()).And().IfNotNull(model => Assert.IsNotNull(model));

            //get valid object, set it to null, check for null, and throw if null
            Assert.Throws<ArgumentNullException>(
                () =>
                    GetModel()
                        .IfNotNull(() => null)
                        .And()
                        .IfNull(model => Assert.IsNull(model))
                        .And()
                        .ThrowIfNull("model"));
        }

        private SampleModel GetModel()
        {
            return new SampleModel() {Age = 22};
        }

        private SampleModel GetModelNull()
        {
            return null;
        }

        [TestCase]
        public void Generic_And_ListAdd()
        {
            var models = new List<SampleModel>()
                .And(l => l.Add(new SampleModel()))
                .And(l => l.Add(new SampleModel()));

            Assert.AreEqual(2, models.Count);
        }

        [TestCase]
        public void Generic_Object_MemberName()
        {
            var model = new SampleModel();
            Assert.AreEqual("Age", model.MemberName(i => i.Age));
        }

        [TestCase]
        public void Generic_String_IsNullOrEmpty()
        {
            Assert.IsTrue("".IsNullOrEmpty());
            Assert.IsTrue(string.Empty.IsNullOrEmpty());
            Assert.IsFalse("test".IsNullOrEmpty());
        }

        [TestCase]
        public void Generic_string_Format()
        {
            Assert.AreEqual("Hello World", "Hello {0}".With("World"));
        }

        [TestCase]
        public void Generic_Link_Finder_Resolve()
        {
            var links = Finder.GetLinks(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu));

            Assert.Greater(links.Count(), 0);
            Assert.AreEqual(".lnk", Path.GetExtension(links.First().Path));
            Assert.AreEqual(".exe", Path.GetExtension(links.First().Resolve()));

            //use the extension method to resolve all at once, which is a cheaper call
            //then resolving every single link seperatly in a foreach
            var exes = links.ResolveAll();

            Assert.AreEqual(links.Count(), exes.Count());

            CustomAssert.Each(exes, s => Path.GetExtension(s) != ".lnk");
        }

        [TestCase]
        public void Generic_File_Extension()
        {
            var file = new FileInfo(@"requirement.json");

            //creates file in the same directory called new.json
            var newCfg = file.Copy("new", true);

            Assert.AreEqual("new.json", newCfg.Name);

            newCfg.Delete();
        }

        [TestCase]
        public void Generic_Environment_SpecialFolders()
        {
            var startup = Environment.SpecialFolder.StartMenu.ToDirectoryInfo();
            
            Assert.That(startup.FullName.EndsWith("Start Menu"));
        }
    }

    public class CustomAssert : NUnit.Framework.Assert
    {
        public static void Each<T>(IEnumerable<T> items, Func<T, bool> predicate)
        {
            foreach (var item in items)
            {
                IsTrue(predicate(item));
            }
        }
    }
}
