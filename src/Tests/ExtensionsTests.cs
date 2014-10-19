using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using int32.Utils.Core.Extensions;
using int32.Utils.Tests;
using int32.Utils.Windows.Files;
using NUnit.Framework;
using Tests.Core;
using Tests.Samples;

namespace Tests
{
    [TestFixture]
    public class ExtensionsTests : BaseTest
    {
        [TestCase]
        public void Object_ThrowIfNull()
        {
            object o = null;
            MakeSure.That(() => o.ThrowIfNull("o")).Throws<ArgumentNullException>();
        }

        [TestCase]
        public void Object_JsonConverters_Simple()
        {
            MakeSure.That(3.ToJSON()).Is("3");
            MakeSure.That("3".FromJSON<int>()).Is(3);
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
            MakeSure.That(x.ToJSON()).Is(expected);

            var json = expected.FromJSON<SampleModel>();
            MakeSure.That(json.Title).Is(x.Title);
        }

        [TestCase]
        public void Object_As_Converter()
        {
            MakeSure.That("3".As<int>()).Is(3);

            var e = new SampleAddEvent() { Data = new SampleAddEvent { Data = 3 } };

            MakeSure.That(e.As<string>()).Is("Tests.Samples.SampleAddEvent");
            MakeSure.That(e.Data.As<SampleAddEvent>().Data.As<int>()).Is(3);
        }

        [TestCase]
        public void Object_Is_Comparisons()
        {
            MakeSure.That(3.Is<int>()).Is(true);

            var e = new SampleAddEvent();
            MakeSure.That(e.Is<SampleAddEvent>()).Is(true);
            e.Data.IfNull(() => MakeSure.That(true).Is(true));
        }

        [TestCase]
        public void Date_Yesterday_Tomorrow()
        {
            var today = DateTime.Now.Date;
            MakeSure.That(today.Yesterday()).Is(today.AddDays(-1));
            MakeSure.That(today.Tomorrow()).Is(today.AddDays(1));
        }

        [TestCase]
        public void Generics_In_List()
        {
            var numbers = new[] { 1, 2, 3, 4, 5 };

            MakeSure.That(3.In(numbers)).Is(true);
            MakeSure.That(new[] { 3, 5 }.In(numbers)).Is(true);
            MakeSure.That(new[] { 7, 1 }.In(numbers)).Is(false);
        }

        [TestCase]
        public void Generic_Between_List()
        {
            MakeSure.That(3.Between(1, 4)).Is(true);
            MakeSure.That(3.Between(1, 3)).Is(false);
        }

        [TestCase]
        public void Generic_ForEach()
        {
            var numbers = new[] { 1, 3, 5, 7, 9 };
            numbers.ForEach(i => MakeSure.That(i.Between(1, 10)).Is(true));
        }

        [TestCase]
        public void String_IsMatch_Regex()
        {
            const string sampleText = "sample 33 xxx 322";

            MakeSure.That(sampleText.Matches(@"\d+")).Is(true);

            sampleText.Matches(@"\d+", () => MakeSure.That(true).Is(true));
        }

        [TestCase]
        public void Reflection_Object_SetProperty()
        {
            var model = new SampleModel() { Age = 23, Title = "Andreas", Type = ModelType.Example };

            model.Set("Age", 3);
            model.Set("Title", "Example");
            model.Set("Internal", "test");
            model.Set("_test", "b");
            model.Set("Type", ModelType.Test);

            MakeSure.That(model.Age).Is(3);
            MakeSure.That(model.Title).Is("Example");
            MakeSure.That(model.Get<string>("Internal")).Is("test");
            MakeSure.That(model.Get<string>("_test")).Is("b");
            MakeSure.That(model.Type).Is(ModelType.Test);
        }

