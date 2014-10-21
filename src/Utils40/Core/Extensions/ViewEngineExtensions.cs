using System.Collections.Generic;
using int32.Utils.Core.Generic.ViewEngine;
using int32.Utils.Core.Generic.ViewEngine.Contracts;

namespace int32.Utils.Core.Extensions
{
    public static class ViewEngineExtensions
    {
        public static string Render(this IRenderable renderable, string template)
        {
            renderable.ThrowIfNull("Renderable");
            return ViewEngine.Instance.Render(template, renderable);
        }

        public static string Render(this IEnumerable<IRenderable> renderables, string template)
        {
            renderables.ThrowIfNull("Renderables");
            return ViewEngine.Instance.Render(template, renderables);
        }
    }
}
