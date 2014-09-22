namespace Tests.Samples
{
    public class SampleModel
    {
        public string Title { get; set; }
        public int Age { get; set; }

        public ModelType Type { get; set; }
    }

    public enum ModelType
    {
        Sample,
        Example,
        Test
    }
}
