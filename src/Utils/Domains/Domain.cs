using System.Collections.Generic;
using System.Linq;
using int32.Utils.Domains.Contracts;
using int32.Utils.Extensions;

namespace int32.Utils.Domains
{
    public static class Domain
    {
        private static IDomainItem _current;

        private static readonly List<IDomainItem> Domains = new List<IDomainItem>()
        {
            new Development(),
            new Integration(),
            new QualityAssurance(),
            new Staging(),
            new Production()
        };

        public static IDomainItem Current
        {
            get { return _current.IfNull(() => Domains.First()); }
            set { _current = value; }
        }

        public static void SetTo<T>() where T : IDomainItem
        {
            var type = typeof(T);

            Domains.FirstOrDefault(i => i.GetType() == type)
                .IfNotNull(i => _current = i);
        }
    }

    public class Development : IDomainItem
    {
        public string Name { get; internal set; }
        public DomainType Type { get; internal set; }
        public Dictionary<string, object> Store { get; set; }

        public Development()
        {
            Name = "Development";
            Type = DomainType.Development;
            Store = new Dictionary<string, object>();
        }
    }

    public class Integration : Development
    {
        public Integration()
        {
            Name = "Integration";
            Type = DomainType.Integration;
        }
    }

    public class QualityAssurance : Integration
    {
        public QualityAssurance()
        {
            Name = "QualityAssurance";
            Type = DomainType.QualityAssurance;
        }
    }

    public class Staging : QualityAssurance
    {
        public Staging()
        {
            Name = "Staging";
            Type = DomainType.Staging;
        }
    }

    public class Production : QualityAssurance
    {
        public Production()
        {
            Name = "Production";
            Type = DomainType.Production;
        }
    }
}
