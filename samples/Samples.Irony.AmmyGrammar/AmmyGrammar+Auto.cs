// ReSharper disable All
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;
using iSukces.Code.Irony;
using System.Collections.Generic;

namespace Samples.Irony.AmmyGrammar
{
    partial class AmmyGrammar : InterpretedLanguageGrammar
    {
        public void AutoInit()
        {
            // generator : IronyAutocodeGenerator.Add_AutoInit:155
            // init 1
            __identifier = TerminalFactory.CreateCSharpIdentifier("identifier");
            __stringLiteral = TerminalFactory.CreateCSharpString("stringLiteral");
            __numberLiteral = TerminalFactory.CreateCSharpNumber("numberLiteral");
            __numberLiteral.Options = NumberOptions.AllowSign
                | NumberOptions.AllowUnderscore;
            __end_of_using_statement = new NonTerminal("end_of_using_statement", typeof(Samples.Irony.AmmyGrammar.Ast.AstEndOfUsingStatement));
            __domain_style_name = new NonTerminal("domain_style_name", typeof(Samples.Irony.AmmyGrammar.Ast.AstDomainStyleName));
            __using_statement = new NonTerminal("using_statement", typeof(Samples.Irony.AmmyGrammar.Ast.AstUsingStatement));
            __using_statement_collection = new NonTerminal("using_statement_collection", typeof(Samples.Irony.AmmyGrammar.Ast.AstUsingStatementCollection));
            __boolean_value = new NonTerminal("boolean_value", typeof(Samples.Irony.AmmyGrammar.Ast.AstBooleanValue));
            __ammy_value = new NonTerminal("ammy_value", typeof(Samples.Irony.AmmyGrammar.Ast.AstAmmyValue));
            __property_set_statement = new NonTerminal("property_set_statement", typeof(Samples.Irony.AmmyGrammar.Ast.AstPropertySetStatement));
            __object_body_element = new NonTerminal("object_body_element", typeof(Samples.Irony.AmmyGrammar.Ast.AstObjectBodyElement));
            __object_body_code_one_line = new NonTerminal("object_body_code_one_line", typeof(Samples.Irony.AmmyGrammar.Ast.AstObjectBodyCodeOneLine));
            __code_lines = new NonTerminal("code_lines", typeof(Samples.Irony.AmmyGrammar.Ast.AstCodeLines));
            __object_body_in_curly_brackets = new NonTerminal("object_body_in_curly_brackets", typeof(Samples.Irony.AmmyGrammar.Ast.AstObjectBodyInCurlyBrackets));
            __main_object_statement = new NonTerminal("main_object_statement", typeof(Samples.Irony.AmmyGrammar.Ast.AstMainObjectStatement));
            __object_name_key_prefix = new NonTerminal("object_name_key_prefix", typeof(Samples.Irony.AmmyGrammar.Ast.AstObjectNameKeyPrefix));
            __object_name_key_prefix_optional = new NonTerminal("object_name_key_prefix_optional", typeof(Samples.Irony.AmmyGrammar.Ast.AstObjectNameKeyPrefixOptional));
            __object_name = new NonTerminal("object_name", typeof(Samples.Irony.AmmyGrammar.Ast.AstObjectName));
            __object_name_optional = new NonTerminal("object_name_optional", typeof(Samples.Irony.AmmyGrammar.Ast.AstObjectNameOptional));
            __object_statement = new NonTerminal("object_statement", typeof(Samples.Irony.AmmyGrammar.Ast.AstObjectStatement));
            __ammy_program = new NonTerminal("ammy_program", typeof(Samples.Irony.AmmyGrammar.Ast.AstAmmyProgram));
            // init Terms
            __comma = ToTerm(",");
            __dot = ToTerm(".");
            __doubledot = ToTerm(":");
            __parOpen = ToTerm("(");
            __parClose = ToTerm(")");
            __true = ToTerm("true");
            __false = ToTerm("false");
            __curlyOpen = ToTerm("{");
            __curlyClose = ToTerm("}");
            // init NonTerminals
            __end_of_using_statement.Rule = NewLinePlus | Eof;
            __domain_style_name.Rule = MakeStarRule(__domain_style_name, __dot, __identifier);
            __using_statement.Rule = ToTerm("using") + __domain_style_name + __end_of_using_statement;
            __using_statement_collection.Rule = MakeStarRule(__using_statement_collection, null, __using_statement);
            __boolean_value.Rule = __true | __false;
            __ammy_value.Rule = __stringLiteral | __numberLiteral | __boolean_value;
            __property_set_statement.Rule = __identifier + __doubledot + __ammy_value;
            __object_body_element.Rule = __property_set_statement | __ammy_value;
            __object_body_code_one_line.Rule = MakePlusRule(__object_body_code_one_line, __comma, __object_body_element);
            __code_lines.Rule = MakeListRule(__code_lines, NewLinePlus, __object_body_code_one_line, TermListOptions.StarList | TermListOptions.AllowTrailingDelimiter);
            __object_body_in_curly_brackets.Rule = __curlyOpen + __code_lines + __curlyClose;
            __main_object_statement.Rule = __domain_style_name + __stringLiteral + this.PreferShiftHere() + __object_body_in_curly_brackets;
            __object_name_key_prefix.Rule = ToTerm("Key") + ToTerm("=");
            __object_name_key_prefix_optional.Rule = Empty | __object_name_key_prefix;
            __object_name.Rule = __object_name_key_prefix_optional + __stringLiteral;
            __object_name_optional.Rule = Empty | __object_name;
            __object_statement.Rule = __domain_style_name + __object_name_optional + __object_body_in_curly_brackets;
            __ammy_program.Rule = __using_statement_collection + __main_object_statement + NewLineStar;
            NonGrammarTerminals.Add(SingleLineComment);
            NonGrammarTerminals.Add(DelimitedComment);
            Root = __ammy_program;
        }

