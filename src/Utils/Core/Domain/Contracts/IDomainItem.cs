using int32.Utils.Core.Configuration;

namespace int32.Utils.Core.Domain.Contracts
{
    public interface IDomainItem
    {
        string Name { get; }
        DomainType Type { get; }
        Config Config { get; set; }
    }
}
