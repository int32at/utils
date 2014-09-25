using System.Collections.Generic;

namespace int32.Utils.Domains.Contracts
{
    public interface IDomainItem
    {
        string Name { get; }
        DomainType Type { get; }
        Dictionary<string, object> Store { get; set; }
    }
}
