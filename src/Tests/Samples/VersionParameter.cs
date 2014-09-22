
using int32.Utils.ServiceHandler.Contracts;

namespace Tests.Samples
{
    public class VersionParameter : IServiceParameter
    {
        public string Key { get; set; }
        public int Value { get; set; }
    }
}
