
using int32.Utils.Extensions;

namespace Tests.Samples
{
    public class HomeModel
    {
        public string Title { get; set; }

        public string Return()
        {
            return this.ToJSON();
        }
    }
}
