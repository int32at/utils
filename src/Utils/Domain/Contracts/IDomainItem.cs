using int32.Utils.Configuration;

namespace int32.Utils.Domain.Contracts
{
    public interface IDomainItem
    {
        string Name { get; }
        DomainType Type { get; }
        Config Config { get; set; }
    }
}
