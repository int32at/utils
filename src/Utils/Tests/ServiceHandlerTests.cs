using System;
using System.Collections.Generic;
using System.Linq;
using int32.Utils.Extensions;
using int32.Utils.ServiceHandler;
using int32.Utils.ServiceHandler.Services;
using NUnit.Framework;
using Tests.Samples;

namespace Tests
{
    [TestFixture]
    public class ServiceHandlerTests
    {
        [TestCase]
        public void RestServer_Create_Object()
        {
            using (var svc = new ServiceHandler())
            {
                svc.Register(new VersionService());

                svc.Initialize();

                var item = new VersionModel { Version = 1 };
                var items = new List<VersionModel> { item };

                Assert.AreEqual(2, svc.Service<VersionService>().Add(item).Version);
                Assert.DoesNotThrow(() => svc.Service<VersionService>().Delete(item));

                svc.Service<VersionService>().Add(items);
                Assert.AreEqual(3, svc.Service<VersionService>().GetAll().Count());

                Assert.AreEqual(1, svc.Service<VersionService>().Get().Version);
                Assert.AreEqual(1, svc.Service<VersionService>().Get(1).Version);
                Assert.AreEqual(3, svc.Service<VersionService>().GetAll().Count());
                Assert.AreEqual(1, svc.Service<VersionService>().Get(new VersionParameter { Value = 1 }).Version);
                Assert.AreEqual(1, svc.Service<VersionService>().GetAll(new VersionParameter { Value = 1 }).Count());
                Assert.AreEqual(2, svc.Service<VersionService>().GetAll(i => i.Version == 2).FirstOrDefault().Version);
                Assert.AreEqual(3, svc.Service<VersionService>().GetCount());
            }
        }

        public class VersionService : BaseService<VersionModel, VersionParameter>
        {
            private List<VersionModel> _data;

            public override void Initialize()
            {
                _data = new List<VersionModel> { new VersionModel() { Version = 1 }, new VersionModel() { Version = 2 } };
            }

            public override void Dispose()
            {
                _data = null;
            }

            public override VersionModel Get()
            {
                return _data.First();
            }

            public override VersionModel Get(int id)
            {
                return _data.First(i => i.Version == id);
            }

            public override IEnumerable<VersionModel> GetAll()
            {
                return _data;
            }

            public override IEnumerable<VersionModel> GetAll(Predicate<VersionModel> predicate)
            {
                return _data.Where(i => predicate(i));
            }

            public override VersionModel Get(VersionParameter param)
            {
                return _data.First(i => i.Version == param.Value);
            }

            public override IEnumerable<VersionModel> GetAll(VersionParameter param)
            {
                return _data.Where(i => i.Version == param.Value);
            }

            public override VersionModel Add(VersionModel item)
            {
                item.Version++;
                _data.Add(item);
                return item;
            }

            public override void Add(IEnumerable<VersionModel> items)
            {
                _data.AddRange(items);
            }

            public override void Delete(int id)
            {
                Delete(i => i.Version == id);
            }

            public override void Delete(VersionModel item)
            {
                _data.Remove(item);
            }
            public override void Delete(VersionParameter param)
            {
                Delete(param.Value);
            }

            public override void Delete(Predicate<VersionModel> predicate)
            {
                _data.Where(i => predicate(i)).ForEach(i => _data.Remove(i));
            }

            public override int GetCount()
            {
                return _data.Count;
            }
        }
    }
}