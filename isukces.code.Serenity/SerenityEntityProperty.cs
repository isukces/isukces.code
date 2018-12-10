using System;
using System.Collections.Generic;
using System.Globalization;
using isukces.code.interfaces;

namespace isukces.code.Serenity
{
    public class SerenityEntityProperty : IAttributable
    {
        public SerenityEntityProperty(string name, Type type, SerenityEntityBuilder owner)
        {
            Name   = name;
            _owner = owner;
            Type   = new ReflectionTypeWrapper(type);
        }


        public SerenityEntityProperty WithColumn(string id)
        {
            return this.WithAttribute("Serenity.Data.Mapping.Column", id);
        }


        public SerenityEntityProperty WithDisplayName(string id)
        {
            return this.WithAttribute("System.ComponentModel.DisplayName", id);
        }

        public SerenityEntityProperty WithIdRow()
        {
            _owner.IdRow = this;
            return this;
        }

        public SerenityEntityProperty WithNameRow()
        {
            _owner.NameRow = this;
            return this;
        }

        public SerenityEntityProperty WithNotNull()
        {
            return this.WithAttribute("Serenity.Data.Mapping.NotNull");
        }

        public SerenityEntityProperty WithPrimaryKey()
        {
            return this.WithAttribute("Serenity.Data.Mapping.PrimaryKey");
        }

        public SerenityEntityProperty WithQuickFilter()
        {
            return this.WithAttribute("Serenity.ComponentModel.QuickFilter");
        }

        public SerenityEntityProperty WithQuickSearch()
        {
            return this.WithAttribute("Serenity.Data.Mapping.QuickSearch");
        }

        public SerenityEntityProperty WithSize(int maxLength)
        {
            return this.WithAttribute("Serenity.Data.Mapping.Size", maxLength.ToString(CultureInfo.InvariantCulture));
        }


        public string Name { get; }

        public IList<ICsAttribute> Attributes { get; } = new List<ICsAttribute>();


        public ReflectionTypeWrapper Type { get; }
        private readonly CsProperty _property;
        private readonly SerenityEntityBuilder _owner;
    }
}