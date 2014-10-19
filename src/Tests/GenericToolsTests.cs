using System;
using System.IO;
using System.Linq;
using System.Threading;
using int32.Utils.Core.Extensions;
using int32.Utils.Core.Generic.Base;
using int32.Utils.Core.Generic.Collections;
using int32.Utils.Core.Generic.Data;
using int32.Utils.Core.Generic.Factory;
using int32.Utils.Core.Generic.Repository;
using int32.Utils.Core.Generic.Singleton;
using int32.Utils.Core.Generic.Tasks;
using int32.Utils.Core.Generic.ViewModel;
using int32.Utils.Tests;
using NUnit.Framework;
using Tests.Core;
using Tests.Samples;

namespace Tests
{
    [TestFixture]
    public class GenericToolsTest : BaseTest
    {
        [TestCase]
        public void GenericTools_Singleton()
        {
            var a = Singleton<SampleModel>.Instance;
            var b = Singleton<SampleModel>.Instance;

            MakeSure.That(a).Is(b);
        }

        [TestCase]
        public void GenericTools_Singleton_Class()
        {
            var a = SampleModelSingleton.Instance;
            var b = SampleModelSingleton.Instance;

            MakeSure.That(a).Is(b);
        }

        [TestCase]
        public void GenericTools_Factory()
        {
            MakeSure.That(SampleModelFactory.Create()).IsNot(null);
            MakeSure.That(SampleModelFactory.Create(model => model.Age = 17).Age).Is(17);
        }

        [TestCase]
        public void GenericTools_Repository()
        {
            var repo = Factory<SampleModelRepository>.Create();
            MakeSure.That(repo.Get().Age).Is(1);
        }

        [TestCase]
        public void GenericTools_RepositoryHandler()
        {
            var vm = RepositoryHandler.Add(new SampleModelRepository());
            MakeSure.That(RepositoryHandler.Get<SampleModelRepository>()).Is((SampleModelRepository)vm);
        }

        [TestCase]
        public void GenericTools_RepositoryHandler_Get()
        {
            //register sample service
            RepositoryHandler.Add(new SampleModelRepository());

            var sampleModel = RepositoryHandler.Get<SampleModelRepository>().Get();

            MakeSure.That(sampleModel).IsNot(null);
            MakeSure.That(sampleModel.Age).Is(1);

            var sampleModel2 = RepositoryHandler.Get<SampleModelRepository>().Get(i => i.Age > 17);
            MakeSure.That(sampleModel2.Age).Is(22);

            MakeSure.That(RepositoryHandler.Get<SampleModelRepository>().Count).Is(3);
        }

        [TestCase]
        public void GenericTools_RepositoryHandler_GetAll()
        {
            //register sample service
            RepositoryHandler.Add(new SampleModelRepository());

            var count = RepositoryHandler.Get<SampleModelRepository>().GetAll().Count();
            MakeSure.That(count).Is(3);

            var count2 = RepositoryHandler.Get<SampleModelRepository>().GetAll(i => i.Age > 17).Count();
            MakeSure.That(count2).Is(2);
        }

        [TestCase]
        public void GenericTools_RepositoryHandler_AddUpdateDelete()
        {
            //register sample service
            RepositoryHandler.Add(new SampleModelRepository());

            var item = RepositoryHandler.Get<SampleModelRepository>().Add(new SampleModel() { Age = 50 });

            MakeSure.That(item.Age).Is(50);
            MakeSure.That(RepositoryHandler.Get<SampleModelRepository>().Get(i => i == item)).Is(item);
            MakeSure.That(RepositoryHandler.Get<SampleModelRepository>().Count).Is(4);

            var newItem = RepositoryHandler.Get<SampleModelRepository>().Update(item, new SampleModel() { Age = 53 });

            MakeSure.That(RepositoryHandler.Get<SampleModelRepository>().Get(i => i == newItem)).Is(newItem);
            MakeSure.That(RepositoryHandler.Get<SampleModelRepository>().Count).Is(4);

            RepositoryHandler.Get<SampleModelRepository>().Delete(i => i.Age > 40);
            MakeSure.That(RepositoryHandler.Get<SampleModelRepository>().Count).Is(3);

            MakeSure.That(() => RepositoryHandler.Get<SampleModelRepository>().SaveChanges()).DoesNotThrow();
        }

        [TestCase]
        public void GenericTools_RepositoryHandler_RegisterMultiple()
        {
            MakeSure.That(() =>
            {
                var items = RepositoryHandler.Add(
                    new SampleModelRepository(),
                    new SampleModelRepository(),
                    new SampleModelRepository()
                    );

                MakeSure.That(items.Count()).Is(3);
            }).DoesNotThrow();


            MakeSure.That(RepositoryHandler.Get<SampleModelRepository>()).IsNot(null);
        }

        [TestCase]
        public void GenericTools_Timer_Measure()
        {
            MakeSure.That(Timing.Measure(() => Thread.Sleep(200)).Milliseconds).IsGreaterThan(100);
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

            MakeSure.That(vm).IsNot(null);
            MakeSure.That(ViewModelHandler.Get<SampleModelViewModel>()).Is(vm);

            vm.PropertyChanged += vm_LoadChanged;
            vm.Load();

            MakeSure.That(vm.IsLoaded).Is(true);
        }

