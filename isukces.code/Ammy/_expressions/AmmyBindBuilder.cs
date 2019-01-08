using System;
using System.Collections.Generic;
using isukces.code.interfaces.Ammy;
using JetBrains.Annotations;

namespace isukces.code.Ammy
{
    public partial class AmmyBindBuilder : IAmmyBindConverterHost, IAmmyBindSourceHost
    {
        public AmmyBindBuilder(string path)
        {
            Path = path;
        }

        public AmmyBind Build()
        {
            var ab = new AmmyBind(Path);
            if (Mode != null)
                ab.WithMode(Mode.Value);
            if (Converter != null)
                ab.WithConverter(Converter);
            if (ValidationRules.Count > 0)
            {
                if (ValidationRules.Count == 1)
                {
                    ab.WithValidationRules(ValidationRules[0]);
                }
                else
                {
                    var array = new AmmyArray();
                    foreach (var i in ValidationRules)
                        array.Items.Add(i);
                    ab.WithValidationRules(array);
                }
            }

            ab.From = From;
            return ab;
        }
 
        public AmmyBindBuilder WithFrom(object data)
        {
            From = data;
            return this;
        }

        public AmmyBindBuilder WithMode(DataBindingMode mode)
        {
            Mode = mode;
            return this;
        }

        public AmmyBindBuilder WithValidationRule(object data)
        {
            if (data != null)
                ValidationRules.Add(data);
            return this;
        }

        void IAmmyBindConverterHost.SetBindConverter(object converter)
        {
            Converter = converter;
        }

        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        public string           Path            { get; set; }
        public DataBindingMode? Mode            { get; set; }
        public object           From            { get; set; }
        public object           Converter       { get; set; }
        public List<object>     ValidationRules { get; } = new List<object>();
        void IAmmyBindSourceHost.SetBindingSource(object bindingSource)
        {
            From = bindingSource;
        }
    }
}