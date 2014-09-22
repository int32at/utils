using int32.Utils.Extensions;
using int32.Utils.ServiceHandler.Contracts;

namespace int32.Utils.ServiceHandler.Services
{
    public abstract class BaseModel : IServiceModel
    {
        public string Return()
        {
            return this.ToJSON();
        }
    }
}