        public CommentTerminal SingleLineComment = new CommentTerminal("SingleLineComment", "//", "\r", "\n", "\u2085", "\u2028", "\u2029");

        public CommentTerminal DelimitedComment = new CommentTerminal("DelimitedComment", "/*", "*/");

        private IdentifierTerminal __identifier;

        private NumberLiteral __numberLiteral;

        private StringLiteral __stringLiteral;

        private KeyTerm __comma;

        private KeyTerm __curlyClose;

        private KeyTerm __curlyOpen;

        private KeyTerm __dot;

        private KeyTerm __doubledot;

        private KeyTerm __false;

        private KeyTerm __parClose;

        private KeyTerm __parOpen;

        private KeyTerm __true;

        private NonTerminal __ammy_program;

        private NonTerminal __ammy_value;

        private NonTerminal __boolean_value;

        private NonTerminal __code_lines;

        private NonTerminal __domain_style_name;

        private NonTerminal __end_of_using_statement;

        private NonTerminal __main_object_statement;

        private NonTerminal __object_body_code_one_line;

        private NonTerminal __object_body_element;

        private NonTerminal __object_body_in_curly_brackets;

        private NonTerminal __object_name;

        private NonTerminal __object_name_key_prefix;

        private NonTerminal __object_name_key_prefix_optional;

        private NonTerminal __object_name_optional;

        private NonTerminal __object_statement;

        private NonTerminal __property_set_statement;

        private NonTerminal __using_statement;

