#nullable disable
// ReSharper disable All
using Irony.Interpreter.Ast;
using Irony.Parsing;

// suggestion: File scope namespace is possible, use [AssumeDefinedNamespace]
namespace Bitbrains.AmmyParser
{
    partial class AmmyGrammar
    {
        public void AutoInit()
        {
            // generator : AssemblyStart:94
            comma_opt.Rule = Empty | comma;
            int_number_optional.Rule = Empty | Number;
            using_directive.Rule = using_ns_directive;
            using_directives_opt.Rule = Empty | using_directives;
            object_setting.Rule = object_property_setting;
            object_settings_opt.Rule = Empty | object_settings;
            ammy_bind_source_opt.Rule = Empty | ammy_bind_source;
            ammy_bind_source_source.Rule = ammy_bind_source_ancestor | ammy_bind_source_element_name | ammy_bind_source_this;
            ammy_bind_set_opt.Rule = Empty | ammy_bind_set;
            ammy_bind_set_NotifyOnSourceUpdated.Rule = ToTerm("NotifyOnSourceUpdated") + ":" + boolean + comma_opt;
            ammy_bind_set_NotifyOnTargetUpdated.Rule = ToTerm("NotifyOnTargetUpdated") + ":" + boolean + comma_opt;
            ammy_bind_set_NotifyOnValidationError.Rule = ToTerm("NotifyOnValidationError") + ":" + boolean + comma_opt;
            ammy_bind_set_ValidatesOnExceptions.Rule = ToTerm("ValidatesOnExceptions") + ":" + boolean + comma_opt;
            ammy_bind_set_ValidatesOnDataErrors.Rule = ToTerm("ValidatesOnDataErrors") + ":" + boolean + comma_opt;
            ammy_bind_set_ValidatesOnNotifyDataErrors.Rule = ToTerm("ValidatesOnNotifyDataErrors") + ":" + boolean + comma_opt;
            ammy_bind_set_StringFormat.Rule = ToTerm("StringFormat") + ":" + TheStringLiteral + comma_opt;
            ammy_bind_set_BindingGroupName.Rule = ToTerm("BindingGroupName") + ":" + TheStringLiteral + comma_opt;
            ammy_bind_set_FallbackValue.Rule = ToTerm("FallbackValue") + ":" + Number + comma_opt;
            ammy_bind_set_IsAsync.Rule = ToTerm("IsAsync") + ":" + boolean + comma_opt;
            ammy_bind_set_item.Rule = ammy_bind_set_NotifyOnSourceUpdated | ammy_bind_set_NotifyOnTargetUpdated | ammy_bind_set_NotifyOnValidationError | ammy_bind_set_ValidatesOnExceptions | ammy_bind_set_ValidatesOnDataErrors | ammy_bind_set_ValidatesOnNotifyDataErrors | ammy_bind_set_StringFormat | ammy_bind_set_BindingGroupName | ammy_bind_set_FallbackValue | ammy_bind_set_IsAsync;
            ammy_bind_set_items.Rule = MakePlusRule(ammy_bind_set_items, null, ammy_bind_set_item);
            ammy_bind_set_items_opt.Rule = Empty | ammy_bind_set_items;
            ammy_property_name.Rule = identifier;
            ammy_property_value.Rule = primary_expression | ammy_bind;
            primary_expression.Rule = literal;
            expression.Rule = primary_expression;
            mixin_or_alias_argument.Rule = identifier;
            mixin_or_alias_arguments_opt.Rule = Empty | mixin_or_alias_arguments;
            object_name_opt.Rule = Empty | object_name;
            statement.Rule = mixin_definition | object_definition;
            statements_opt.Rule = Empty | statements;
        }

        public void AutoInit2()
        {
            // generator : AssemblyStart:116
            MarkPunctuation(":", "BindingGroupName", "FallbackValue", "IsAsync", "NotifyOnSourceUpdated", "NotifyOnTargetUpdated", "NotifyOnValidationError", "StringFormat", "ValidatesOnDataErrors", "ValidatesOnExceptions", "ValidatesOnNotifyDataErrors");
        }