        [TestCase]
        public void Generic_RemoveFromList_Predicate()
        {
            var numbers = new List<int> { 1, 3, 5, 7, 9 };
            numbers.Remove(i => i == 7);
            MakeSure.That(numbers.Contains(7)).Is(false);
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

            MakeSure.That(models.FirstOrDefault(i => i.Age < 20)).Is(null);
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

            MakeSure.That(() => models.FirstOrDefault(i => i.Age == 34).Safe()).Throws<ArgumentNullException>();

            models.ForEach(i => MakeSure.That(i.Type).Is(ModelType.Sample));

            var x = GetModel().ThrowIfNull("model").And().IfNotNull(model => model.Age = 23);

            var y = GetModelNull().IfNull(model => new SampleModel() { Age = 17 });

            MakeSure.That(x.Age).Is(23);
            MakeSure.That(y.Age).Is(17);
        }

        [TestCase]
        public void Generic_IfNull_IfNotNull_Chaining()
        {
            //just check if not null
            MakeSure.That(GetModel().IfNotNull(() => Assert.IsTrue(true)).Age).Is(22);

            //reset the age
            MakeSure.That(GetModel().IfNotNull(() => new SampleModel() { Age = 27 }).Age).Is(27);

            //check for null
            MakeSure.That(GetModelNull().IfNull(() => MakeSure.That(true).Is(true))).Is(null);

            //create new object when null
            MakeSure.That(GetModelNull().IfNull(() => new SampleModel() { Age = 27 }).Age).Is(27);

            //create ne wobject and check directly
            GetModelNull().IfNull(() => new SampleModel()).And().IfNotNull(model => MakeSure.That(model).IsNot(null));

            MakeSure.That(() =>
                GetModel()
                    .IfNotNull(() => null)
                    .And()
                    .IfNull(model => Assert.IsNull(model))
                    .And()
                    .ThrowIfNull("model"))
                .Throws<ArgumentNullException>();
        }

        [TestCase]
        public void Generic_And_ListAdd()
        {
            var models = new List<SampleModel>()
                .And(l => l.Add(new SampleModel()))
                .And(l => l.Add(new SampleModel()));

            MakeSure.That(models.Count).Is(2);
        }

        [TestCase]
        public void Generic_Object_MemberName()
        {
            var model = new SampleModel();
            MakeSure.That(model.MemberName(i => i.Age)).Is("Age");
        }

        [TestCase]
        public void Generic_String_IsNullOrEmpty()
        {
            MakeSure.That("".IsNullOrEmpty()).Is(true);
            MakeSure.That(string.Empty.IsNullOrEmpty()).Is(true);
            MakeSure.That("test".IsNullOrEmpty()).Is(false);
        }

        [TestCase]
        public void Generic_string_Format()
        {
            MakeSure.That("Hello {0}".With("World")).Is("Hello World");
        }

        [TestCase]
        public void Generic_Link_Finder_Resolve()
        {
            var links = Finder.GetLinks(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu));

            MakeSure.That(links.Count()).IsGreaterThan(0);
            MakeSure.That(Path.GetExtension(links.First().Path)).Is(".lnk");
            MakeSure.That(Path.GetExtension(links.First().Resolve())).Is(".exe");

            //use the extension method to resolve all at once, which is a cheaper call
            //then resolving every single link seperatly in a foreach
            var exes = links.ResolveAll();

            MakeSure.That(exes.Count()).Is(links.Count());
        }

        [TestCase]
        public void Generic_File_Extension()
        {
            var file = new FileInfo(@"requirement.json");

            //creates file in the same directory called new.json
            var newCfg = file.Copy("new", true);

            Assert.AreEqual("new.json", newCfg.Name);
            MakeSure.That(newCfg.Name).Is("new.json");

            newCfg.Delete();
        }

        [TestCase]
        public void Generic_Delegate_Extensions()
        {
            var counter = 0;

            //execute it 5 times...
            new Action(() => counter++).Execute(5).Times();

            MakeSure.That(counter).Is(5);
        }

        private SampleModel GetModel()
        {
            return new SampleModel() { Age = 22 };
        }

        private SampleModel GetModelNull()
        {
            return null;
        }
    }
}