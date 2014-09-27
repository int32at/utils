using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using int32.Utils.Extensions;
using int32.Utils.Generics.Repository;

namespace Tests.Samples
{
    public class SampleModelRepository : Repository<SampleModel>
    {
        private readonly List<SampleModel> _data;

        public SampleModelRepository()
        {
            _data = new List<SampleModel>()
            {
                new SampleModel() {Age = 17},
                new SampleModel() {Age = 22},
                new SampleModel() {Age = 33}
            };
        }

        public override int Count
        {
            get { return _data.Count; }
        }

        public override SampleModel Add(SampleModel item)
        {
            _data.Add(item);
            return item;
        }

        public override void Delete(SampleModel item)
        {
            _data.Remove(item);
        }

        public override void Delete(Func<SampleModel, bool> predicate)
        {
            _data.Remove(predicate);
        }

        public override SampleModel Get()
        {
            return new SampleModel() { Age = 1 };
        }

        public override SampleModel Get(Func<SampleModel, bool> predicate)
        {
            return _data.FirstOrDefault(predicate);
        }

        public override IEnumerable<SampleModel> GetAll()
        {
            return _data;
        }

        public override IEnumerable<SampleModel> GetAll(Func<SampleModel, bool> predicate)
        {
            return _data.Where(predicate);
        }

        public override SampleModel Update(SampleModel oldItem, SampleModel newItem)
        {
            var x = _data.Find(model => model == oldItem);
            x.Age = newItem.Age;
            return x;
        }

        public override void SaveChanges()
        {

        }
    }
}
