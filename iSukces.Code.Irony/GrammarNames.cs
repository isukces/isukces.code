using iSukces.Code.Interfaces;

#nullable disable
namespace iSukces.Code.Irony
{
    public interface IGrammarNamespaces
    {
        string GrammarNamespace { get; }
        string AstNamespace     { get; }
        string DataNamespace    { get; }
    }

    public struct GrammarNames : IGrammarNamespaces
    {
        private GrammarNames(string grammarNamespace, string astNamespace, string dataNamespace,
            NamespaceAndName grammarType)
        {
            GrammarNamespace = grammarNamespace;
            AstNamespace     = astNamespace;
            DataNamespace    = dataNamespace;
            GrammarType      = grammarType;
        }

        public static GrammarNames Make(NamespaceAndName grammarType) =>
            new GrammarNames(
                grammarType.Namespace,
                grammarType.Namespace + AstSuffix,
                grammarType.Namespace + DataSuffix,
                grammarType);

        public override string ToString() => "GrammarNames " + GrammarType.FullName;

        public string GrammarNamespace { get; }

        public string AstNamespace  { get; }
        public string DataNamespace { get; }

        public NamespaceAndName GrammarType { get; }

        public static string AstSuffix = ".Ast";
        public static string DataSuffix = ".Data";
    }
}

