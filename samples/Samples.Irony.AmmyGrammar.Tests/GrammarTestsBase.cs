using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Irony;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;
using Newtonsoft.Json;

namespace Samples.Irony.AmmyGrammar.GrammarTest
{
    public class GrammarTestsBase<TAmmyGrammar, TData> where TAmmyGrammar : Grammar, new()
    {
        public static LanguageData GetLanguageData()
        {
            var grammar  = new TAmmyGrammar();
            var language = new LanguageData(grammar);

            foreach (var i in language.Errors)
            {
                IronyDebugger.Debug(i.State, language);
              
                throw new Exception(i.Message);
            }

            return language;
        }

        public static AstNode ParseAst(LanguageData language, string sourceCode)
        {
            var parser = new Parser(language);
            var tree   = parser.Parse(sourceCode);
            if (tree.Status == ParseTreeStatus.Error)
            {
                var msg = tree.ParserMessages.LastOrDefault(a => a.Level == ErrorLevel.Error);
                if (msg != null)
                    throw new Exception(msg.Message + " at " + msg.Location);
                var errorToken = parser.Context.CurrentParseTree.Tokens
                    .LastOrDefault(a => a.Category == TokenCategory.Error);
                if (errorToken != null)
                    throw new Exception(errorToken.ValueString + " at " + errorToken.Location);

                throw new Exception("other");
            }

            var ast         = tree.Root.AstNode;
            var rootAstNode = (AstNode)ast;
            return rootAstNode;
        }

        public static TData ParseTree(LanguageData language, string sourceCode)
        {
            var rootAstNode = ParseAst(language, sourceCode);
            var runtime     = new LanguageRuntime(language);
            var app         = new ScriptApp(runtime);
            var o           = rootAstNode.Evaluate(new ScriptThread(app));
            return (TData)o;
        }

        protected string Serialize(object obj)
        {
            var sb = new StringBuilder();
            using(var sw = new StringWriter(sb))
            {
                using(var writer = new JsonTextWriter(sw))
                {
                    writer.QuoteChar  = '\'';
                    writer.Formatting = Formatting.Indented;
                    var ser = new JsonSerializer();
                    ser.Converters.Add(new SourceSpanJsonConverter());
                    ser.Serialize(writer, obj);
                }
            }

            return sb.ToString();
        }
    }

    internal sealed class SourceSpanJsonConverter : JsonConverter<SourceSpan>
    {
        public override SourceSpan ReadJson(JsonReader reader, Type objectType, SourceSpan existingValue,
            bool hasExistingValue,
            JsonSerializer serializer) =>
            throw new NotImplementedException();

        public override void WriteJson(JsonWriter writer, SourceSpan value, JsonSerializer serializer)
        {
            var tmp =
                value.Location.Line.ToString(CultureInfo.InvariantCulture)
                + ","
                + value.Location.Column.ToString(CultureInfo.InvariantCulture)
                + ","
                + value.Length.ToString(CultureInfo.InvariantCulture);

            writer.WriteValue(tmp);
        }
    }
}