using System.Collections.Generic;
using isukces.code.interfaces;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Ammy
{
    public partial class AmmyBindBuilder:IAnnotableByUser 
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
            switch (ValidationRules.Count)
            {
                case 0:
                    return ammyBind;
                case 1:
                    ammyBind.WithValidationRules(ValidationRules[0]);
                    break;
                default:
                {
                    var array = new AmmyArray();
                    foreach (var i in ValidationRules)
                        array.Items.Add(i);
                    ammyBind.WithValidationRules(array);
                    break;
                }
            }
            return ammyBind;
        }

        public AmmyBindBuilder WithValidationRule(object data)
        {
            if (data != null)
                ValidationRules.Add(data);
            return this;
        }

        public IDictionary<string, object> UserAnnotations { get; } = new Dictionary<string, object>();
    }
}