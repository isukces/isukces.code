using Irony.Ast;
using Irony.Interpreter;
using Irony.Parsing;

namespace Bitbrains.AmmyParser
{
    public partial class AstObjectDefinition
    {
        public IAstStatement GetData(ScriptThread thread)
        {
            var x                  = (object[])base.DoEvaluate(thread);
            var typeName           = (FullQualifiedNameData)x[0];
            var settingsCollection = (AstObjectSettingsCollection)x[2];
            var instanceName = (string)x[1];
            return new AstObjectDefinitionData(Span, typeName, settingsCollection,instanceName);
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
        }

        protected override object DoEvaluate(ScriptThread thread) => GetData(thread);
    }

    public class AstObjectDefinitionData : IBaseData, IAstStatement
    {
        public AstObjectDefinitionData(SourceSpan span, FullQualifiedNameData typeName,
            AstObjectSettingsCollection settingsCollection, string instanceName)
        {
            Span               = span;
            TypeName           = typeName;
            SettingsCollection = settingsCollection;
            InstanceName = instanceName;
        }

        public SourceSpan                  Span               { get; }
        public FullQualifiedNameData                      TypeName           { get; }
        public AstObjectSettingsCollection SettingsCollection { get; }
        public string InstanceName { get; }
    }
}