        public NonTerminal comma_opt = new NonTerminal("comma_opt", typeof(AstOptNode));

        public NonTerminal int_number_optional = new NonTerminal("int_number_optional", typeof(AstOptNode));

        public NonTerminal boolean = new NonTerminal("boolean", typeof(AstBoolean));

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

        public NonTerminal object_setting = new NonTerminal("object_setting", typeof(AstObjectSetting));

        public NonTerminal object_settings = new NonTerminal("object_settings", typeof(AstObjectSettings));

        public NonTerminal object_settings_opt = new NonTerminal("object_settings_opt", typeof(AstOptNode));

        public NonTerminal ammy_bind = new NonTerminal("ammy_bind", typeof(AstAmmyBind));

        public NonTerminal ammy_bind_source = new NonTerminal("ammy_bind_source", typeof(AstAmmyBindSource));

        public NonTerminal ammy_bind_source_opt = new NonTerminal("ammy_bind_source_opt", typeof(AstOptNode));

        public NonTerminal ammy_bind_source_source = new NonTerminal("ammy_bind_source_source", typeof(AstAmmyBindSourceSource));

        public NonTerminal ammy_bind_source_ancestor = new NonTerminal("ammy_bind_source_ancestor", typeof(AstAmmyBindSourceAncestor));

        public NonTerminal ammy_bind_source_element_name = new NonTerminal("ammy_bind_source_element_name", typeof(AstAmmyBindSourceElementName));

        public NonTerminal ammy_bind_source_this = new NonTerminal("ammy_bind_source_this", typeof(AstAmmyBindSourceThis));

        public NonTerminal ammy_bind_set = new NonTerminal("ammy_bind_set", typeof(AstAmmyBindSet));

        public NonTerminal ammy_bind_set_opt = new NonTerminal("ammy_bind_set_opt", typeof(AstOptNode));

        public NonTerminal ammy_bind_set_NotifyOnSourceUpdated = new NonTerminal("ammy_bind_set_NotifyOnSourceUpdated", typeof(AstAmmyBindSetNotifyOnSourceUpdated));

        public NonTerminal ammy_bind_set_NotifyOnTargetUpdated = new NonTerminal("ammy_bind_set_NotifyOnTargetUpdated", typeof(AstAmmyBindSetNotifyOnTargetUpdated));

        public NonTerminal ammy_bind_set_NotifyOnValidationError = new NonTerminal("ammy_bind_set_NotifyOnValidationError", typeof(AstAmmyBindSetNotifyOnValidationError));

        public NonTerminal ammy_bind_set_ValidatesOnExceptions = new NonTerminal("ammy_bind_set_ValidatesOnExceptions", typeof(AstAmmyBindSetValidatesOnExceptions));

        public NonTerminal ammy_bind_set_ValidatesOnDataErrors = new NonTerminal("ammy_bind_set_ValidatesOnDataErrors", typeof(AstAmmyBindSetValidatesOnDataErrors));

        public NonTerminal ammy_bind_set_ValidatesOnNotifyDataErrors = new NonTerminal("ammy_bind_set_ValidatesOnNotifyDataErrors", typeof(AstAmmyBindSetValidatesOnNotifyDataErrors));

        public NonTerminal ammy_bind_set_StringFormat = new NonTerminal("ammy_bind_set_StringFormat", typeof(AstAmmyBindSetStringFormat));

        public NonTerminal ammy_bind_set_BindingGroupName = new NonTerminal("ammy_bind_set_BindingGroupName", typeof(AstAmmyBindSetBindingGroupName));

        public NonTerminal ammy_bind_set_FallbackValue = new NonTerminal("ammy_bind_set_FallbackValue", typeof(AstAmmyBindSetFallbackValue));

        public NonTerminal ammy_bind_set_IsAsync = new NonTerminal("ammy_bind_set_IsAsync", typeof(AstAmmyBindSetIsAsync));

