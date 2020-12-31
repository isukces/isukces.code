using System.Collections.Generic;
using System.Linq;
using Irony.Parsing;

namespace Samples.Irony.AmmyGrammar
{
    public class IronyDebugger
    {
        public IronyDebugger(LanguageData language)
        {
            _language = language;
            _dict     = _language.ParserData.States.ToDictionary(a => a.Name, a => a);
        }

        public static void Debug(ParserState iState, LanguageData language)
        {
            new IronyDebugger(language).Debug(iState);
        }

        public static IEnumerable<string> GetDebugLines(ParserState state, LanguageData language) =>
            new IronyDebugger(language).GetDebugLines(state);
        public static IEnumerable<string> GetDebugLines(string state, LanguageData language) =>
            new IronyDebugger(language).GetDebugLines(state);

        private static string Get(Production production) => production.ToString();

        public void Debug(ParserState iState)
        {
            foreach (var q in GetDebugLines(iState))
                System.Diagnostics.Debug.WriteLine(q);
        }

        public IEnumerable<string> DebugStatesScan(string stateName, int level = 0)
        {
            foreach (var langState in _language.ParserData.States)
            foreach (var pair in langState.Actions)
            {
                if (!(pair.Value is ShiftParserAction spa)) continue;
                if (stateName != spa.NewState.Name) continue;
                var to = $"{langState.Name} + '{pair.Key}' goes to {stateName}";
                if (level > 8)
                {
                    yield return "..." + to;
                }
                else
                {
                    var tmp  = DebugStatesScan(langState.Name, level + 1);
                    var flag = true;
                    foreach (var i in tmp)
                    {
                        flag = false;
                        yield return i + "\r\n  " + to;
                    }

                    if (flag)
                        yield return to;
                }
            }
        }


        public ParserState Find(string name)
        {
            _dict.TryGetValue(name, out var x);
            return x;
        }


        public IEnumerable<string> GetDebugLines(string state)
        {
            var q = Find(state);
            return GetDebugLines(q);
        }
        public IEnumerable<string> GetDebugLines(ParserState state)
        {
            if (state is null)
                yield break;
            yield return "States for " + state.Name;
            foreach (var q in DebugStatesScan(state.Name))
                yield return "   " + q;
            yield return "Actions";

            foreach (var j in state.Actions)
            {
                var line = "    '" + j.Key.Name + "' " + GetDesription(j.Value);
                yield return line;
            }
        }

        private string GetDesription(ParserAction parserAction)
        {
            string si(ParserState spaNewState)
            {
                var q = Find(spaNewState.Name);
                return spaNewState.Name;
            }

            switch (parserAction)
            {
                case ReduceParserAction reduceParserAction:
                    return "reduces to " + Get(reduceParserAction.Production);
                case AcceptParserAction acceptParserAction:
                    return "accepts " + acceptParserAction;
                case ShiftParserAction spa:
                    return "shifts to " + si(spa.NewState);
                default:
                    return " -> ???";
            }
        }

        private readonly LanguageData _language;
        private readonly Dictionary<string, ParserState> _dict;
    }
}