        private NonTerminal __using_statement_collection;

    }
}
namespace Samples.Irony.AmmyGrammar.Ast
{
    /// <summary>
    /// sequence of __using_statement_collection  [Samples.Irony.AmmyGrammar.Ast.AstUsingStatementCollection, Samples.Irony.AmmyGrammar.Data.AmmyUsingStatementCollection], __main_object_statement  [Samples.Irony.AmmyGrammar.Ast.AstMainObjectStatement, Samples.Irony.AmmyGrammar.Data.AmmyMainObjectStatement]
    /// </summary>
    public partial class AstAmmyProgram : BaseAstNode
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            AsString = "Sequence ammy_program";
            Usings = (AstUsingStatementCollection)ChildNodes[0];
            ObjectDefinition = (AstMainObjectStatement)ChildNodes[1];
        }

        protected override int[] GetMap()
        {
            return new [] { 0, 1 };
        }

        /// <summary>
        /// Index = 0
        /// </summary>
        public AstUsingStatementCollection Usings { get; private set; }

        /// <summary>
        /// Index = 1
        /// </summary>
        public AstMainObjectStatement ObjectDefinition { get; private set; }

    }

    /// <summary>
    /// rule = alternative: __stringLiteral, __numberLiteral, __boolean_value
    /// </summary>
    public partial class AstAmmyValue : BaseAstNode
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
        }

        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

        public Samples.Irony.AmmyGrammar.Ast.IAstAmmyValue Value { get; private set; }

    }

    /// <summary>
    /// rule = alternative: __true, __false
    /// </summary>
    public partial class AstBooleanValue : BaseAstNode
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
        }

        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

        public Samples.Irony.AmmyGrammar.Ast.IAstBooleanValue Value { get; private set; }

    }

    /// <summary>
    /// zero of more __object_body_code_one_line
    /// </summary>
    public partial class AstCodeLines : BaseAstNode
    {
        public IReadOnlyList<Samples.Irony.AmmyGrammar.Data.AmmyObjectBodyCodeOneLine> EvaluateItems(ScriptThread thread)
        {
            var cnt = ChildNodes.Count;
            var result = new Samples.Irony.AmmyGrammar.Data.AmmyObjectBodyCodeOneLine[cnt];
            for (var i = cnt - 1; i >= 0; i--)
            {
                var childNode = ChildNodes[i];
                result[i] = (Samples.Irony.AmmyGrammar.Data.AmmyObjectBodyCodeOneLine)childNode.Evaluate(thread);
            }
            return result;
        }

        public IEnumerable<AstObjectBodyCodeOneLine> GetItems()
        {
            var cnt = ChildNodes.Count;
            for (var i = 0; i < cnt; i++)
            {
                var el = ChildNodes[i];
                yield return (AstObjectBodyCodeOneLine)el;
            }
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            AsString = "Collection of object_body_code_one_line";
        }

    }

    /// <summary>
    /// zero of more __identifierIdentifierTerminal, string
    /// </summary>
    public partial class AstDomainStyleName : BaseAstNode
    {
        public IReadOnlyList<string> EvaluateItems()
        {
            var cnt = ChildNodes.Count;
            var result = new string[cnt];
            for (var i = cnt - 1; i >= 0; i--)
            {
                var childNode = ChildNodes[i];
                result[i] = ((IdentifierNode)childNode).Symbol;
            }
            return result;
        }

        public IEnumerable<IdentifierNode> GetItems()
        {
            var cnt = ChildNodes.Count;
            for (var i = 0; i < cnt; i++)
            {
                var el = ChildNodes[i];
                yield return (IdentifierNode)el;
            }
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            AsString = "Collection of identifier";
        }

    }

    /// <summary>
    /// rule = alternative: NewLinePlus, Eof
    /// </summary>
    public partial class AstEndOfUsingStatement : BaseAstNode
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
        }

        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

        public Samples.Irony.AmmyGrammar.Ast.IAstEndOfUsingStatement Value { get; private set; }

    }

    /// <summary>
    /// sequence of __domain_style_name  [Samples.Irony.AmmyGrammar.Ast.AstDomainStyleName, Samples.Irony.AmmyGrammar.Data.AmmyDomainStyleName], __stringLiteral  [StringLiteral, string]
    /// </summary>
    public partial class AstMainObjectStatement : BaseAstNode
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            AsString = "Sequence main_object_statement";
            BaseObjectType = (AstDomainStyleName)ChildNodes[0];
            FullTypeName = (LiteralValueNode)ChildNodes[1];
        }

        protected override int[] GetMap()
        {
            return new [] { 0, 1 };
        }

        /// <summary>
        /// Index = 0
        /// </summary>
        public AstDomainStyleName BaseObjectType { get; private set; }

        /// <summary>
        /// Index = 1
        /// </summary>
        public LiteralValueNode FullTypeName { get; private set; }

    }

    /// <summary>
    /// one of more __object_body_element
    /// </summary>
    public partial class AstObjectBodyCodeOneLine : BaseAstNode
    {
        public IReadOnlyList<Samples.Irony.AmmyGrammar.Data.AmmyObjectBodyElement> EvaluateItems(ScriptThread thread)
        {
            var cnt = ChildNodes.Count;
            var result = new Samples.Irony.AmmyGrammar.Data.AmmyObjectBodyElement[cnt];
            for (var i = cnt - 1; i >= 0; i--)
            {
                var childNode = ChildNodes[i];
                result[i] = (Samples.Irony.AmmyGrammar.Data.AmmyObjectBodyElement)childNode.Evaluate(thread);
            }
            return result;
        }

        public IEnumerable<AstObjectBodyElement> GetItems()
        {
            var cnt = ChildNodes.Count;
            for (var i = 0; i < cnt; i++)
            {
                var el = ChildNodes[i];
                yield return (AstObjectBodyElement)el;
            }
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            AsString = "Collection of object_body_element";
        }

    }

    /// <summary>
    /// rule = alternative: __property_set_statement, __ammy_value
    /// </summary>
    public partial class AstObjectBodyElement : BaseAstNode
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
        }

        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

        public Samples.Irony.AmmyGrammar.Ast.IAstObjectBodyElement Value { get; private set; }

    }

    /// <summary>
    /// sequence of __code_lines  [Samples.Irony.AmmyGrammar.Ast.AstCodeLines, Samples.Irony.AmmyGrammar.Data.AmmyCodeLines]
    /// </summary>
    public partial class AstObjectBodyInCurlyBrackets : BaseAstNode
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            AsString = "Sequence object_body_in_curly_brackets";
            Lines = (AstCodeLines)ChildNodes[1];
        }

        protected override int[] GetMap()
        {
            return new [] { 1 };
        }

        /// <summary>
        /// Index = 1
        /// </summary>
        public AstCodeLines Lines { get; private set; }

    }

    /// <summary>
    /// sequence of
    /// </summary>
    public partial class AstObjectName : BaseAstNode
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            AsString = "Sequence object_name";
        }

    }

    /// <summary>
    /// sequence of
    /// </summary>
    public partial class AstObjectNameKeyPrefix : BaseAstNode
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            AsString = "Sequence object_name_key_prefix";
        }

    }

    /// <summary>
    /// rule = alternative: Empty, __object_name_key_prefix
    /// </summary>
    public partial class AstObjectNameKeyPrefixOptional : BaseAstNode
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
        }

        public AstObjectNameKeyPrefix Optional { get; private set; }

    }

    /// <summary>
    /// rule = alternative: Empty, __object_name
    /// </summary>
    public partial class AstObjectNameOptional : BaseAstNode
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
        }

        public AstObjectName Optional { get; private set; }

    }

    /// <summary>
    /// sequence of __domain_style_name  [Samples.Irony.AmmyGrammar.Ast.AstDomainStyleName, Samples.Irony.AmmyGrammar.Data.AmmyDomainStyleName], __object_name_optional  [Samples.Irony.AmmyGrammar.Ast.AstObjectNameOptional, Samples.Irony.AmmyGrammar.Data.AmmyObjectNameOptional]
    /// </summary>
    public partial class AstObjectStatement : BaseAstNode
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            AsString = "Sequence object_statement";
            ObjectType = (AstDomainStyleName)ChildNodes[0];
            ObjectName = (AstObjectNameOptional)ChildNodes[1];
        }

        protected override int[] GetMap()
        {
            return new [] { 0, 1 };
        }

        /// <summary>
        /// Index = 0
        /// </summary>
        public AstDomainStyleName ObjectType { get; private set; }

        /// <summary>
        /// Index = 1
        /// </summary>
        public AstObjectNameOptional ObjectName { get; private set; }

    }

    /// <summary>
    /// sequence of __identifier  [IdentifierTerminal, string]
    /// </summary>
    public partial class AstPropertySetStatement : BaseAstNode
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            AsString = "Sequence property_set_statement";
            PropertyName = (IdentifierNode)ChildNodes[0];
        }

        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

        /// <summary>
        /// Index = 0
        /// </summary>
        public IdentifierNode PropertyName { get; private set; }

    }

    /// <summary>
    /// sequence of __domain_style_name  [Samples.Irony.AmmyGrammar.Ast.AstDomainStyleName, Samples.Irony.AmmyGrammar.Data.AmmyDomainStyleName]
    /// </summary>
    public partial class AstUsingStatement : BaseAstNode
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            AsString = "Sequence using_statement";
            NamespaceName = (AstDomainStyleName)ChildNodes[1];
        }

        protected override int[] GetMap()
        {
            return new [] { 1 };
        }

        /// <summary>
        /// Index = 1
        /// </summary>
        public AstDomainStyleName NamespaceName { get; private set; }

    }

    /// <summary>
    /// zero of more __using_statement
    /// </summary>
    public partial class AstUsingStatementCollection : BaseAstNode
    {
        public IReadOnlyList<Samples.Irony.AmmyGrammar.Data.AmmyUsingStatement> EvaluateItems(ScriptThread thread)
        {
            var cnt = ChildNodes.Count;
            var result = new Samples.Irony.AmmyGrammar.Data.AmmyUsingStatement[cnt];
            for (var i = cnt - 1; i >= 0; i--)
            {
                var childNode = ChildNodes[i];
                result[i] = (Samples.Irony.AmmyGrammar.Data.AmmyUsingStatement)childNode.Evaluate(thread);
            }
            return result;
        }

        public IEnumerable<AstUsingStatement> GetItems()
        {
            var cnt = ChildNodes.Count;
            for (var i = 0; i < cnt; i++)
            {
                var el = ChildNodes[i];
                yield return (AstUsingStatement)el;
            }
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            AsString = "Collection of using_statement";
        }

    }

    /// <summary>
    /// one of: stringLiteral, numberLiteral, boolean_value
    /// </summary>
    public partial interface IAstAmmyValue
    {
    }

    /// <summary>
    /// one of: true, false
    /// </summary>
    public partial interface IAstBooleanValue
    {
    }

    /// <summary>
    /// one of: NewLinePlus, Eof
    /// </summary>
    public partial interface IAstEndOfUsingStatement
    {
    }

    /// <summary>
    /// one of: property_set_statement, ammy_value
    /// </summary>
    public partial interface IAstObjectBodyElement
    {
    }

    /// <summary>
    /// optional Name = object_name_key_prefix
    /// </summary>
    public partial interface IAstObjectNameKeyPrefixOptional
    {
    }

    /// <summary>
    /// optional Name = object_name
    /// </summary>
    public partial interface IAstObjectNameOptional
    {
    }
}
namespace Samples.Irony.AmmyGrammar.Data
{
    /// <summary>
    /// rule = alternative: __true, __false
    /// </summary>
    public partial class AmmyBooleanValue : AmmyStatement
    {
        public AmmyBooleanValue(SourceSpan span)
            : base(span)
        {
        }

    }

