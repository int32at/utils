using int32.Utils.Core.Generic.Singleton;
using int32.Utils.Core.Generic.ViewEngine.Contracts;

namespace int32.Utils.Core.Generic.ViewEngine
{
    public class ViewEngine : Singleton<IViewEngine>
    {
        public string Render(string content, dynamic model)
        {
            return content;
        }
    }
}
