using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using iSukces.Code.Compatibility.System.Windows.Data;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Ammy
{
    public partial class AmmyBindBuilder : IAnnotableByUser
    {
        public AmmyBindBuilder(string path) => Path = path;

        public static AmmyBindBuilder FromAncestor<T>(Expression<Func<T, object>> func, XBindingMode? mode = null)
        {
            var path = ExpressionTools.GetBindingPath(func);
            var tmp  = new AmmyBindBuilder(path).WithBindFromAncestor<T>();
            if (mode != null)
                tmp = tmp.WithMode(mode);
            return tmp;
        }

        public static AmmyBindBuilder FromElementName<T>(Expression<Func<T, object>> func, XBindingMode? mode = null)
        {
            var path        = ExpressionTools.GetBindingPath(func);
            var elementName = path.GetUntilSeparator(".", out path);
            var tmp = new AmmyBindBuilder(path)
                .WithBindFromElementName(elementName);
            if (mode != null)
                tmp = tmp.WithMode(mode);
            return tmp;
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