    /// <summary>
    /// zero of more __object_body_code_one_line
    /// </summary>
    public partial class AmmyCodeLines : AmmyStatement
    {
        public AmmyCodeLines(SourceSpan span, IReadOnlyList<AmmyObjectBodyCodeOneLine> items)
            : base(span)
        {
            Items = items;
        }

        public IReadOnlyList<AmmyObjectBodyCodeOneLine> Items { get; }

    }

    /// <summary>
    /// zero of more __identifierIdentifierTerminal, string
    /// </summary>
    public partial class AmmyDomainStyleName : AmmyStatement
    {
        public AmmyDomainStyleName(SourceSpan span, IReadOnlyList<string> items)
            : base(span)
        {
            Items = items;
        }

        public IReadOnlyList<string> Items { get; }

    }

    /// <summary>
    /// sequence of __domain_style_name  [Samples.Irony.AmmyGrammar.Ast.AstDomainStyleName, Samples.Irony.AmmyGrammar.Data.AmmyDomainStyleName], __stringLiteral  [StringLiteral, string]
    /// </summary>
    public partial class AmmyMainObjectStatement : AmmyStatement
    {
        public AmmyMainObjectStatement(SourceSpan span, AmmyDomainStyleName baseObjectType, string fullTypeName)
            : base(span)
        {
            BaseObjectType = baseObjectType;
            FullTypeName = fullTypeName;
        }

