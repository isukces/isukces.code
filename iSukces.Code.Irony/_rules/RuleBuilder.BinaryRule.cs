using System.Collections.Generic;
using System.Linq;
using iSukces.Code.Interfaces;

#nullable disable
namespace iSukces.Code.Irony
{
    public abstract partial class RuleBuilder
    {
        public class BinaryRule : RuleBuilder
        {
            public BinaryRule(string delimiter, IReadOnlyList<ICsExpression> items)
            {
                Delimiter = delimiter;
                Items     = items;
            }

            public BinaryRule(string delimiter, params ICsExpression[] items)
            {
                Delimiter = delimiter;
                Items     = items;
            }

            public static BinaryRule Or(params ICsExpression[] items) => new BinaryRule(" | ", items);

            public static BinaryRule Plus(params ICsExpression[] items) => new BinaryRule(" + ", items);

            public override string GetCode(ITypeNameResolver resolver)
            {
                var code = string.Join(Delimiter, Items.Select(a => a.GetCode(resolver)));
                if (Brackets)
                    return $"({code})";
                return code;
            }

            public BinaryRule WithBrackets()
            {
                Brackets = true;
                return this;
            }

            public bool Brackets { get; set; }

            public string                       Delimiter { get; }
            public IReadOnlyList<ICsExpression> Items     { get; }
        }
    }
}

