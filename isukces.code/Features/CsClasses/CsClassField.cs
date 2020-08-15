using System.Text;
using isukces.code.interfaces;

namespace isukces.code
{
    public class CsClassField : CsMethodParameter, ICsClassMember, ICommentable
    {
        public CsClassField(string name) : base(name)
        {
        }

        public CsClassField(string name, string type) : base(name, type)
        {
        }

        public CsClassField(string name, string type, string description) 
            : base(name, type, description)
        {
        }

        public void AddComment(string x)
        {
            _extraComment.AppendLine(x);
        }

        public string GetComments() => _extraComment.ToString();

        public string CompilerDirective { get; set; }

        private readonly StringBuilder _extraComment = new StringBuilder();
    }
}