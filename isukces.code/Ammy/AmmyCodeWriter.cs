using System.Collections.Generic;
using System.Linq;
using iSukces.Code.Interfaces;
using iSukces.Code.Interfaces.Ammy;

namespace iSukces.Code.Ammy
{
    public class AmmyCodeWriter : CodeWriter, IAmmyNamespaceProvider, IAmmyCodeWriter
    {
        public AmmyCodeWriter() : base(AmmyLangInfo.Instance)
        {
        }

        protected override string GetCodeForSave()
        {
            return FullCode;
        }

        public ISet<string> Namespaces
        {
            get { return _namespaces; }
        }

        public string FullCode
        {
            get
            {
                var code = Code;
                if (!Namespaces.Any())
                    return code;
                var ns = Namespaces.OrderBy(a => a).Select(a => "using " + a);
                code = string.Join("\r\n", ns) + "\r\n\r\n" + code;
                return code;
            }
        }

        private readonly HashSet<string> _namespaces = new HashSet<string>();
    }
}