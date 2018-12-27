using isukces.code.interfaces;

namespace isukces.code
{
    public class CsClassField : CsMethodParameter, ICsClassMember
    {
        public CsClassField(string name) : base(name)
        {
        }

        public CsClassField(string name, string type) : base(name, type)
        {
        }

        public CsClassField(string name, string type, string description) : base(name, type, description)
        {
        }

        public string CompilerDirective { get; set; }
    }
}