using int32.Utils.Generics;

namespace Tests.Samples
{
    public class SampleModel
    {
        private string _test = "a";
        private string Internal { get; set; }
        public string Title { get; set; }
        public int Age { get; set; }

        public ModelType Type { get; set; }
    }

    public class SampleModelSingleton : Singleton<SampleModel> { }

    public enum ModelType
    {
        Sample,
        Example,
        Test
    }
}
