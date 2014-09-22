using System.Collections.Generic;
using int32.Utils.Extensions;
using int32.Utils.ServiceHandler.Contracts;

namespace int32.Utils.ServiceHandler.Extensions
{
    public static class ServiceExtensions
    {
        public static string Return(this IEnumerable<IServiceModel> models)
        {
            return models.ToJSON();
        }
    }
}