        public AmmyDomainStyleName BaseObjectType { get; }

        public string FullTypeName { get; }

    }

    /// <summary>
    /// one of more __object_body_element
    /// </summary>
    public partial class AmmyObjectBodyCodeOneLine : AmmyStatement
    {
        public AmmyObjectBodyCodeOneLine(SourceSpan span, IReadOnlyList<AmmyObjectBodyElement> items)
            : base(span)
        {
            Items = items;
        }

        public IReadOnlyList<AmmyObjectBodyElement> Items { get; }

    }

    /// <summary>
    /// rule = alternative: __property_set_statement, __ammy_value
    /// </summary>
    public partial class AmmyObjectBodyElement : AmmyStatement
    {
        public AmmyObjectBodyElement(SourceSpan span)
            : base(span)
        {
        }

    }

    /// <summary>
    /// sequence of __code_lines  [Samples.Irony.AmmyGrammar.Ast.AstCodeLines, Samples.Irony.AmmyGrammar.Data.AmmyCodeLines]
    /// </summary>
    public partial class AmmyObjectBodyInCurlyBrackets : AmmyStatement
    {
        public AmmyObjectBodyInCurlyBrackets(SourceSpan span, AmmyCodeLines lines)
            : base(span)
        {
            Lines = lines;
        }

