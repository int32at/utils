namespace int32.Utils.Logger
{
    public class LogConfig
    {
        public bool EnableDebugging { get; set; }
        public bool EnableInfos { get; set; }
        public bool EnableWarnings { get; set; }
        public bool EnableErrors { get; set; }
        public string Format { get; set; }

        public LogConfig()
        {
            Format = "{0}\t{1}\t{2}";
        }
    }
}