        public NonTerminal ammy_bind_set_item = new NonTerminal("ammy_bind_set_item", typeof(AstAmmyBindSetItem));

        public NonTerminal ammy_bind_set_items = new NonTerminal("ammy_bind_set_items", typeof(AstAmmyBindSetItems));

        public NonTerminal ammy_bind_set_items_opt = new NonTerminal("ammy_bind_set_items_opt", typeof(AstOptNode));

        public NonTerminal object_property_setting = new NonTerminal("object_property_setting", typeof(AstObjectPropertySetting));

        public NonTerminal ammy_property_name = new NonTerminal("ammy_property_name", typeof(AstAmmyPropertyName));

        public NonTerminal ammy_property_value = new NonTerminal("ammy_property_value", typeof(AstAmmyPropertyValue));

        public NonTerminal primary_expression = new NonTerminal("primary_expression", typeof(AstPrimaryExpression));

        public NonTerminal expression = new NonTerminal("expression", typeof(AstExpression));

        public NonTerminal mixin_or_alias_argument = new NonTerminal("mixin_or_alias_argument", typeof(AstMixinOrAliasArgument));

        public NonTerminal mixin_or_alias_arguments = new NonTerminal("mixin_or_alias_arguments", typeof(AstMixinOrAliasArguments));

        public NonTerminal mixin_or_alias_arguments_opt = new NonTerminal("mixin_or_alias_arguments_opt", typeof(AstOptNode));

        public NonTerminal object_definition = new NonTerminal("object_definition", typeof(AstObjectDefinition));

        public NonTerminal object_name = new NonTerminal("object_name", typeof(AstObjectName));

        public NonTerminal object_name_opt = new NonTerminal("object_name_opt", typeof(AstOptNode));

        public NonTerminal statement = new NonTerminal("statement", typeof(AstStatement));

        public NonTerminal statements = new NonTerminal("statements", typeof(AstStatements));

        public NonTerminal statements_opt = new NonTerminal("statements_opt", typeof(AstOptNode));

        public NonTerminal ammyCode = new NonTerminal("ammyCode", typeof(AstAmmyCode));

    }

    /// <summary>
    /// AST class for ammy_bind terminal
    /// </summary>
    public partial class AstAmmyBind : BbExpressionListNode, IAstAmmyPropertyValueProvider
    {
    }

    /// <summary>
    /// AST class for ammy_bind_set terminal
    /// </summary>
    public partial class AstAmmyBindSet : BbExpressionListNode
    {
    }

    /// <summary>
    /// AST class for ammy_bind_set_BindingGroupName terminal
    /// </summary>
    public partial class AstAmmyBindSetBindingGroupName : BbExpressionListNode, IAstAmmyBindSetItemProvider
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