        public AmmyCodeLines Lines { get; }

    }

    /// <summary>
    /// sequence of
    /// </summary>
    public partial class AmmyObjectName : AmmyStatement
    {
        public AmmyObjectName(SourceSpan span)
            : base(span)
        {
        }

    }

    /// <summary>
    /// sequence of
    /// </summary>
    public partial class AmmyObjectNameKeyPrefix : AmmyStatement
    {
        public AmmyObjectNameKeyPrefix(SourceSpan span)
            : base(span)
        {
        }

    }

    /// <summary>
    /// rule = alternative: Empty, __object_name_key_prefix
    /// </summary>
    public partial class AmmyObjectNameKeyPrefixOptional : AmmyStatement
    {
        public AmmyObjectNameKeyPrefixOptional(SourceSpan span)
            : base(span)
        {
        }

    }

    /// <summary>
    /// rule = alternative: Empty, __object_name
    /// </summary>
    public partial class AmmyObjectNameOptional : AmmyStatement
    {
        public AmmyObjectNameOptional(SourceSpan span)
            : base(span)
        {
        }

    }

    /// <summary>
    /// sequence of __domain_style_name  [Samples.Irony.AmmyGrammar.Ast.AstDomainStyleName, Samples.Irony.AmmyGrammar.Data.AmmyDomainStyleName], __object_name_optional  [Samples.Irony.AmmyGrammar.Ast.AstObjectNameOptional, Samples.Irony.AmmyGrammar.Data.AmmyObjectNameOptional]
    /// </summary>
    public partial class AmmyObjectStatement : AmmyStatement
    {
        public AmmyObjectStatement(SourceSpan span, AmmyDomainStyleName objectType, AmmyObjectNameOptional objectName)
            : base(span)
        {
            ObjectType = objectType;
            ObjectName = objectName;
        }

        public AmmyDomainStyleName ObjectType { get; }

        public AmmyObjectNameOptional ObjectName { get; }

    }

    /// <summary>
    /// sequence of __using_statement_collection  [Samples.Irony.AmmyGrammar.Ast.AstUsingStatementCollection, Samples.Irony.AmmyGrammar.Data.AmmyUsingStatementCollection], __main_object_statement  [Samples.Irony.AmmyGrammar.Ast.AstMainObjectStatement, Samples.Irony.AmmyGrammar.Data.AmmyMainObjectStatement]
    /// </summary>
    public partial class AmmyProgram : AmmyStatement
    {
        public AmmyProgram(SourceSpan span, AmmyUsingStatementCollection usings, AmmyMainObjectStatement objectDefinition)
            : base(span)
        {
            Usings = usings;
            ObjectDefinition = objectDefinition;
        }

        public AmmyUsingStatementCollection Usings { get; }

        public AmmyMainObjectStatement ObjectDefinition { get; }

    }

    /// <summary>
    /// sequence of __identifier  [IdentifierTerminal, string]
    /// </summary>
    public partial class AmmyPropertySetStatement : AmmyStatement
    {
        public AmmyPropertySetStatement(SourceSpan span, string propertyName)
            : base(span)
        {
            PropertyName = propertyName;
        }

        public string PropertyName { get; }

    }

    /// <summary>
    /// sequence of __domain_style_name  [Samples.Irony.AmmyGrammar.Ast.AstDomainStyleName, Samples.Irony.AmmyGrammar.Data.AmmyDomainStyleName]
    /// </summary>
    public partial class AmmyUsingStatement : AmmyStatement
    {
        public AmmyUsingStatement(SourceSpan span, AmmyDomainStyleName namespaceName)
            : base(span)
        {
            NamespaceName = namespaceName;
        }

        public AmmyDomainStyleName NamespaceName { get; }

    }

    /// <summary>
    /// zero of more __using_statement
    /// </summary>
    public partial class AmmyUsingStatementCollection : AmmyStatement
    {
        public AmmyUsingStatementCollection(SourceSpan span, IReadOnlyList<AmmyUsingStatement> items)
            : base(span)
        {
            Items = items;
        }

        public IReadOnlyList<AmmyUsingStatement> Items { get; }

    }

    /// <summary>
    /// rule = alternative: __stringLiteral, __numberLiteral, __boolean_value
    /// </summary>
    public partial class AmmyValue : AmmyStatement
    {
        public AmmyValue(SourceSpan span)
            : base(span)
        {
        }

    }
}
