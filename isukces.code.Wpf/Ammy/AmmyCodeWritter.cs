using System.Collections.Generic;
using System.Linq;
using isukces.code.interfaces;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Wpf.Ammy
{
    public class AmmyCodeWritter : CodeWritter, IAmmyNamespaceProvider, IAmmyCodeWritter
    {
        public AmmyCodeWritter() : base(AmmyLangInfo.Instance)
        {
        }

        public void AddNamespace<T>()
        {
            _namespaces.Add(typeof(T).Namespace);
        }

      

        public void Writeln(IAmmyExpression expression)
        {
            var c = new ConversionCtx
            {
                Aaa = this
            };
            Writeln(expression.GetAmmyCode(c));
        }

        public string AmmyCode
        {
            get
            {
                var ns = Namespaces.Distinct().Select(q => "using " + q + "\r\n");
                return string.Join("", ns) + "\r\n" + Code;
            }
        }

        private List<string>          _namespaces { get; } = new List<string>();
        public  IReadOnlyList<string> Namespaces  => _namespaces;
    }
}