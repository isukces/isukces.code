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
            // generator : IronyAutocodeGenerator.Add_AutoInit:164
            // init 1
            __identifier = TerminalFactory.CreateCSharpIdentifier("identifier");
            __stringLiteral = TerminalFactory.CreateCSharpString("stringLiteral");
            __numberLiteral = TerminalFactory.CreateCSharpNumber("numberLiteral");
            __numberLiteral.Options = NumberOptions.AllowSign
                | NumberOptions.AllowUnderscore;
            __end_of_using_statement = new NonTerminal("end_of_using_statement", typeof(Samples.Irony.AmmyGrammar.Ast.BaseAstNode));
            __domain_style_name = new NonTerminal("domain_style_name", typeof(Samples.Irony.AmmyGrammar.Ast.AstDomainStyleName));
            __using_statement = new NonTerminal("using_statement", typeof(Samples.Irony.AmmyGrammar.Ast.AstUsingStatement));
            __using_statement_collection = new NonTerminal("using_statement_collection", typeof(Samples.Irony.AmmyGrammar.Ast.AstUsingStatementCollection));
            __boolean_value = new NonTerminal("boolean_value", typeof(Samples.Irony.AmmyGrammar.Ast.AstBooleanValue));
            __ammy_value = new NonTerminal("ammy_value", typeof(Samples.Irony.AmmyGrammar.Ast.AstAmmyValue));
            __property_set_statement = new NonTerminal("property_set_statement", typeof(Samples.Irony.AmmyGrammar.Ast.AstPropertySetStatement));
            __object_body_element = new NonTerminal("object_body_element", typeof(Samples.Irony.AmmyGrammar.Ast.AstObjectBodyElement));
            __object_body_one_line = new NonTerminal("object_body_one_line", typeof(Samples.Irony.AmmyGrammar.Ast.AstObjectBodyOneLine));
            __object_body = new NonTerminal("object_body", typeof(Samples.Irony.AmmyGrammar.Ast.AstObjectBody));
            __object_body_in_curly_brackets = new NonTerminal("object_body_in_curly_brackets", typeof(Samples.Irony.AmmyGrammar.Ast.AstObjectBodyInCurlyBrackets));
            __main_object_statement = new NonTerminal("main_object_statement", typeof(Samples.Irony.AmmyGrammar.Ast.AstMainObjectStatement));
            __object_name_key_prefix = new NonTerminal("object_name_key_prefix", typeof(Samples.Irony.AmmyGrammar.Ast.AstObjectNameKeyPrefix));
            __object_name_key_prefix_optional = new NonTerminal("object_name_key_prefix_optional", typeof(Samples.Irony.AmmyGrammar.Ast.AstObjectNameKeyPrefixOptional));
            __object_name = new NonTerminal("object_name", typeof(Samples.Irony.AmmyGrammar.Ast.AstObjectName));
            __object_name_optional = new NonTerminal("object_name_optional", typeof(Samples.Irony.AmmyGrammar.Ast.AstObjectNameOptional));
            __object_statement = new NonTerminal("object_statement", typeof(Samples.Irony.AmmyGrammar.Ast.AstObjectStatement));
            __ammy_program = new NonTerminal("ammy_program", typeof(Samples.Irony.AmmyGrammar.Ast.AstAmmyProgram));
            // == init Terminals
            __comma = ToTerm(",");
            __dot = ToTerm(".");
            __doubledot = ToTerm(":");
            __parOpen = ToTerm("(");
            __parClose = ToTerm(")");
            __curlyOpen = ToTerm("{");
            __curlyClose = ToTerm("}");
            __squareOpen = ToTerm("[");
            __squareClose = ToTerm("]");
            __true = ToTerm("true");
            __false = ToTerm("false");
            __using = ToTerm("using");
            __key = ToTerm("Key");
            // == init NonTerminals
            __end_of_using_statement.Rule = NewLinePlus | Eof;
            __domain_style_name.Rule = MakeStarRule(__domain_style_name, __dot, __identifier);
            __using_statement.Rule = ToTerm("using") + __domain_style_name + __end_of_using_statement;
            __using_statement_collection.Rule = MakeStarRule(__using_statement_collection, null, __using_statement);
            __boolean_value.Rule = __true | __false;
            __ammy_value.Rule = __stringLiteral | __numberLiteral | __boolean_value;
            __property_set_statement.Rule = __identifier + __doubledot + __ammy_value;
            __object_body_element.Rule = __property_set_statement | __ammy_value;
            __object_body_one_line.Rule = MakePlusRule(__object_body_one_line, __comma, __object_body_element);
            __object_body.Rule = MakeListRule(__object_body, NewLinePlus, __object_body_one_line, TermListOptions.StarList | TermListOptions.AllowTrailingDelimiter);
            __object_body_in_curly_brackets.Rule = ToTerm("{") + __object_body + ToTerm("}");
            __main_object_statement.Rule = __domain_style_name + __stringLiteral + this.PreferShiftHere() + __object_body_in_curly_brackets;
            __object_name_key_prefix.Rule = ToTerm("Key") + ToTerm("=");
            __object_name_key_prefix_optional.Rule = Empty | __object_name_key_prefix;
            __object_name.Rule = __object_name_key_prefix_optional + __stringLiteral;
            __object_name_optional.Rule = Empty | __object_name;
            __object_statement.Rule = __domain_style_name + __object_name_optional + __object_body_in_curly_brackets;
            __ammy_program.Rule = __using_statement_collection + __main_object_statement + NewLineStar;
            // == brackets
            RegisterBracePair("(", ")");
            RegisterBracePair("{", "}");
            RegisterBracePair("[", "]");
            // == mark reserved words
            MarkReservedWords("true", "false", "using", "Key");
            // == mark punctuations
            MarkPunctuation(",", ".", ":");
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

        private KeyTerm __key;

        private KeyTerm __parClose;

        private KeyTerm __parOpen;

        private KeyTerm __squareClose;

        private KeyTerm __squareOpen;

        private KeyTerm __true;

        private KeyTerm __using;

        private NonTerminal __ammy_program;

        private NonTerminal __ammy_value;

        private NonTerminal __boolean_value;

        private NonTerminal __domain_style_name;

        private NonTerminal __end_of_using_statement;

        private NonTerminal __main_object_statement;

        private NonTerminal __object_body;

        private NonTerminal __object_body_element;

        private NonTerminal __object_body_in_curly_brackets;

        private NonTerminal __object_body_one_line;

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
    using Samples.Irony.AmmyGrammar.Data;

    /// <summary>
    /// sequence of __using_statement_collection  [AstUsingStatementCollection, AmmyUsingStatementCollection], __main_object_statement  [AstMainObjectStatement, AmmyMainObjectStatement]
    /// </summary>
    public partial class AstAmmyProgram : BaseAstNode
    {
        public AmmyMainObjectStatement GetObjectDefinitionValue(ScriptThread thread)
        {
            // generator : AstClassesGenerator.Process_SequenceRule:335
            var tmp = ObjectDefinition.Evaluate(thread);
            var result = (AmmyMainObjectStatement)tmp;
            return result;
        }

        public AmmyUsingStatementCollection GetUsingsValue(ScriptThread thread)
        {
            // generator : AstClassesGenerator.Process_SequenceRule:335
            var tmp = Usings.Evaluate(thread);
            var result = (AmmyUsingStatementCollection)tmp;
            return result;
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            AsString = "Sequence ammy_program";
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            // generator : ForSequenceRule.Create:44
            var usings = GetUsingsValue(thread);
            var objectDefinition = GetObjectDefinitionValue(thread);
            var doEvaluateResult = new AmmyProgram(Span, usings, objectDefinition);
            return doEvaluateResult;
        }

        // created AstClassesGenerator.Add_GetMap:96
        protected override int[] GetMap()
        {
            return new [] { 0, 1 };
        }

        // created AstClassesGenerator.Process_SequenceRule:314
        /// <summary>
        /// Index = 0
        /// </summary>
        public AstUsingStatementCollection Usings
        {
            get { return (AstUsingStatementCollection)ChildNodes[0]; }
        }

        // created AstClassesGenerator.Process_SequenceRule:314
        /// <summary>
        /// Index = 1
        /// </summary>
        public AstMainObjectStatement ObjectDefinition
        {
            get { return (AstMainObjectStatement)ChildNodes[1]; }
        }

    }

    /// <summary>
    /// rule = alternative: __stringLiteral, __numberLiteral, __boolean_value
    /// </summary>
    public partial class AstAmmyValue : BaseAstNode
    {
        public AstAmmyValueNodeKinds GetNodeKind()
        {
            // generator : AstClassesGenerator.Process_Alternative:203
            switch (OptionNode)
            {
                // AstType = StringLiteral
                // DataType = string
                // NodeType = LiteralValueNode
                case LiteralValueNode _:
                    return AstAmmyValueNodeKinds.StringLiteral;
                // AstType = NumberLiteral
                // DataType = int
                // NodeType = FakeCSharpNumber
                case FakeCSharpNumber _:
                    return AstAmmyValueNodeKinds.NumberLiteral;
                // AstType = AstBooleanValue
                // DataType = AmmyBooleanValue
                // NodeType = AstBooleanValue
                case AstBooleanValue _:
                    return AstAmmyValueNodeKinds.BooleanValue;
            }
            return AstAmmyValueNodeKinds.Unknown;
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            // generator : ForAlternative.Create:44
            var altValue = base.DoEvaluate(thread);
            var nodeKind = GetNodeKind();
            var doEvaluateResult = new AmmyValue(Span, altValue, nodeKind);
            return doEvaluateResult;
        }

        // created AstClassesGenerator.Add_GetMap:96
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

        // created AstClassesGenerator.Process_Alternative:182
        public AstNode OptionNode
        {
            get { return ChildNodes[0]; }
        }

    }

    /// <summary>
    /// rule = alternative: __true, __false
    /// </summary>
    public partial class AstBooleanValue : BaseAstNode
    {
        public AstBooleanValueNodeKinds GetNodeKind()
        {
            // generator : AstClassesGenerator.Process_Alternative:203
            switch (OptionNode)
            {
            }
            return AstBooleanValueNodeKinds.Unknown;
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            // generator : ForAlternative.Create:44
            var altValue = base.DoEvaluate(thread);
            var nodeKind = GetNodeKind();
            var doEvaluateResult = new AmmyBooleanValue(Span, altValue, nodeKind);
            return doEvaluateResult;
        }

        // created AstClassesGenerator.Add_GetMap:96
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

        // created AstClassesGenerator.Process_Alternative:182
        public AstNode OptionNode
        {
            get { return ChildNodes[0]; }
        }

    }

    /// <summary>
    /// zero or more __identifierIdentifierTerminal, string
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

        protected override object DoEvaluate(ScriptThread thread)
        {
            // generator : ForPlusOrStar.Create:44
            var items = EvaluateItems();
            var doEvaluateResult = new AmmyDomainStyleName(Span, items);
            return doEvaluateResult;
        }

    }

    /// <summary>
    /// sequence of __domain_style_name  [AstDomainStyleName, AmmyDomainStyleName], __stringLiteral  [StringLiteral, string], __object_body_in_curly_brackets  [AstObjectBodyInCurlyBrackets, AmmyObjectBodyInCurlyBrackets]
    /// </summary>
    public partial class AstMainObjectStatement : BaseAstNode
    {
        public AmmyDomainStyleName GetBaseObjectTypeValue(ScriptThread thread)
        {
            // generator : AstClassesGenerator.Process_SequenceRule:335
            var tmp = BaseObjectType.Evaluate(thread);
            var result = (AmmyDomainStyleName)tmp;
            return result;
        }

        public AmmyObjectBodyInCurlyBrackets GetBodyValue(ScriptThread thread)
        {
            // generator : AstClassesGenerator.Process_SequenceRule:335
            var tmp = Body.Evaluate(thread);
            var result = (AmmyObjectBodyInCurlyBrackets)tmp;
            return result;
        }

        public string GetFullTypeNameValue()
        {
            // generator : AstClassesGenerator.Process_SequenceRule:335
            var result = FullTypeName.Value?.ToString();
            return result;
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            AsString = "Sequence main_object_statement";
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            // generator : ForSequenceRule.Create:44
            var baseObjectType = GetBaseObjectTypeValue(thread);
            var fullTypeName = GetFullTypeNameValue();
            var body = GetBodyValue(thread);
            var doEvaluateResult = new AmmyMainObjectStatement(Span, baseObjectType, fullTypeName, body);
            return doEvaluateResult;
        }

        // created AstClassesGenerator.Add_GetMap:96
        protected override int[] GetMap()
        {
            return new [] { 0, 1, 2 };
        }

        // created AstClassesGenerator.Process_SequenceRule:314
        /// <summary>
        /// Index = 0
        /// </summary>
        public AstDomainStyleName BaseObjectType
        {
            get { return (AstDomainStyleName)ChildNodes[0]; }
        }

        // created AstClassesGenerator.Process_SequenceRule:314
        /// <summary>
        /// Index = 1
        /// </summary>
        public LiteralValueNode FullTypeName
        {
            get { return (LiteralValueNode)ChildNodes[1]; }
        }

        // created AstClassesGenerator.Process_SequenceRule:314
        /// <summary>
        /// Index = 2
        /// </summary>
        public AstObjectBodyInCurlyBrackets Body
        {
            get { return (AstObjectBodyInCurlyBrackets)ChildNodes[2]; }
        }

    }

    /// <summary>
    /// zero or more __object_body_one_line
    /// </summary>
    public partial class AstObjectBody : BaseAstNode
    {
        public IReadOnlyList<AmmyObjectBodyOneLine> EvaluateItems(ScriptThread thread)
        {
            var cnt = ChildNodes.Count;
            var result = new AmmyObjectBodyOneLine[cnt];
            for (var i = cnt - 1; i >= 0; i--)
            {
                var childNode = ChildNodes[i];
                result[i] = (AmmyObjectBodyOneLine)childNode.Evaluate(thread);
            }
            return result;
        }

        public IEnumerable<AstObjectBodyOneLine> GetItems()
        {
            var cnt = ChildNodes.Count;
            for (var i = 0; i < cnt; i++)
            {
                var el = ChildNodes[i];
                yield return (AstObjectBodyOneLine)el;
            }
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            AsString = "Collection of object_body_one_line";
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            // generator : ForPlusOrStar.Create:44
            var items = EvaluateItems(thread);
            var doEvaluateResult = new AmmyObjectBody(Span, items);
            return doEvaluateResult;
        }

    }

    /// <summary>
    /// rule = alternative: __property_set_statement, __ammy_value
    /// </summary>
    public partial class AstObjectBodyElement : BaseAstNode
    {
        public AstObjectBodyElementNodeKinds GetNodeKind()
        {
            // generator : AstClassesGenerator.Process_Alternative:203
            switch (OptionNode)
            {
                // AstType = AstPropertySetStatement
                // DataType = AmmyPropertySetStatement
                // NodeType = AstPropertySetStatement
                case AstPropertySetStatement _:
                    return AstObjectBodyElementNodeKinds.PropertySetStatement;
                // AstType = AstAmmyValue
                // DataType = AmmyValue
                // NodeType = AstAmmyValue
                case AstAmmyValue _:
                    return AstObjectBodyElementNodeKinds.AmmyValue;
            }
            return AstObjectBodyElementNodeKinds.Unknown;
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            // generator : ForAlternative.Create:44
            var altValue = base.DoEvaluate(thread);
            var nodeKind = GetNodeKind();
            var doEvaluateResult = new AmmyObjectBodyElement(Span, altValue, nodeKind);
            return doEvaluateResult;
        }

        // created AstClassesGenerator.Add_GetMap:96
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

        // created AstClassesGenerator.Process_Alternative:182
        public AstNode OptionNode
        {
            get { return ChildNodes[0]; }
        }

    }

    /// <summary>
    /// sequence of __object_body  [AstObjectBody, AmmyObjectBody]
    /// </summary>
    public partial class AstObjectBodyInCurlyBrackets : BaseAstNode
    {
        public AmmyObjectBody GetBodyValue(ScriptThread thread)
        {
            // generator : AstClassesGenerator.Process_SequenceRule:335
            var tmp = Body.Evaluate(thread);
            var result = (AmmyObjectBody)tmp;
            return result;
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            AsString = "Sequence object_body_in_curly_brackets";
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            // generator : ForSequenceRule.Create:44
            var body = GetBodyValue(thread);
            var doEvaluateResult = new AmmyObjectBodyInCurlyBrackets(Span, body);
            return doEvaluateResult;
        }

        // created AstClassesGenerator.Add_GetMap:96
        protected override int[] GetMap()
        {
            return new [] { 1 };
        }

        // created AstClassesGenerator.Process_SequenceRule:314
        /// <summary>
        /// Index = 1
        /// </summary>
        public AstObjectBody Body
        {
            get { return (AstObjectBody)ChildNodes[1]; }
        }

    }

    /// <summary>
    /// one or more __object_body_element
    /// </summary>
    public partial class AstObjectBodyOneLine : BaseAstNode
    {
        public IReadOnlyList<AmmyObjectBodyElement> EvaluateItems(ScriptThread thread)
        {
            var cnt = ChildNodes.Count;
            var result = new AmmyObjectBodyElement[cnt];
            for (var i = cnt - 1; i >= 0; i--)
            {
                var childNode = ChildNodes[i];
                result[i] = (AmmyObjectBodyElement)childNode.Evaluate(thread);
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

        protected override object DoEvaluate(ScriptThread thread)
        {
            // generator : ForPlusOrStar.Create:44
            var items = EvaluateItems(thread);
            var doEvaluateResult = new AmmyObjectBodyOneLine(Span, items);
            return doEvaluateResult;
        }

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

        protected override object DoEvaluate(ScriptThread thread)
        {
            // generator : ForSequenceRule.Create:44
            var doEvaluateResult = new AmmyObjectName(Span);
            return doEvaluateResult;
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

        protected override object DoEvaluate(ScriptThread thread)
        {
            // generator : ForSequenceRule.Create:44
            var doEvaluateResult = new AmmyObjectNameKeyPrefix(Span);
            return doEvaluateResult;
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

        // created AstClassesGenerator.Process_OptionAlternative:239
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

        // created AstClassesGenerator.Process_OptionAlternative:239
        public AstObjectName Optional { get; private set; }

    }

    /// <summary>
    /// sequence of __domain_style_name  [AstDomainStyleName, AmmyDomainStyleName], __object_name_optional  [AstObjectNameOptional, AmmyObjectNameOptional]
    /// </summary>
    public partial class AstObjectStatement : BaseAstNode
    {
        public AmmyObjectNameOptional GetObjectNameValue(ScriptThread thread)
        {
            // generator : AstClassesGenerator.Process_SequenceRule:335
            var tmp = ObjectName.Evaluate(thread);
            var result = (AmmyObjectNameOptional)tmp;
            return result;
        }

        public AmmyDomainStyleName GetObjectTypeValue(ScriptThread thread)
        {
            // generator : AstClassesGenerator.Process_SequenceRule:335
            var tmp = ObjectType.Evaluate(thread);
            var result = (AmmyDomainStyleName)tmp;
            return result;
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            AsString = "Sequence object_statement";
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            // generator : ForSequenceRule.Create:44
            var objectType = GetObjectTypeValue(thread);
            var objectName = GetObjectNameValue(thread);
            var doEvaluateResult = new AmmyObjectStatement(Span, objectType, objectName);
            return doEvaluateResult;
        }

        // created AstClassesGenerator.Add_GetMap:96
        protected override int[] GetMap()
        {
            return new [] { 0, 1 };
        }

        // created AstClassesGenerator.Process_SequenceRule:314
        /// <summary>
        /// Index = 0
        /// </summary>
        public AstDomainStyleName ObjectType
        {
            get { return (AstDomainStyleName)ChildNodes[0]; }
        }

        // created AstClassesGenerator.Process_SequenceRule:314
        /// <summary>
        /// Index = 1
        /// </summary>
        public AstObjectNameOptional ObjectName
        {
            get { return (AstObjectNameOptional)ChildNodes[1]; }
        }

    }

    /// <summary>
    /// sequence of __identifier  [IdentifierTerminal, string], __ammy_value  [AstAmmyValue, AmmyValue]
    /// </summary>
    public partial class AstPropertySetStatement : BaseAstNode
    {
        public string GetPropertyNameValue()
        {
            // generator : AstClassesGenerator.Process_SequenceRule:335
            var result = PropertyName.Symbol;
            return result;
        }

        public AmmyValue GetPropertyValueValue(ScriptThread thread)
        {
            // generator : AstClassesGenerator.Process_SequenceRule:335
            var tmp = PropertyValue.Evaluate(thread);
            var result = (AmmyValue)tmp;
            return result;
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            AsString = "Sequence property_set_statement";
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            // generator : ForSequenceRule.Create:44
            var propertyName = GetPropertyNameValue();
            var propertyValue = GetPropertyValueValue(thread);
            var doEvaluateResult = new AmmyPropertySetStatement(Span, propertyName, propertyValue);
            return doEvaluateResult;
        }

        // created AstClassesGenerator.Add_GetMap:96
        protected override int[] GetMap()
        {
            return new [] { 0, 1 };
        }

        // created AstClassesGenerator.Process_SequenceRule:314
        /// <summary>
        /// Index = 0
        /// </summary>
        public IdentifierNode PropertyName
        {
            get { return (IdentifierNode)ChildNodes[0]; }
        }

        // created AstClassesGenerator.Process_SequenceRule:314
        /// <summary>
        /// Index = 1
        /// </summary>
        public AstAmmyValue PropertyValue
        {
            get { return (AstAmmyValue)ChildNodes[1]; }
        }

    }

    /// <summary>
    /// sequence of __domain_style_name  [AstDomainStyleName, AmmyDomainStyleName]
    /// </summary>
    public partial class AstUsingStatement : BaseAstNode
    {
        public AmmyDomainStyleName GetNamespaceNameValue(ScriptThread thread)
        {
            // generator : AstClassesGenerator.Process_SequenceRule:335
            var tmp = NamespaceName.Evaluate(thread);
            var result = (AmmyDomainStyleName)tmp;
            return result;
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            AsString = "Sequence using_statement";
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            // generator : ForSequenceRule.Create:44
            var namespaceName = GetNamespaceNameValue(thread);
            var doEvaluateResult = new AmmyUsingStatement(Span, namespaceName);
            return doEvaluateResult;
        }

        // created AstClassesGenerator.Add_GetMap:96
        protected override int[] GetMap()
        {
            return new [] { 1 };
        }

        // created AstClassesGenerator.Process_SequenceRule:314
        /// <summary>
        /// Index = 1
        /// </summary>
        public AstDomainStyleName NamespaceName
        {
            get { return (AstDomainStyleName)ChildNodes[1]; }
        }

    }

    /// <summary>
    /// zero or more __using_statement
    /// </summary>
    public partial class AstUsingStatementCollection : BaseAstNode
    {
        public IReadOnlyList<AmmyUsingStatement> EvaluateItems(ScriptThread thread)
        {
            var cnt = ChildNodes.Count;
            var result = new AmmyUsingStatement[cnt];
            for (var i = cnt - 1; i >= 0; i--)
            {
                var childNode = ChildNodes[i];
                result[i] = (AmmyUsingStatement)childNode.Evaluate(thread);
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

        protected override object DoEvaluate(ScriptThread thread)
        {
            // generator : ForPlusOrStar.Create:44
            var items = EvaluateItems(thread);
            var doEvaluateResult = new AmmyUsingStatementCollection(Span, items);
            return doEvaluateResult;
        }

    }

    /// <summary>
    /// one of: stringLiteral, numberLiteral, boolean_value
    /// </summary>
    public partial interface IAstAmmyValue
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

    public enum AstBooleanValueNodeKinds
    {
        Unknown,
        True,
        False
    }

    public enum AstAmmyValueNodeKinds
    {
        Unknown,
        StringLiteral,
        NumberLiteral,
        BooleanValue
    }

    public enum AstObjectBodyElementNodeKinds
    {
        Unknown,
        PropertySetStatement,
        AmmyValue
    }
}
namespace Samples.Irony.AmmyGrammar.Data
{
    /// <summary>
    /// rule = alternative: __true, __false
    /// </summary>
    public partial class AmmyBooleanValue : AmmyStatement
    {
        public AmmyBooleanValue(SourceSpan span, object tmpValue, Samples.Irony.AmmyGrammar.Ast.AstBooleanValueNodeKinds nodeKind)
            : base(span)
        {
            // generator : ConstructorBuilder.CreateConstructor:37
            TmpValue = tmpValue;
            NodeKind = nodeKind;
        }

        public override string ToString()
        {
            return TmpValue?.ToString() ?? string.Empty;
        }

        public object TmpValue { get; set; }

        public Samples.Irony.AmmyGrammar.Ast.AstBooleanValueNodeKinds NodeKind { get; }

    }

    /// <summary>
    /// zero or more __identifierIdentifierTerminal, string
    /// </summary>
    public partial class AmmyDomainStyleName : AmmyStatement
    {
        public AmmyDomainStyleName(SourceSpan span, IReadOnlyList<string> items)
            : base(span)
        {
            // generator : ConstructorBuilder.CreateConstructor:37
            Items = items;
        }

        public override string ToString()
        {
            return string.Join(".", Items);
        }

        public IReadOnlyList<string> Items { get; }

    }

    /// <summary>
    /// sequence of __domain_style_name  [Samples.Irony.AmmyGrammar.Ast.AstDomainStyleName, AmmyDomainStyleName], __stringLiteral  [StringLiteral, string], __object_body_in_curly_brackets  [Samples.Irony.AmmyGrammar.Ast.AstObjectBodyInCurlyBrackets, AmmyObjectBodyInCurlyBrackets]
    /// </summary>
    public partial class AmmyMainObjectStatement : AmmyStatement
    {
        public AmmyMainObjectStatement(SourceSpan span, AmmyDomainStyleName baseObjectType, string fullTypeName, AmmyObjectBodyInCurlyBrackets body)
            : base(span)
        {
            // generator : ConstructorBuilder.CreateConstructor:37
            BaseObjectType = baseObjectType;
            FullTypeName = fullTypeName;
            Body = body;
        }

        public AmmyDomainStyleName BaseObjectType { get; }

        public string FullTypeName { get; }

        public AmmyObjectBodyInCurlyBrackets Body { get; }

    }

    /// <summary>
    /// zero or more __object_body_one_line
    /// </summary>
    public partial class AmmyObjectBody : AmmyStatement
    {
        public AmmyObjectBody(SourceSpan span, IReadOnlyList<AmmyObjectBodyOneLine> items)
            : base(span)
        {
            // generator : ConstructorBuilder.CreateConstructor:37
            Items = items;
        }

        public override string ToString()
        {
            return string.Join(".", Items);
        }

        public IReadOnlyList<AmmyObjectBodyOneLine> Items { get; }

    }

    /// <summary>
    /// rule = alternative: __property_set_statement, __ammy_value
    /// </summary>
    public partial class AmmyObjectBodyElement : AmmyStatement
    {
        public AmmyObjectBodyElement(SourceSpan span, object tmpValue, Samples.Irony.AmmyGrammar.Ast.AstObjectBodyElementNodeKinds nodeKind)
            : base(span)
        {
            // generator : ConstructorBuilder.CreateConstructor:37
            TmpValue = tmpValue;
            NodeKind = nodeKind;
        }

        public override string ToString()
        {
            return TmpValue?.ToString() ?? string.Empty;
        }

        public object TmpValue { get; set; }

        public Samples.Irony.AmmyGrammar.Ast.AstObjectBodyElementNodeKinds NodeKind { get; }

    }

    /// <summary>
    /// sequence of __object_body  [Samples.Irony.AmmyGrammar.Ast.AstObjectBody, AmmyObjectBody]
    /// </summary>
    public partial class AmmyObjectBodyInCurlyBrackets : AmmyStatement
    {
        public AmmyObjectBodyInCurlyBrackets(SourceSpan span, AmmyObjectBody body)
            : base(span)
        {
            // generator : ConstructorBuilder.CreateConstructor:37
            Body = body;
        }

        public AmmyObjectBody Body { get; }

    }

    /// <summary>
    /// one or more __object_body_element
    /// </summary>
    public partial class AmmyObjectBodyOneLine : AmmyStatement
    {
        public AmmyObjectBodyOneLine(SourceSpan span, IReadOnlyList<AmmyObjectBodyElement> items)
            : base(span)
        {
            // generator : ConstructorBuilder.CreateConstructor:37
            Items = items;
        }

        public override string ToString()
        {
            return string.Join(".", Items);
        }

        public IReadOnlyList<AmmyObjectBodyElement> Items { get; }

    }

    /// <summary>
    /// sequence of
    /// </summary>
    public partial class AmmyObjectName : AmmyStatement
    {
        public AmmyObjectName(SourceSpan span)
            : base(span)
        {
            // generator : ConstructorBuilder.CreateConstructor:37
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
            // generator : ConstructorBuilder.CreateConstructor:37
        }

    }

    /// <summary>
    /// rule = alternative: Empty, __object_name_key_prefix
    /// </summary>
    public partial class AmmyObjectNameKeyPrefixOptional : AmmyStatement
    {
        public AmmyObjectNameKeyPrefixOptional(SourceSpan span, object tmpValue)
            : base(span)
        {
            // generator : ConstructorBuilder.CreateConstructor:37
            TmpValue = tmpValue;
        }

        public override string ToString()
        {
            return TmpValue?.ToString() ?? string.Empty;
        }

        public object TmpValue { get; set; }

    }

    /// <summary>
    /// rule = alternative: Empty, __object_name
    /// </summary>
    public partial class AmmyObjectNameOptional : AmmyStatement
    {
        public AmmyObjectNameOptional(SourceSpan span, object tmpValue)
            : base(span)
        {
            // generator : ConstructorBuilder.CreateConstructor:37
            TmpValue = tmpValue;
        }

        public override string ToString()
        {
            return TmpValue?.ToString() ?? string.Empty;
        }

        public object TmpValue { get; set; }

    }

    /// <summary>
    /// sequence of __domain_style_name  [Samples.Irony.AmmyGrammar.Ast.AstDomainStyleName, AmmyDomainStyleName], __object_name_optional  [Samples.Irony.AmmyGrammar.Ast.AstObjectNameOptional, AmmyObjectNameOptional]
    /// </summary>
    public partial class AmmyObjectStatement : AmmyStatement
    {
        public AmmyObjectStatement(SourceSpan span, AmmyDomainStyleName objectType, AmmyObjectNameOptional objectName)
            : base(span)
        {
            // generator : ConstructorBuilder.CreateConstructor:37
            ObjectType = objectType;
            ObjectName = objectName;
        }

        public AmmyDomainStyleName ObjectType { get; }

        public AmmyObjectNameOptional ObjectName { get; }

    }

    /// <summary>
    /// sequence of __using_statement_collection  [Samples.Irony.AmmyGrammar.Ast.AstUsingStatementCollection, AmmyUsingStatementCollection], __main_object_statement  [Samples.Irony.AmmyGrammar.Ast.AstMainObjectStatement, AmmyMainObjectStatement]
    /// </summary>
    public partial class AmmyProgram : AmmyStatement
    {
        public AmmyProgram(SourceSpan span, AmmyUsingStatementCollection usings, AmmyMainObjectStatement objectDefinition)
            : base(span)
        {
            // generator : ConstructorBuilder.CreateConstructor:37
            Usings = usings;
            ObjectDefinition = objectDefinition;
        }

        public AmmyUsingStatementCollection Usings { get; }

        public AmmyMainObjectStatement ObjectDefinition { get; }

    }

    /// <summary>
    /// sequence of __identifier  [IdentifierTerminal, string], __ammy_value  [Samples.Irony.AmmyGrammar.Ast.AstAmmyValue, AmmyValue]
    /// </summary>
    public partial class AmmyPropertySetStatement : AmmyStatement
    {
        public AmmyPropertySetStatement(SourceSpan span, string propertyName, AmmyValue propertyValue)
            : base(span)
        {
            // generator : ConstructorBuilder.CreateConstructor:37
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        public string PropertyName { get; }

        public AmmyValue PropertyValue { get; }

    }

    /// <summary>
    /// sequence of __domain_style_name  [Samples.Irony.AmmyGrammar.Ast.AstDomainStyleName, AmmyDomainStyleName]
    /// </summary>
    public partial class AmmyUsingStatement : AmmyStatement
    {
        public AmmyUsingStatement(SourceSpan span, AmmyDomainStyleName namespaceName)
            : base(span)
        {
            // generator : ConstructorBuilder.CreateConstructor:37
            NamespaceName = namespaceName;
        }

        public AmmyDomainStyleName NamespaceName { get; }

    }

    /// <summary>
    /// zero or more __using_statement
    /// </summary>
    public partial class AmmyUsingStatementCollection : AmmyStatement
    {
        public AmmyUsingStatementCollection(SourceSpan span, IReadOnlyList<AmmyUsingStatement> items)
            : base(span)
        {
            // generator : ConstructorBuilder.CreateConstructor:37
            Items = items;
        }

        public override string ToString()
        {
            return string.Join(".", Items);
        }

        public IReadOnlyList<AmmyUsingStatement> Items { get; }

    }

    /// <summary>
    /// rule = alternative: __stringLiteral, __numberLiteral, __boolean_value
    /// </summary>
    public partial class AmmyValue : AmmyStatement
    {
        public AmmyValue(SourceSpan span, object tmpValue, Samples.Irony.AmmyGrammar.Ast.AstAmmyValueNodeKinds nodeKind)
            : base(span)
        {
            // generator : ConstructorBuilder.CreateConstructor:37
            TmpValue = tmpValue;
            NodeKind = nodeKind;
        }

        public override string ToString()
        {
            return TmpValue?.ToString() ?? string.Empty;
        }

        public object TmpValue { get; set; }

        public Samples.Irony.AmmyGrammar.Ast.AstAmmyValueNodeKinds NodeKind { get; }

    }
}
