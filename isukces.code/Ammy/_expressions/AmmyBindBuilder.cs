using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace isukces.code.Ammy
{
    public class AmmyBindBuilder
    {
        public AmmyBindBuilder(string path)
        {
            Path = path;
        }

        public AmmyBind Build()
        {
            var ab = new AmmyBind(Path);
            if (Mode != null)
                ab.AddParameter("Mode", Mode.Value);
            if (Converter != null)
                ab.AddParameter("Converter", Converter);
            if (ValidationRules.Count > 0)
            {
                if (ValidationRules.Count == 1)
                {
                    ab.AddParameter("ValidationRules", ValidationRules[0]);
                }
                else
                {
                    var array = new AmmyArray();
                    foreach (var i in ValidationRules)
                        array.Items.Add(i);
                    ab.AddParameter("ValidationRules", array);
                }
            }

            ab.From = From;
            return ab;
        }


        public AmmyBindBuilder WithFromAncestor([NotNull] Type ancestorType, int? level = null)
        {
            if (ancestorType == null) throw new ArgumentNullException(nameof(ancestorType));
            From = new AncestorBindingSource(ancestorType, level);
            return this;
        }
        
        public AmmyBindBuilder WithFromAncestor<T>(int? level = null)
        {
            return WithFromAncestor(typeof(T), level);
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
        public AmmyBindBuilder WithConverter(object converter)
        {
            Converter = converter;
            return this;
        }

        public AmmyBindBuilder WithValidationRule(object data)
        {
            if (data != null)
                ValidationRules.Add(data);
            return this;
        }

        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        public string           Path            { get; set; }
        public DataBindingMode? Mode            { get; set; }
        public object           From            { get; set; }
        public object           Converter       { get; set; }
        public List<object>     ValidationRules { get; } = new List<object>();
    }
}