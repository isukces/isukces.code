using System;
using System.Linq;
using Irony;
using Irony.Interpreter;
using Irony.Parsing;

namespace Bitbrains.AmmyParser.Tests
{
    public class AmmyParserTestsBase
    {
        protected static LanguageData GetLanguageData()
        {
            var grammar  = new AmmyGrammar();
            var language = new LanguageData(grammar);
            foreach (var i in language.Errors)
                throw new Exception(i.Message);
            return language;
        }
        protected static AmmyCode ParseTree(LanguageData language, string sourceCode)
        {
            var parser  = new Parser(language);
            var tree    = parser.Parse(sourceCode);
            var runtime = new LanguageRuntime(language);
            var app     = new ScriptApp(runtime);
            if (tree.Status == ParseTreeStatus.Error)
            {
                var msg = tree.ParserMessages.LastOrDefault(a => a.Level == ErrorLevel.Error);
                if (msg!= null)
                    throw new Exception(msg.Message + " at " + msg.Location);
                var errorToken = parser.Context.CurrentParseTree.Tokens.LastOrDefault(a => a.Category == TokenCategory.Error);
                if (errorToken != null)
                    throw new Exception(errorToken.ValueString + " at " + errorToken.Location);

                throw new Exception("other");
            }
            
            var rootAstNode = (AstAmmyCode)tree.Root.AstNode;
            var o           = rootAstNode.Evaluate(new ScriptThread(app));
            return (AmmyCode)o;
        }

    }
}