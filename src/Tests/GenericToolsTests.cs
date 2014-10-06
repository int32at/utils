using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using int32.Utils.Core.Extensions;
using int32.Utils.Core.Generic.Base;
using int32.Utils.Core.Generic.Collections;
using int32.Utils.Core.Generic.Factory;
using int32.Utils.Core.Generic.Repository;
using int32.Utils.Core.Generic.Singleton;
using int32.Utils.Core.Generic.Tasks;
using int32.Utils.Core.Generic.ViewModel;
using NUnit.Framework;
using Tests.Samples;

namespace Tests
{
    [TestFixture]
    public class GenericToolsTest
    {
        [TestCase]
        public void GenericTools_Singleton()
        {
            var a = Singleton<SampleModel>.Instance;
            var b = Singleton<SampleModel>.Instance;

            Assert.AreEqual(a, b);
        }

        [TestCase]
        public void GenericTools_Singleton_Class()
        {
            var a = SampleModelSingleton.Instance;
            var b = SampleModelSingleton.Instance;

            Assert.AreEqual(a, b);
        }

        [TestCase]
        public void GenericTools_Factory()
        {
            Assert.IsNotNull(SampleModelFactory.Create());
            Assert.AreEqual(17, SampleModelFactory.Create(model => model.Age = 17).Age);
        }

        [TestCase]
        public void GenericTools_Repository()
        {
            var repo = Factory<SampleModelRepository>.Create();
            Assert.AreEqual(1, repo.Get().Age);
        }

        [TestCase]
        public void GenericTools_RepositoryHandler()
        {
            var vm = RepositoryHandler.Add(new SampleModelRepository());
            Assert.AreEqual(vm, RepositoryHandler.Get<SampleModelRepository>());
        }

        [TestCase]
        public void GenericTools_RepositoryHandler_Get()
        {
            //register sample service
            RepositoryHandler.Add(new SampleModelRepository());

            var sampleModel = RepositoryHandler.Get<SampleModelRepository>().Get();

            Assert.IsNotNull(sampleModel);
            Assert.AreEqual(1, sampleModel.Age);

            var sampleModel2 = RepositoryHandler.Get<SampleModelRepository>().Get(i => i.Age > 17);
            Assert.AreEqual(22, sampleModel2.Age);

            Assert.AreEqual(3, RepositoryHandler.Get<SampleModelRepository>().Count);
        }

        [TestCase]
        public void GenericTools_RepositoryHandler_GetAll()
        {
            //register sample service
            RepositoryHandler.Add(new SampleModelRepository());

            var count = RepositoryHandler.Get<SampleModelRepository>().GetAll().Count();
            Assert.AreEqual(3, count);

            var count2 = RepositoryHandler.Get<SampleModelRepository>().GetAll(i => i.Age > 17).Count();
            Assert.AreEqual(2, count2);
        }

        [TestCase]
        public void GenericTools_RepositoryHandler_AddUpdateDelete()
        {
            //register sample service
            RepositoryHandler.Add(new SampleModelRepository());

            var item = RepositoryHandler.Get<SampleModelRepository>().Add(new SampleModel() { Age = 50 });

            Assert.AreEqual(50, item.Age);
            Assert.AreEqual(item, RepositoryHandler.Get<SampleModelRepository>().Get(i => i == item));
            Assert.AreEqual(4, RepositoryHandler.Get<SampleModelRepository>().Count);

            var newItem = RepositoryHandler.Get<SampleModelRepository>().Update(item, new SampleModel() { Age = 53 });
            Assert.AreEqual(newItem, RepositoryHandler.Get<SampleModelRepository>().Get(i => i == newItem));
            Assert.AreEqual(4, RepositoryHandler.Get<SampleModelRepository>().Count);

            RepositoryHandler.Get<SampleModelRepository>().Delete(i => i.Age > 40);
            Assert.AreEqual(3, RepositoryHandler.Get<SampleModelRepository>().Count);

            Assert.DoesNotThrow(() => RepositoryHandler.Get<SampleModelRepository>().SaveChanges());
        }

        [TestCase]
        public void GenericTools_RepositoryHandler_RegisterMultiple()
        {
            Assert.DoesNotThrow(() =>
            {
                var items = RepositoryHandler.Add(
                    new SampleModelRepository(),
                    new SampleModelRepository(),
                    new SampleModelRepository()
                    );

                Assert.AreEqual(3, items.Count());
            });


            Assert.IsNotNull(RepositoryHandler.Get<SampleModelRepository>());
        }