        public const string Keyword = "BindingGroupName";

    }

    /// <summary>
    /// AST class for ammy_bind_set_FallbackValue terminal
    /// </summary>
    public partial class AstAmmyBindSetFallbackValue : BbExpressionListNode, IAstAmmyBindSetItemProvider
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

        public const string Keyword = "FallbackValue";

    }

    /// <summary>
    /// AST class for ammy_bind_set_IsAsync terminal
    /// </summary>
    public partial class AstAmmyBindSetIsAsync : BbExpressionListNode, IAstAmmyBindSetItemProvider
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

        public const string Keyword = "IsAsync";

    }

    /// <summary>
    /// AST class for ammy_bind_set_item terminal
    /// </summary>
    public partial class AstAmmyBindSetItem : BbExpressionListNode
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

    }

    /// <summary>
    /// AST class for ammy_bind_set_items terminal
    /// </summary>
    public partial class AstAmmyBindSetItems : BbExpressionListNode<IAstAmmyBindSetItem>
    {
    }

    /// <summary>
    /// AST class for ammy_bind_set_NotifyOnSourceUpdated terminal
    /// </summary>
    public partial class AstAmmyBindSetNotifyOnSourceUpdated : BbExpressionListNode, IAstAmmyBindSetItemProvider
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

        public const string Keyword = "NotifyOnSourceUpdated";

    }

    /// <summary>
    /// AST class for ammy_bind_set_NotifyOnTargetUpdated terminal
    /// </summary>
    public partial class AstAmmyBindSetNotifyOnTargetUpdated : BbExpressionListNode, IAstAmmyBindSetItemProvider
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

        public const string Keyword = "NotifyOnTargetUpdated";

    }

    /// <summary>
    /// AST class for ammy_bind_set_NotifyOnValidationError terminal
    /// </summary>
    public partial class AstAmmyBindSetNotifyOnValidationError : BbExpressionListNode, IAstAmmyBindSetItemProvider
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

        public const string Keyword = "NotifyOnValidationError";

    }

    /// <summary>
    /// AST class for ammy_bind_set_StringFormat terminal
    /// </summary>
    public partial class AstAmmyBindSetStringFormat : BbExpressionListNode, IAstAmmyBindSetItemProvider
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

        public const string Keyword = "StringFormat";

    }

    /// <summary>
    /// AST class for ammy_bind_set_ValidatesOnDataErrors terminal
    /// </summary>
    public partial class AstAmmyBindSetValidatesOnDataErrors : BbExpressionListNode, IAstAmmyBindSetItemProvider
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

        public const string Keyword = "ValidatesOnDataErrors";

    }

    /// <summary>
    /// AST class for ammy_bind_set_ValidatesOnExceptions terminal
    /// </summary>
    public partial class AstAmmyBindSetValidatesOnExceptions : BbExpressionListNode, IAstAmmyBindSetItemProvider
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

        public const string Keyword = "ValidatesOnExceptions";

    }

    /// <summary>
    /// AST class for ammy_bind_set_ValidatesOnNotifyDataErrors terminal
    /// </summary>
    public partial class AstAmmyBindSetValidatesOnNotifyDataErrors : BbExpressionListNode, IAstAmmyBindSetItemProvider
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

        public const string Keyword = "ValidatesOnNotifyDataErrors";

    }

    /// <summary>
    /// AST class for ammy_bind_source terminal
    /// </summary>
    public partial class AstAmmyBindSource : BbExpressionListNode
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

    }

    /// <summary>
    /// AST class for ammy_bind_source_ancestor terminal
    /// </summary>
    public partial class AstAmmyBindSourceAncestor : BbExpressionListNode, IAstAmmyBindSourceSourceProvider
    {
    }

    /// <summary>
    /// AST class for ammy_bind_source_element_name terminal
    /// </summary>
    public partial class AstAmmyBindSourceElementName : BbExpressionListNode, IAstAmmyBindSourceSourceProvider
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

    }

    /// <summary>
    /// AST class for ammy_bind_source_source terminal
    /// </summary>
    public partial class AstAmmyBindSourceSource : BbExpressionListNode
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

    }

    /// <summary>
    /// AST class for ammy_bind_source_this terminal
    /// </summary>
    public partial class AstAmmyBindSourceThis : BbExpressionListNode, IAstAmmyBindSourceSourceProvider
    {
    }

    /// <summary>
    /// AST class for ammyCode terminal
    /// </summary>
    public partial class AstAmmyCode : BbExpressionListNode<System.Object>
    {
    }

    /// <summary>
    /// AST class for ammy_property_name terminal
    /// </summary>
    public partial class AstAmmyPropertyName : BbExpressionListNode
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

    }

    /// <summary>
    /// AST class for ammy_property_value terminal
    /// </summary>
    public partial class AstAmmyPropertyValue : BbExpressionListNode
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

    }

    /// <summary>
    /// AST class for boolean terminal
    /// </summary>
    public partial class AstBoolean : AstNode
    {
    }

    /// <summary>
    /// AST class for expression terminal
    /// </summary>
    public partial class AstExpression : BbExpressionListNode
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

    }

    /// <summary>
    /// AST class for identifier_or_builtin terminal
    /// </summary>
    public partial class AstIdentifierOrBuiltin : BbExpressionListNode
    {
    }

    /// <summary>
    /// AST class for literal terminal
    /// </summary>
    public partial class AstLiteral : BbExpressionListNode, IAstPrimaryExpressionProvider
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

    }

    /// <summary>
    /// AST class for mixin_definition terminal
    /// </summary>
    public partial class AstMixinDefinition : BbExpressionListNode, IAstStatementProvider
    {
    }

    /// <summary>
    /// AST class for mixin_or_alias_argument terminal
    /// </summary>
    public partial class AstMixinOrAliasArgument : BbExpressionListNode
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

    }

    /// <summary>
    /// AST class for mixin_or_alias_arguments terminal
    /// </summary>
    public partial class AstMixinOrAliasArguments : BbExpressionListNode<System.Object>
    {
    }

    /// <summary>
    /// AST class for object_definition terminal
    /// </summary>
    public partial class AstObjectDefinition : BbExpressionListNode, IAstStatementProvider
    {
    }

    /// <summary>
    /// AST class for object_name terminal
    /// </summary>
    public partial class AstObjectName : BbExpressionListNode
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

    }

    /// <summary>
    /// AST class for object_property_setting terminal
    /// </summary>
    public partial class AstObjectPropertySetting : BbExpressionListNode, IAstObjectSettingProvider
    {
    }

    /// <summary>
    /// AST class for object_setting terminal
    /// </summary>
    public partial class AstObjectSetting : BbExpressionListNode
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

    }

    /// <summary>
    /// AST class for object_settings terminal
    /// </summary>
    public partial class AstObjectSettings : BbExpressionListNode<IAstObjectSetting>
    {
    }

    /// <summary>
    /// AST class for primary_expression terminal
    /// </summary>
    public partial class AstPrimaryExpression : BbExpressionListNode, IAstAmmyPropertyValueProvider, IAstExpressionProvider
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

    }

    /// <summary>
    /// AST class for qual_name_segment terminal
    /// </summary>
    public partial class AstQualNameSegment : BbExpressionListNode
    {
    }

    /// <summary>
    /// AST class for qual_name_segments_opt2 terminal
    /// </summary>
    public partial class AstQualNameSegmentsOpt2 : BbExpressionListNode
    {
    }

    /// <summary>
    /// AST class for qual_name_with_targs terminal
    /// </summary>
    public partial class AstQualNameWithTargs : BbExpressionListNode
    {
    }

    /// <summary>
    /// AST class for statement terminal
    /// </summary>
    public partial class AstStatement : BbExpressionListNode
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

    }

    /// <summary>
    /// AST class for statements terminal
    /// </summary>
    public partial class AstStatements : BbExpressionListNode<IAstStatement>
    {
    }

    /// <summary>
    /// AST class for using_directive terminal
    /// </summary>
    public partial class AstUsingDirective : BbExpressionListNode
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

    }

    /// <summary>
    /// AST class for using_directives terminal
    /// </summary>
    public partial class AstUsingDirectives : BbExpressionListNode<IAstUsingDirective>
    {
    }

    /// <summary>
    /// AST class for using_ns_directive terminal
    /// </summary>
    public partial class AstUsingNsDirective : BbExpressionListNode, IAstUsingDirectiveProvider
    {
        protected override int[] GetMap()
        {
            return new [] { 0 };
        }

    }

    public partial interface IAstAmmyBindSetItem
    {
    }

    public partial interface IAstAmmyBindSetItemProvider
    {
        IAstAmmyBindSetItem GetData(Irony.Interpreter.ScriptThread thread);

    }

    public partial interface IAstAmmyBindSourceSource
    {
    }

    public partial interface IAstAmmyBindSourceSourceProvider
    {
        IAstAmmyBindSourceSource GetData(Irony.Interpreter.ScriptThread thread);

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

