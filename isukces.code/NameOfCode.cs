using isukces.code.interfaces;

namespace isukces.code
{
    public class NameOfCode : IDirectCode
    {
        public NameOfCode(string argument)
        {
            Argument = argument;
        }

        public string Argument { get; }
        public string Code
        {
            get { return "nameof(" + Argument + ")"; }
        }
    }
}