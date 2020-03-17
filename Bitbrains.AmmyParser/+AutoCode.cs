// ReSharper disable All
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Bitbrains.AmmyParser
{
    partial class AmmyGrammar
    {
        public void AutoInit()
        {
            // generator : AssemblyStart:89
            using_directive.Rule = using_ns_directive;
            using_directives_opt.Rule = Empty | using_directives;
            statement.Rule = mixin_definition;
            statements_opt.Rule = Empty | statements;
            object_setting.Rule = object_property_setting;
            object_settings_opt.Rule = Empty | object_settings;
            ammy_property_name.Rule = identifier;
            ammy_property_value.Rule = primary_expression | ammy_bind;
            primary_expression.Rule = literal;
            expression.Rule = primary_expression;
            mixin_or_alias_argument.Rule = identifier;
            mixin_or_alias_arguments_opt.Rule = Empty | mixin_or_alias_arguments;
        }

        public NonTerminal literal = new NonTerminal("literal", typeof(AstLiteral));

        public NonTerminal qual_name_segment = new NonTerminal("qual_name_segment", typeof(AstQualNameSegment));

        public NonTerminal qual_name_segments_opt2 = new NonTerminal("qual_name_segments_opt2", typeof(AstQualNameSegmentsOpt2));

        public NonTerminal qual_name_with_targs = new NonTerminal("qual_name_with_targs", typeof(AstQualNameWithTargs));

        public NonTerminal identifier_or_builtin = new NonTerminal("identifier_or_builtin", typeof(AstIdentifierOrBuiltin));

        public NonTerminal using_ns_directive = new NonTerminal("using_ns_directive", typeof(AstUsingNsDirective));

        public NonTerminal using_directive = new NonTerminal("using_directive", typeof(AstUsingDirective));

        public NonTerminal using_directives = new NonTerminal("using_directives", typeof(AstUsingDirectives));

        public NonTerminal using_directives_opt = new NonTerminal("using_directives_opt", typeof(AstOptNode));

        public NonTerminal mixin_definition = new NonTerminal("mixin_definition", typeof(AstMixinDefinition));

        public NonTerminal statement = new NonTerminal("statement", typeof(AstStatement));

        public NonTerminal statements = new NonTerminal("statements", typeof(AstStatements));

        public NonTerminal statements_opt = new NonTerminal("statements_opt", typeof(AstOptNode));

        public NonTerminal object_setting = new NonTerminal("object_setting", typeof(AstObjectSetting));

        public NonTerminal object_settings = new NonTerminal("object_settings", typeof(AstObjectSettings));

        public NonTerminal object_settings_opt = new NonTerminal("object_settings_opt", typeof(AstOptNode));

        public NonTerminal ammy_bind = new NonTerminal("ammy_bind", typeof(AstAmmyBind));

        public NonTerminal object_property_setting = new NonTerminal("object_property_setting", typeof(AstObjectPropertySetting));

        public NonTerminal ammy_property_name = new NonTerminal("ammy_property_name", typeof(AstAmmyPropertyName));

        public NonTerminal ammy_property_value = new NonTerminal("ammy_property_value", typeof(AstAmmyPropertyValue));

        public NonTerminal primary_expression = new NonTerminal("primary_expression", typeof(AstPrimaryExpression));

        public NonTerminal expression = new NonTerminal("expression", typeof(AstExpression));

        public NonTerminal mixin_or_alias_argument = new NonTerminal("mixin_or_alias_argument", typeof(AstMixinOrAliasArgument));

        public NonTerminal mixin_or_alias_arguments = new NonTerminal("mixin_or_alias_arguments", typeof(AstMixinOrAliasArguments));

        public NonTerminal mixin_or_alias_arguments_opt = new NonTerminal("mixin_or_alias_arguments_opt", typeof(AstOptNode));

        public NonTerminal ammyCode = new NonTerminal("ammyCode", typeof(AstAmmyCode));

    }

    /// <summary>
    /// AST class for ammy_bind terminal
    /// </summary>
    partial class AstAmmyBind : BbExpressionListNode, IAstAmmyPropertyValueProvider
    {
    }

    /// <summary>
    /// AST class for ammyCode terminal
    /// </summary>
    partial class AstAmmyCode : ExpressionListNode<System.Object>
    {
    }

    /// <summary>
    /// AST class for ammy_property_name terminal
    /// </summary>
    partial class AstAmmyPropertyName : BbExpressionListNode
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

    }

    /// <summary>
    /// AST class for ammy_property_value terminal
    /// </summary>
    partial class AstAmmyPropertyValue : BbExpressionListNode
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

    }

    /// <summary>
    /// AST class for expression terminal
    /// </summary>
    partial class AstExpression : BbExpressionListNode
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

    }

    /// <summary>
    /// AST class for identifier_or_builtin terminal
    /// </summary>
    partial class AstIdentifierOrBuiltin : BbExpressionListNode
    {
    }

    /// <summary>
    /// AST class for literal terminal
    /// </summary>
    partial class AstLiteral : BbExpressionListNode, IAstPrimaryExpressionProvider
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

    }

    /// <summary>
    /// AST class for mixin_definition terminal
    /// </summary>
    partial class AstMixinDefinition : BbExpressionListNode, IAstStatementProvider
    {
    }

    /// <summary>
    /// AST class for mixin_or_alias_argument terminal
    /// </summary>
    partial class AstMixinOrAliasArgument : BbExpressionListNode
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

    }

    /// <summary>
    /// AST class for mixin_or_alias_arguments terminal
    /// </summary>
    partial class AstMixinOrAliasArguments : ExpressionListNode<System.Object>
    {
    }

    /// <summary>
    /// AST class for object_property_setting terminal
    /// </summary>
    partial class AstObjectPropertySetting : BbExpressionListNode, IAstObjectSettingProvider
    {
    }

    /// <summary>
    /// AST class for object_setting terminal
    /// </summary>
    partial class AstObjectSetting : BbExpressionListNode
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

    }

    /// <summary>
    /// AST class for object_settings terminal
    /// </summary>
    partial class AstObjectSettings : ExpressionListNode<IAstObjectSetting>
    {
    }

    /// <summary>
    /// AST class for primary_expression terminal
    /// </summary>
    partial class AstPrimaryExpression : BbExpressionListNode, IAstAmmyPropertyValueProvider, IAstExpressionProvider
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

    }

    /// <summary>
    /// AST class for qual_name_segment terminal
    /// </summary>
    partial class AstQualNameSegment : BbExpressionListNode
    {
    }

    /// <summary>
    /// AST class for qual_name_segments_opt2 terminal
    /// </summary>
    partial class AstQualNameSegmentsOpt2 : BbExpressionListNode
    {
    }

    /// <summary>
    /// AST class for qual_name_with_targs terminal
    /// </summary>
    partial class AstQualNameWithTargs : BbExpressionListNode
    {
    }

    /// <summary>
    /// AST class for statement terminal
    /// </summary>
    partial class AstStatement : BbExpressionListNode
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

    }

    /// <summary>
    /// AST class for statements terminal
    /// </summary>
    partial class AstStatements : ExpressionListNode<IAstStatement>
    {
    }

    /// <summary>
    /// AST class for using_directive terminal
    /// </summary>
    partial class AstUsingDirective : BbExpressionListNode
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

    }

    /// <summary>
    /// AST class for using_directives terminal
    /// </summary>
    partial class AstUsingDirectives : ExpressionListNode<IAstUsingDirective>
    {
    }

    /// <summary>
    /// AST class for using_ns_directive terminal
    /// </summary>
    partial class AstUsingNsDirective : BbExpressionListNode, IAstUsingDirectiveProvider
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

    }

    public partial interface IAstAmmyPropertyValue
    {
    }

    public partial interface IAstAmmyPropertyValueProvider
    {
        IAstAmmyPropertyValue GetData(Irony.Interpreter.ScriptThread thread);

    }

    public partial interface IAstExpression
    {
    }

    public partial interface IAstExpressionProvider
    {
        IAstExpression GetData(Irony.Interpreter.ScriptThread thread);

    }

    public partial interface IAstObjectSetting
    {
    }

    public partial interface IAstObjectSettingProvider
    {
        IAstObjectSetting GetData(Irony.Interpreter.ScriptThread thread);

    }

    public partial interface IAstPrimaryExpression
    {
    }

    public partial interface IAstPrimaryExpressionProvider
    {
        IAstPrimaryExpression GetData(Irony.Interpreter.ScriptThread thread);

    }

    public partial interface IAstStatement
    {
    }

    public partial interface IAstStatementProvider
    {
        IAstStatement GetData(Irony.Interpreter.ScriptThread thread);

    }

    public partial interface IAstUsingDirective
    {
    }

    public partial interface IAstUsingDirectiveProvider
    {
        IAstUsingDirective GetData(Irony.Interpreter.ScriptThread thread);

    }
}
