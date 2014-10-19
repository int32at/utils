using int32.Utils.Core.Extensions;
using int32.Utils.Core.Generic.Repository.Contracts;
using int32.Utils.Core.Generic.Singleton;

namespace Tests.Samples
{
    public class SampleModel : IModel
    {
        // ReSharper disable once InconsistentNaming
        private string _test = "a";
        private string Internal { get; set; }
        public string Title { get; set; }
        public int Age { get; set; }

        public ModelType Type { get; set; }

        public string Return()
        {
            return this.ToJSON();
        }

        public string Test()
        {
            return _test;
        }
    }

    public class SampleModelDto 
    {
        public string Title { get; set; }
        public int Age { get; set; }
    }

    public class SampleModelSingleton : Singleton<SampleModel> { }

    public enum ModelType
    {
        Sample,
        Example,
        Test
    }
}
