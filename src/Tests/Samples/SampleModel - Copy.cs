using int32.Utils.Extensions;
using int32.Utils.Generics.Repository.Contracts;
using int32.Utils.Generics.Singleton;

namespace Tests.Samples
{
    public class SampleModel : IModel
    {
        private string _test = "a";
        private string Internal { get; set; }
        public string Title { get; set; }
        public int Age { get; set; }

        public ModelType Type { get; set; }

        public string Return()
        {
            return this.ToJSON();
        }
    }

    public class SampleModelSingleton : Singleton<SampleModel> { }

    public enum ModelType
    {
        Sample,
        Example,
        Test
    }
}