        [TestCase]
        public void GenericTools_Collections_FluentList()
        {
            var list = new FluentList<SampleModel>()
                .Add(new SampleModel { Title = "Test" })
                .Add(new SampleModel { Title = "Test2" })
                .Add(new SampleModel { Title = "Test3" })
                .RemoveAt(0).Reverse().Update(i => i.Type = ModelType.Test);

            MakeSure.That(list.Count).Is(2);
            MakeSure.That(list.First().Type).Is(ModelType.Test);
        }

        [TestCase]
        public void GenericTools_Collections_FluentDictionary()
        {
            var dict = new FluentDictionary<string, int>()
                .Add("Key1", 1)
                .Add("Key2", 2)
                .Add("Key3", 3)
                .Remove("Key3");

            MakeSure.That(dict.Count).Is(2);
            MakeSure.That(dict.ContainsKey("Key2")).Is(true);
            MakeSure.That(dict["Key1"]).Is(1);
            MakeSure.That(dict.Keys.Count).Is(2);
            MakeSure.That(dict.Values.Count).Is(2);

            dict.Clear();

            MakeSure.That(dict.Count).Is(0);
        }

        [TestCase]
        public void GenericTools_Collection_DataStore()
        {
            var store = new DataStore()
                .Set("test", 3)
                .Set("test2", new SampleModel())
                .Set("test3", "bla");

            MakeSure.That(store.Get<int>("test")).Is(3);
            MakeSure.That(store.Get<SampleModel>("test2").GetType()).Is(typeof(SampleModel));
            MakeSure.That(store.Get<string>("test3")).Is("bla");
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

            MakeSure.That(addedCounter).Is(6);
            MakeSure.That(removeCounter).Is(4);
        }

        [TestCase]
        public void GenericTools_Require_List()
        {
            var sampleModel = new SampleModel() { Age = 23, Type = ModelType.Test };

            MakeSure.That(() => Require.That(sampleModel,
                model => model != null,
                model => model.Age > 18
                )).DoesNotThrow();
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
                MakeSure.That(rex.Message).Is(string.Empty);
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
                MakeSure.That(rex).IsNot(null);
            }
        }

        [TestCase]
        public void GenericTools_Data_MemoryDatabase()
        {
            var db = new MemoryDatabase<SampleModel>();

            db.Add(new SampleModel());
            db.Add(new SampleModel { Age = 17 });
            db.Add(new SampleModel { Type = ModelType.Test });

            MakeSure.That(db.Count).Is(3);
            MakeSure.That(db.Get(i => i.Age == 17).Age).Is(17);
            MakeSure.That(db.GetAll(i => i.Type == ModelType.Sample).Count()).Is(2);
        }

        [TestCase]
        public void GenericTools_Data_MemoryDatabase_int()
        {
            var db = new MemoryDatabase<int>();

            db.Add(3);
            db.Add(7);
            db.Add(5);

            MakeSure.That(db.GetAll().Sum(i => i)).Is(15);
        }


        [TestCase]
        public void GenericTools_Data_MemorySession()
        {
            using (var session = new MemorySession())
            {
                var db = session.Database<SampleModel>();
                db.Add(new SampleModel());

                var db2 = session.Database<SampleModel>();
                MakeSure.That(db2.Count).Is(1);
            }
        }

        [TestCase]
        public void GenericTools_Data_Flatdatabase()
        {
            var file = Guid.NewGuid().ToString();
            var db = new FlatDatabase<SampleModel>(file).Load();

            //should not have any files in the database
            Assert.That(db.Count == 0);

            //add a new model to the database
            db.Add(new SampleModel { Age = 17 });

            //save changes (persist to file)
            db.SaveChanges();

            //make sure entry was saved correctly
            Assert.That(db.Count == 1);

            //reload the database from file and check again
            Assert.That(db.Load().Count == 1);

            File.Delete(file);
        }

        [TestCase]
        public void GenericTools_Data_Flatdatabase_Performance()
        {
            var file = Guid.NewGuid().ToString();
            var times = 100 * 1000;
            var db = new FlatDatabase<SampleModel>(file);

            var saving = Timing.Measure(() =>
            {
                for (int i = 0; i < times; i++)
                    db.Add(new SampleModel { Age = 17, Title = "i" + i, Type = ModelType.Example });

                db.SaveChanges();
            });

            Assert.IsTrue(db.Count == times);
            File.Delete(file);
        }

        //[TestCase]
        public void GenericTools_Data_FlatSession()
        {
            using (var session = new FlatSession())
            {
                var file = Guid.NewGuid().ToString();
                var db = session.Database<SampleModel>(file);
                db.Add(new SampleModel { Age = 23 });
                db.SaveChanges();

                var db2 = session.Database<SampleModel>();
                Assert.That(db2.Count == 1);

                File.Delete(file);
            }
        }

        //////HELPERS

        void vm_LoadChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            MakeSure.That(e.PropertyName).Is("IsLoaded");
        }

        void vm_TestChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            MakeSure.That(e.PropertyName).Is("Test");
        }
    }
}