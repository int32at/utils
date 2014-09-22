using int32.Utils.Extensions;

namespace Tests.Samples
{
    public class VersionModel
    {
        public int Version { get; set; }
        public string Return()
        {
            return this.ToJSON();
        }
    }
}
