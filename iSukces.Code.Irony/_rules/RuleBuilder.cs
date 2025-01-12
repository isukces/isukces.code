using System.Collections.Generic;
using System.Linq;
using iSukces.Code.Interfaces;

#nullable disable
namespace iSukces.Code.Irony
{
    public abstract partial class RuleBuilder : ICsExpression
    {
        public static string GetCode(ITypeNameResolver resolver, string delimiter, IEnumerable<ICsExpression> items)
        {
            return string.Join(delimiter, items.Select(a => a.GetCode(resolver)));
        }

        public abstract string GetCode(ITypeNameResolver resolver);

        public CreationInfoData CreationInfo { get; } = new CreationInfoData();

        public class CreationInfoData
        {
            public CsEnum Enum1 { get; set; }
        }
    }
}

