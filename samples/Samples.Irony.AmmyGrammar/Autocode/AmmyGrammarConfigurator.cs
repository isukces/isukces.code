using Irony.Parsing;
using iSukces.Code;
using iSukces.Code.AutoCode;
using iSukces.Code.Irony;
using Samples.Irony.AmmyGrammar.Ast;
using Samples.Irony.AmmyGrammar.Data;

namespace Samples.Irony.AmmyGrammar.Autocode
{
    public class AmmyGrammarConfigurator : IronyAutocodeConfigurator<BaseAstNode>
    {
        public AmmyGrammarConfigurator()
            : base("Samples.Irony.AmmyGrammar", "AmmyGrammar")
        {
        }

        protected override void AfterAdd(NonTerminalInfo info)
        {
            base.AfterAdd(info);
            info.WithDataBaseClassName(typeof(AmmyStatement));
        }

        protected override void BuildInternal()
        {
            NonTerminalInfo.DataClassNameFactory =
                name =>
                {
                    var tmp = name.GetCamelTerminalName();
                    if (!tmp.StartsWith("Ammy"))
                        tmp = "Ammy" + tmp;
                    return new TypeNameProviderEx(new StringTypeNameProvider("." + tmp), TypeNameProviderFlags.CreateAutoCode);
                };
            var identifier    = new TokenName("identifier");
            var stringLiteral = new TokenName("stringLiteral");
            var numberLiteral = new TokenName("numberLiteral");

            var generator = Generator;
            var cfg = generator.Cfg
                .WithSingleLineComment("//", "\r", "\n", "\u2085", "\u2028", "\u2029")
                .WithDelimitedComment("/*", "*/");

            cfg.SpecialTerminals[SpecialTerminalKind.CreateCSharpIdentifier] = identifier.Name;
            cfg.SpecialTerminals[SpecialTerminalKind.CreateCSharpNumber] = numberLiteral.Name;
            cfg.CSharpNumberLiteralOptions = NumberOptions.AllowSign | NumberOptions.AllowUnderscore;
            cfg.SpecialTerminals[SpecialTerminalKind.CreateCSharpString] = stringLiteral.Name;

            var comma     = AddPunctuation(",", "comma");
            var dot       = AddPunctuation(".", "dot");
            var doubledot = AddPunctuation(":", "doubledot");

            AddBrackets("(", ")", "par");
            AddBrackets("{", "}", "curly");
            AddBrackets("[", "]", "square");

            var xtrue  = AddReservedWord("true", "true");
            var xfalse = AddReservedWord("false", "false");
            AddReservedWord("using", "using");
            AddReservedWord("Key", "key");

            /*AddNonTerminal("literal")
                .AsOneOf(identifier, stringLiteral);*/

            var end_of_using_statement = AddNonTerminal("end_of_using_statement")
                .AsOneOf( NewLinePlus, Eof)
                .WithNoAstClass()
                .WithNoDataClass();

            // var semicolon_optional = AddOptionalFrom(semicolon);

            // ==== using 

            var domain_style_name = AddNonTerminal("domain_style_name")
                .WithStarRule(dot, identifier);

            var using_statement = AddNonTerminal("using_statement")
                .WithSequenceRule(
                    generator.GetSequenceRuleBuilder()
                        .With("using")
                        .With(domain_style_name, "NamespaceName")
                        // .With(semicolon_optional)
                        .With(end_of_using_statement)
                );
            var using_statement_collection = AddNonTerminal("using_statement_collection")
                .WithStarRule(using_statement);

            var boolean_value = AddNonTerminal("boolean_value")
                .AsOneOf(rule =>
                {
                    rule.AlternativeInterfaceName = TypeNameProviderEx.MakeBuiltIn<bool>();
                }, xtrue, xfalse);

            var ammy_value = AddNonTerminal("ammy_value")
                .AsOneOf(stringLiteral, numberLiteral, boolean_value);

            var property_set_statement = AddNonTerminal("property_set_statement")
                .WithSequenceRule(
                    generator.GetSequenceRuleBuilder()
                        .With(identifier, "PropertyName")
                        .With(doubledot)
                        .With(ammy_value, "PropertyValue")
                );

            var object_body_element = AddNonTerminal("object_body_element")
                .AsOneOf(property_set_statement, ammy_value);

            var object_body_one_line = AddNonTerminal("object_body_one_line")
                .WithPlusRule(comma, object_body_element);

            var object_body = AddNonTerminal("object_body")
                .WithStarRule(NewLinePlus, object_body_one_line, Delimiters2.Trailing);

            var object_body_in_curly_brackets = AddNonTerminal("object_body_in_curly_brackets")
                .WithSequenceRule(
                    generator.GetSequenceRuleBuilder()
                        .With("{")
                        .With(object_body, "Body")
                        .With("}")
                );

            // ==== MAIN OBJECT
            // UserControl "Pd.Cad.Wpf.CtrlCadBusSegmentSplit" {
            var main_object_statement = AddNonTerminal("main_object_statement")
                .WithSequenceRule(
                    generator.GetSequenceRuleBuilder()
                        .With(domain_style_name, "BaseObjectType", "Type of base class")
                        // .With(NewLineStar)
                        .With(stringLiteral, "FullTypeName", "Full type name")
                        //.With(NewLineStar)
                        .With(object_body_in_curly_brackets, SequenceFlags.PreferShift,
                            "Body", "Body in brackets")
                    //.With(NewLineStar)
                );

            // ==== OBJECT
            // var object_name1 = stringLiteral;
            var object_name_key_prefix = AddNonTerminal("object_name_key_prefix")
                .WithSequenceRule(
                    generator.GetSequenceRuleBuilder()
                        .With("Key")
                        .With("=")
                );
            var object_name_key_prefix_optional = AddOptionalFrom(object_name_key_prefix);

            var object_name = AddNonTerminal("object_name")
                .WithSequenceRule(
                    generator.GetSequenceRuleBuilder()
                        .With(object_name_key_prefix_optional)
                        .With(stringLiteral)
                );
            var object_name_optional = AddOptionalFrom(object_name);

            var object_statement = AddNonTerminal("object_statement")
                .WithSequenceRule(
                    generator.GetSequenceRuleBuilder()
                        .With(domain_style_name, "ObjectType")
                        //.With(NewLineStar)
                        .With(object_name_optional, "ObjectName")
                        //.With(NewLineStar)
                        .With(object_body_in_curly_brackets)
                    // .With(NewLineStar)
                );

            //===============================
            {
                // ============ Root
                generator.Cfg.Root = AddNonTerminal("ammy_program")
                        .WithSequenceRule(
                            generator.GetSequenceRuleBuilder()
                                .With(using_statement_collection, "Usings")
                                .With(main_object_statement, "ObjectDefinition")
                                .With(NewLineStar)
                        )
                    // .WithDataClassName(".AmmyProgram", typeof(AmmyStatement))
                    ;
            }

            /*
            AddNonTerminal("primary_expression")
                .AsOneOf("literal");*/
            /*
            AddNonTerminal("qual_name_segments_opt2");
            AddNonTerminal("qual_name_with_targs");
            AddNonTerminal("identifier_or_builtin");
            */

            /*
             * KeyTerm Lpar = ToTerm("(");
      KeyTerm Rpar = ToTerm(")");
             *using_statement.Rule = "using" + Lpar + resource_acquisition + Rpar + embedded_statement;
             * resource_acquisition.Rule = local_variable_declaration | expression;
             *  local_variable_declaration.Rule = local_variable_type + local_variable_declarators; //!!!
             * member_access
             *  member_access.Rule = identifier_ext + member_access_segments_opt;
             *  member_access_segments_opt.Rule = MakeStarRule(member_access_segments_opt, null, member_access_segment);
             *
             *  member_access_segment.Rule = dot + identifier
                                 | array_indexer
                                 | argument_list_par
                                 | type_argument_list;
                                 KeyTerm dot = ToTerm(".", "dot");
             * 
             */

            /*var info = AddNonTerminal("using_directives")
                .AsExpressionList<UsingStatementData>();
            AddOptionalFrom(info);*/

            /*
            
          

            AddNonTerminal("using_ns_directive")
                .AsOneOf(1, "using_ns_directive");
            AddNonTerminal("using_directive")
                .AsOneOf("using_ns_directive");
           

            AddNonTerminal("using_directives_opt,AstOptNode");
            AddNonTerminal("ammyCode,StatementListNode");
            AddNonTerminal("mixin_definition,,,1 3 6 8");

            AddNonTerminal("statement")
                .AsOneOf("mixin_definition");
            AddNonTerminal("statements")
                .AsExpressionList("AmmyStatement");

            AddNonTerminal("statements_opt,AstOptNode");
            AddNonTerminal("object_setting")
                .AsOneOf("object_property_setting");
            AddNonTerminal("object_settings");
            AddNonTerminal("object_settings_opt,AstOptNode");
            AddNonTerminal("object_property_setting,,,0 2");

            AddNonTerminal("ammy_property_name")
                .AsOneOf("identifier");
            AddNonTerminal("ammy_property_value")
                .AsOneOf("primary_expression");
           
            AddNonTerminal("expression")
                .AsOneOf("primary_expression");
            AddNonTerminal("mixin_or_alias_argument")
                .AsOneOf("identifier");

            AddNonTerminal("mixin_or_alias_arguments");
            AddNonTerminal("mixin_or_alias_arguments_opt");
            */
            cfg.DoEvaluateHelper = new MyDoEvaluateHelper();
        }

        public class MyDoEvaluateHelper : IDoEvaluateHelper
        {
            public bool GetDataClassConstructorArgument(Input34 input)
            {
                if (input.LastChance)
                {
                    if (input.Argument.Type == "SourceSpan")
                    {
                        input.Expression = new CsExpression(nameof(AmmyStatement.Span));
                        return true;
                    }
                }

                
                return false;
            }

            
        }
    }
}