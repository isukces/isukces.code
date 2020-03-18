using Irony.Interpreter;
using Irony.Parsing;

namespace Bitbrains.AmmyParser
{
    [Language("Ammy")]
    public partial class AmmyGrammar : InterpretedLanguageGrammar
    {
        public AmmyGrammar() : base(true)
        {
            Number     = TerminalFactory.CreateCSharpNumber("Number");
            identifier = TerminalFactory.CreateCSharpIdentifier("identifier");

            AutoInit();
            var SingleLineComment =
                new CommentTerminal("SingleLineComment", "//", "\r", "\n", "\u2085", "\u2028", "\u2029");
            var DelimitedComment = new CommentTerminal("DelimitedComment", "/*", "*/");
            NonGrammarTerminals.Add(SingleLineComment);
            NonGrammarTerminals.Add(DelimitedComment);

            var comma = ToTerm(",", "comma");
            var comma_opt = new NonTerminal("comma_opt")
            {
                Rule = Empty | comma
            };
            var dot      = ToTerm(".", "dot");
            var parOpen  = ToTerm("(");
            var parClose = ToTerm(")");

            var curlyOpen  = ToTerm("{");
            var curlyClose = ToTerm("}");

            /*
            var new_line = new NewLineTerminal("new_line");
            var new_lines = new NonTerminal("new_lines");
            new_lines.Rule = MakePlusRule(new_lines, new_line);
            var new_lines_or_comma = new NonTerminal("new_lines_or_comma", typeof(AstNode))
            {
                Rule = new_lines | comma
            };
            */

            // var number     = TerminalFactory.CreateCSharpNumber("number");

            var comment = new CommentTerminal("comment", "#", "\n", "\r");

            // var literal = new NonTerminal("literal");

            // ==========================================================
            /*var qual_name_segment = new NonTerminal("qual_name_segment", typeof(Tqual_name_segment));
            var qual_name_segments_opt = new NonTerminal("qual_name_segments_opt", typeof(Tqual_name_segments_opt));
            var qual_name_with_targs = new NonTerminal("qual_name_with_targs");
            var identifier_or_builtin = new NonTerminal("identifier_or_builtin", typeof(Tidentifier_or_builtin));
            
            
            var using_ns_directive = new NonTerminal("using_ns_directive");
            var using_directive      = new NonTerminal("using_directive");
            var using_directives     = new NonTerminal("using_directives");
            var using_directives_opt = new NonTerminal("using_directives_opt");*/
            // ==========================================================

            Number.Options |= NumberOptions.AllowSign | NumberOptions.AllowLetterAfter |
                              NumberOptions.AllowStartEndDot | NumberOptions.AllowUnderscore;
            var StringLiteral = TerminalFactory.CreateCSharpString("StringLiteral");

            //var primary_expression = new NonTerminal("primary_expression");
            literal.Rule = Number | StringLiteral | /*  | CharLiteral |*/ "true" | "false" | "null";

            /*primary_expression.Rule =
                literal
                /*| unary_expression
                | parenthesized_expression
                | member_access
                | pre_incr_decr_expression
                | post_incr_decr_expression
                | object_creation_expression
                | anonymous_object_creation_expression
                | typeof_expression
                | checked_expression
                | unchecked_expression
                | default_value_expression
                | anonymous_method_expression#1#
                ;*/

            /*
            var expression = new NonTerminal("expression", "expression");
            expression.Rule = /*conditional_expression
                              | bin_op_expression
                              | typecast_expression
                              |#1#
                primary_expression;*/

            var initializer_value = new NonTerminal("initializer_value");
            initializer_value.Rule = expression; // | list_initializer;

            var local_variable_type = new NonTerminal("local_variable_type");
            local_variable_type.Rule = /* member_access | */
                "var"; // | builtin_type; //to fix the conflict, changing to member-access here

            var local_variable_declarator = new NonTerminal("local_variable_declarator");
            local_variable_declarator.Rule = identifier | (identifier + "=" + initializer_value);

            var local_variable_declarators = new NonTerminal("local_variable_declarators");
            local_variable_declarators.Rule =
                MakePlusRule(local_variable_declarators, comma, local_variable_declarator);

            var local_variable_declaration = new NonTerminal("local_variable_declaration");
            local_variable_declaration.Rule = local_variable_type + local_variable_declarators; //!!!

            var resource_acquisition = new NonTerminal("resource_acquisition");
            var embedded_statement   = new NonTerminal("embedded_statement");
            resource_acquisition.Rule = local_variable_declaration; //| expression;

            var using_statement = new NonTerminal("using_statement")
            {
                Rule = "using" + parOpen + resource_acquisition + parClose + embedded_statement
            };

            embedded_statement.Rule = using_statement
                ;
            /* block | semi /*empty_statement#1# | statement_expression + semi | selection_statement
                                      | iteration_statement | jump_statement | try_statement | checked_statement | unchecked_statement
                                      | lock_statement | using_statement | yield_statement;*/

            /*
            var Stmt = new NonTerminal("Stmt");
            Stmt.Rule = using_statement | Empty;
            var ExtStmt = new NonTerminal("ExtStmt");
            ExtStmt.Rule = Stmt; // + Eos | FunctionDef;
            */

            // === Basic
            qual_name_segment.Rule = dot + identifier
                //| "::" + identifier
                //| type_argument_list;
                ;

            qual_name_segments_opt2.Rule = MakeStarRule(qual_name_segments_opt2, null, qual_name_segment);
            identifier_or_builtin.Rule   = identifier; // | builtin_type;
            qual_name_with_targs.Rule    = identifier_or_builtin + qual_name_segments_opt2;

            // ============== Using
            using_ns_directive.Rule = "using" + qual_name_with_targs; // + new_lines; //  + semi;
            // using_directive.Rule      = using_ns_directive; // | using_alias_directive ;
            using_directives.Rule = MakePlusRule(using_directives, null, using_directive);

            // ============== object internals
            // ammy_property_name.Rule = identifier;
            object_property_setting.Rule = ammy_property_name + ":" + ammy_property_value;
            // object_setting.Rule = object_property_setting;
            object_settings.Rule = MakePlusRule(object_settings, comma_opt, object_setting);
            // ============== values and bindigs
            ammy_bind_source.Rule = "from" + ammy_bind_source_source;
            ammy_bind.Rule        = "bind" + StringLiteral + ammy_bind_source_opt;
            ammy_bind_source_ancestor.Rule =
                "$ancestor" + "<" + qual_name_with_targs + ">" + "(" + int_number_optional + ")";

            ammy_bind_source_element_name.Rule = StringLiteral;
            ammy_bind_source_this.Rule = "$this";

            // ammy_property_value.Rule = primary_expression;
            // ============== Mixin

            // mixin_or_alias_argument.Rule = identifier;
            mixin_or_alias_arguments.Rule = MakePlusRule(mixin_or_alias_arguments, null, mixin_or_alias_argument);

            mixin_definition.Rule = "mixin" + identifier
                                            + parOpen + mixin_or_alias_arguments_opt + parClose
                                            + "for" + qual_name_with_targs
                                            + curlyOpen + object_settings_opt + curlyClose;
            // ============= Statements
            //statement.Rule = mixin_definition; 
            statements.Rule = MakePlusRule(statements, null, statement);

            ammyCode.Rule = using_directives_opt + statements_opt;
            //ammyCode.Rule = MakePlusRule(ammyCode, ExtStmt);

            Root = ammyCode;
            // 10. Language flags
            LanguageFlags = LanguageFlags.NewLineBeforeEOF
                            | LanguageFlags.CreateAst
                            | LanguageFlags.SupportsBigInt;
            MarkPunctuation("bind", "mixin", "for", ":", "using", "{", "}", "(", ")", ",", ".",
                "from",  "<", ">", "$ancestor", "$this");
        }

        public NumberLiteral Number;

        private readonly IdentifierTerminal identifier;
    }
}