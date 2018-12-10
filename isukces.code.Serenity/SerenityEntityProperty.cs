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
            _type  = new ReflectionTypeWrapper(type);
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

        public Type RowPropertyType => _type.MakeNullableIfPossible();

        public Type SerenityRowPropertyType
        {
            get
            {
                var t = new ReflectionTypeWrapper(_type.UnwrapNullable());
                if (t.IsEnum)
                    return typeof(int?);
                return t.MakeNullableIfPossible();
            }
        }

        public string Name { get; }

        public IList<ICsAttribute> Attributes { get; } = new List<ICsAttribute>();

        public string RowFieldType
        {
            get
            {
                var t = _type.UnwrapNullable();
                if (t == typeof(string)) return "StringField";
                if (t == typeof(int)) return "Int32Field";
                if (t == typeof(DateTime)) return "DateTimeField";
                if (t == typeof(bool)) return "BooleanField";
                if (t == typeof(Guid)) return "GuidField";
                throw new Exception("Unsupported type " + t);
            }
        }

        private ReflectionTypeWrapper _type;
        private readonly CsProperty _property;
        private readonly SerenityEntityBuilder _owner;
    }
}