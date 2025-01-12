using iSukces.Code.AutoCode;

#nullable disable
namespace iSukces.Code.Irony
{
    public interface IDoEvaluateHelper
    {
        bool GetDataClassConstructorArgument(Input34 input);
    }

    public sealed class Input34
    {
        public Input34(ConstructorBuilder.Argument argument, bool lastChance, CsCodeWriter writer)
        {
            Argument   = argument;
            LastChance = lastChance;
            Writer     = writer;
        }

        public ConstructorBuilder.Argument Argument   { get; }
        public bool                        LastChance { get; }
        public CsCodeWriter                Writer     { get; }
        public CsExpression                Expression { get; set; }
    }
}

