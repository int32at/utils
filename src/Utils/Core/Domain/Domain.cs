﻿using System;
using System.Collections.Generic;
using System.Linq;
using int32.Utils.Core.Configuration;
using int32.Utils.Core.Extensions;
using int32.Utils.Core.Domain.Contracts;

namespace int32.Utils.Core.Domain
{
    public static class Domain
    {
        private static IDomainItem _current;

        private static readonly List<IDomainItem> Domains = new List<IDomainItem>
        {
            new Development(),
            new Integration(),
            new QualityAssurance(),
            new Staging(),
            new Production()
        };

        public static Action OnChanged;

        public static IDomainItem Current
        {
            get
            {
                if (_current == null)
                {
                    _current = Domains.First();
                    _current.Config.Load();
                }

                return _current;
            }
        }

        public static void SetTo<T>() where T : IDomainItem
        {
            var type = typeof(T);

            Domains.FirstOrDefault(i => i.GetType() == type)
                .IfNotNull(i =>
                {
                    _current = i;
                    //execute onChanged when subscribed
                    OnChanged.Execute();
                });
        }
    }

    public class Development : IDomainItem
    {
        public string Name { get; internal set; }
        public DomainType Type { get; internal set; }
        public Config Config { get; set; }

        public Development()
        {
            Name = "Development";
            Type = DomainType.Development;
            Config = new Config();
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
