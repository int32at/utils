using System.Linq;
using int32.Utils.Generics.Factory;
using int32.Utils.Generics.Repository;
using int32.Utils.Generics.Singleton;
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
        public void GenericTools_RepositoryHandler_Get()
        {
            //register sample service
            RepositoryHandler.Register(new SampleModelRepository());

            var sampleModel = RepositoryHandler.Repository<SampleModelRepository>().Get();

            Assert.IsNotNull(sampleModel);
            Assert.AreEqual(1, sampleModel.Age);

            var sampleModel2 = RepositoryHandler.Repository<SampleModelRepository>().Get(i => i.Age > 17);
            Assert.AreEqual(22, sampleModel2.Age);

            Assert.AreEqual(3, RepositoryHandler.Repository<SampleModelRepository>().Count);
        }

        [TestCase]
        public void GenericTools_RepositoryHandler_GetAll()
        {
            //register sample service
            RepositoryHandler.Register(new SampleModelRepository());

            var count = RepositoryHandler.Repository<SampleModelRepository>().GetAll().Count();
            Assert.AreEqual(3, count);

            var count2 = RepositoryHandler.Repository<SampleModelRepository>().GetAll(i => i.Age > 17).Count();
            Assert.AreEqual(2, count2);
        }

        [TestCase]
        public void GenericTools_RepositoryHandler_AddUpdateDelete()
        {
            //register sample service
            RepositoryHandler.Register(new SampleModelRepository());

            var item = RepositoryHandler.Repository<SampleModelRepository>().Add(new SampleModel() { Age = 50 });

            Assert.AreEqual(50, item.Age);
            Assert.AreEqual(item, RepositoryHandler.Repository<SampleModelRepository>().Get(i => i == item));
            Assert.AreEqual(4, RepositoryHandler.Repository<SampleModelRepository>().Count);

            var newItem = RepositoryHandler.Repository<SampleModelRepository>().Update(item, new SampleModel() { Age = 53 });
            Assert.AreEqual(newItem, RepositoryHandler.Repository<SampleModelRepository>().Get(i => i == newItem));
            Assert.AreEqual(4, RepositoryHandler.Repository<SampleModelRepository>().Count);

            RepositoryHandler.Repository<SampleModelRepository>().Delete(i => i.Age > 40);
            Assert.AreEqual(3, RepositoryHandler.Repository<SampleModelRepository>().Count);

            Assert.DoesNotThrow(() => RepositoryHandler.Repository<SampleModelRepository>().SaveChanges());
        }

        [TestCase]
        public void GenericTools_RepositoryHandler_RegisterMultiple()
        {
            Assert.DoesNotThrow(() => RepositoryHandler.Register(
                new SampleModelRepository(),
                new SampleModelRepository(),
                new SampleModelRepository()
                ));

            Assert.IsNotNull(RepositoryHandler.Repository<SampleModelRepository>());
        }
    }
}
