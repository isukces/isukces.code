using System.Collections.Generic;
using isukces.code.interfaces.Ammy;

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
            var ammyBind = new AmmyBind(Path)
            {
                From = From
            };
            SetupAmmyBind(ammyBind);
            if (ValidationRules.Count > 0)
            {
                if (ValidationRules.Count == 1)
                {
                    ammyBind.WithValidationRules(ValidationRules[0]);
                }
                else
                {
                    var array = new AmmyArray();
                    foreach (var i in ValidationRules)
                        array.Items.Add(i);
                    ammyBind.WithValidationRules(array);
                }
            }

            return ammyBind;
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

        void IAmmyBindSourceHost.SetBindingSource(object bindingSource)
        {
            From = bindingSource;
        }

        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        public string           Path            { get; set; }
        public object           From            { get; set; }
    }
}