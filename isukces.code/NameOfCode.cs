using isukces.code.interfaces;

namespace isukces.code
{
    public class NameOfCode : IDirectCode
    {
        public NameOfCode(string argument)
        {
            Argument = argument;
        }

        public override string ToString()
        {
            return Code;
        }

        public string Argument { get; }

        public string Code => "nameof(" + Argument + ")";
    }
}