        [TestCase]
        public void GenericTools_Timer_Measure()
        {
            Assert.GreaterOrEqual(Timing.Measure(() => Thread.Sleep(200)).Milliseconds, 100);
        }

        [TestCase]
        public void GenericTools_ViewModel()
        {
            var vm = Factory<SampleModelViewModel>.Create();
            vm.PropertyChanged += vm_LoadChanged;
            vm.Load();
            vm.PropertyChanged -= vm_LoadChanged;

            vm.PropertyChanged += vm_TestChanged;
            vm.Test = 37;
        }

        [TestCase]
        public void GenericTools_ViewModel_Handler()
        {
            ViewModelHandler.Add(new SampleModelViewModel());
            var vm = ViewModelHandler.Get<SampleModelViewModel>();

            Assert.IsNotNull(vm);
            Assert.AreEqual(vm, ViewModelHandler.Get<SampleModelViewModel>());

            vm.PropertyChanged += vm_LoadChanged;
            vm.Load();

            Assert.IsTrue(vm.IsLoaded);
        }

        [TestCase]
        public void GenericTools_Collections_FluentList()
        {
            var list = new FluentList<SampleModel>()
                .Add(new SampleModel { Title = "Test" })
                .Add(new SampleModel { Title = "Test2" })
                .Add(new SampleModel { Title = "Test3" })
                .RemoveAt(0).Reverse().Update(i => i.Type = ModelType.Test);

            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(ModelType.Test, list.First().Type);
        }

        [TestCase]
        public void GenericTools_Collections_FluentDictionary()
        {
            var dict = new FluentDictionary<string, int>()
                .Add("Key1", 1)
                .Add("Key2", 2)
                .Add("Key3", 3)
                .Remove("Key3");

            Assert.AreEqual(2, dict.Count);
            Assert.IsTrue(dict.ContainsKey("Key2"));
            Assert.AreEqual(1, dict["Key1"]);
            Assert.AreEqual(2, dict.Keys.Count);
            Assert.AreEqual(2, dict.Values.Count);

            dict.Clear();

            Assert.AreEqual(0, dict.Count);
        }

        [TestCase]
        public void GenericTools_Collection_DataStore()
        {
            var store = new DataStore()
                .Set("test", 3)
                .Set("test2", new SampleModel())
                .Set("test3", "bla");

            Assert.AreEqual(3, store.Get<int>("test"));
            Assert.AreEqual(typeof(SampleModel), store.Get<SampleModel>("test2").GetType());
            Assert.AreEqual("bla", store.Get<string>("test3"));
        }

        [TestCase]
        public void GenericTools_Collection_EventList()
        {
            var addedCounter = 0;
            var removeCounter = 0;

            var list = new EventList<int>
            {
                ItemAdded = i => addedCounter += i,
                ItemRemoved = i => removeCounter += i
            };

            list.Add(1);
            list.AddRange(new[] { 2, 3 });

            list.Remove(1);
            list.RemoveAt(0);

            Assert.AreEqual(6, addedCounter);
            Assert.AreEqual(4, removeCounter);
        }

        [TestCase]
        public void GenericTools_Require_List()
        {
            var sampleModel = new SampleModel() { Age = 23, Type = ModelType.Test };

            Assert.DoesNotThrow(() => Require.That(sampleModel,
                model => model != null,
                model => model.Age > 18
                ));
        }

        [TestCase]
        public void GenericTools_RequirementBuilder()
        {
            var model = new SampleModel { Age = 14, Type = ModelType.Test };

            var requirements = RequirementBuilder.Load<SampleModel>("requirement.json");

            try
            {
                Require.That(model, requirements);
            }
            catch (RequirementNotMetException rex)
            {
                Assert.AreEqual("", rex.Message);
            }
        }

        [TestCase]
        public void GenericTools_Require_Throw()
        {
            var sampleModel = new SampleModel() { Age = 17, Type = ModelType.Test };

            try
            {
                Require.That(sampleModel,
                    model => model != null,
                    model => model.Age > 18
                    );
            }
            catch (Exception rex)
            {
                Assert.IsNotNull(rex);
            }
        }

        //////HELPERS

        void vm_LoadChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Assert.AreEqual("IsLoaded", e.PropertyName);
        }

        void vm_TestChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Assert.AreEqual("Test", e.PropertyName);
        }
    }
}