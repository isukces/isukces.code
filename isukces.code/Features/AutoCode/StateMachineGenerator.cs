#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using iSukces.Code.Interfaces;

namespace iSukces.Code.AutoCode;


[AttributeUsage(AttributeTargets.Class)]
// ReSharper disable once ClassNeverInstantiated.Global
public class StateMachineAttribute : Attribute
{
    public StateMachineAttribute(string beginState, string script)
    {
        Script     = script;
        BeginState = beginState;
    }

    public string Script     { get; }
    public string BeginState { get; }
    
    /// <summary>
    /// Call method after exexuting command
    /// </summary>
    public string[]? CommandsWithCustomActions { get; set; } 
}


public class StateMachineGenerator : Generators.SingleClassGenerator<StateMachineAttribute>
{
    private static void AddEnumState(CsClass cl, Graph graph)
    {
        var en = new CsEnum("State")
        {
            UnderlyingType = (CsType)"byte"
        };
        foreach (var i in graph.States)
            en.Items.Add(new CsEnumItem(i));
        cl.AddEnum(en);
    }

    private static void AddFlags(CsClass cl, string enumName, IEnumerable<string> values)
    {
        var en = new CsEnum(enumName);
        en.WithAttribute(new CsAttribute("Flags"));
        en.Items.Add(new CsEnumItem("None", 0));
        var value = 1;
        foreach (var i in values)
        {
            en.Items.Add(new CsEnumItem(i, value));
            value += value;
        }

        en.UnderlyingType = value switch
        {
            <= 0xFF   => (CsType)"byte",
            <= 0xFFFF => (CsType)"ushort",
            _         => (CsType)"uint"
        };

        cl.AddEnum(en);
    }

    private static void CreateExecuteMethod(Item i, CsClass cl)
    {
        var code = new CsCodeWriter();
        {
            var exceptionArg   = $"$\"Unable to execute {i.Name} command at '{{{CurrentState}}}' state\"";
            var throwStatement = "throw new InvalidOperationException(" + exceptionArg + ");";
            var condition      = $"!{i.Name}CanExecute";
            if (i.CustomAction)
                throw new NotImplementedException();
            code.SingleLineIf(condition, throwStatement);
        }

        code.WriteLine($"{CurrentState} = State.{i.ToState};");
        var method = cl.AddMethod(i.Name + "Execute", (CsType)"void");
        method.WithVisibility(Visibilities.Private).WithBody(code);
        method.Description = i.ExecuteDescription;
    }

    protected override void GenerateInternal()
    {
        Action? flush = null;
        var    graph = Graph.ParseScript(Attribute.Script, Attribute.CommandsWithCustomActions);
        var    cl    = Class;
        cl.AddComment("Created by " + GetType().FullName);

        var iniCode    = new CsCodeWriter();
        var updateCode = new CsCodeWriter();

        // === enums and flags
        AddEnumState(cl, graph);
        AddFlags(cl, "MachineMove", graph.Items.Select(a => a.Name));
        AddFlags(cl, "PossibleState", graph.States);

        // === state property
        var currentStateProperty = cl.AddProperty(CurrentState, (CsType)State);
        currentStateProperty.WithOwnSetterAsExpression("UpdateStates(value, false)");

        updateCode.WriteLine("if (newValue == " + currentStateProperty.PropertyFieldName + " && !initialChange) return;");

        iniCode.WriteLine("UpdateStates(State." + Attribute.BeginState + ", true);");
        foreach (var i in graph.Items)
        {
            var canExecute = i.Name + "CanExecute";
            var condition  = string.Join(" || ", i.FromState.Select(a => $"{CurrentState} == {State}.{a}"));
            var variable   = $"save{canExecute}";

            var property = cl.AddProperty(canExecute, (CsType)"bool")
                .WithNoEmitField()
                .WithOwnGetterAsExpression(condition)
                .WithIsPropertyReadOnly();
            AddSetAndNotify(property);
            updateCode.WriteLine($"var {variable} = {canExecute};");
            flush += () =>
            {
                updateCode
                    .SingleLineIf($"{variable} != {property.Name}",
                        $"OnPropertyChanged(nameof({property.Name}));");
            };
            if (i.StepType == ItemType.Command)
            {
                var p = cl.AddProperty($"{i.Name}Command", (CsType)"ICommand")
                    .WithNoEmitField()
                    .WithMakeAutoImplementIfPossible();
                p.SetterVisibility = Visibilities.Private;

                iniCode.Open($"{p.Name} = new ActionUiCommand(_ =>");
                {
                    iniCode.WriteLine("// " + i.ExecuteDescription);
                    iniCode.SingleLineIf($"!{canExecute}", "return;");
                    if (i.CustomAction)
                        iniCode.WriteLine(i.Name + "CommandExecuting();");
                    iniCode.WriteLine($"{CurrentState} = {State}.{i.ToState};");
                }
                iniCode.Close($"}}, _ => {canExecute});");
            }
            else if (i.StepType == ItemType.Method)
            {
                CreateExecuteMethod(i, cl);
            }
        }

        updateCode.WriteLine($"{currentStateProperty.PropertyFieldName} = newValue;");

        var movesAllowed = cl.AddProperty("MovesAllowed", (CsType)"MachineMove");
        SetAutoImplement(movesAllowed);
        var possibleNextState = cl.AddProperty("PossibleNextState", (CsType)"PossibleState");
        SetAutoImplement(possibleNextState);

        var movesAllowedWrapper = new Wrapper(movesAllowed, updateCode);
        flush += movesAllowedWrapper.WriteSetter(graph.MovesAllowed, "MachineMove");

        var possibleNextStateWrapper = new Wrapper(possibleNextState, updateCode);
        flush += possibleNextStateWrapper.WriteSetter(graph.PossibleNextState, "PossibleState");

        updateCode.WriteLine("StateChanged();");
        updateCode.SingleLineIf("initialChange", "return;");

        flush?.Invoke();
        updateCode.WriteLine($"OnPropertyChanged(nameof({currentStateProperty.Name}));");
        cl.AddMethod("InitStateMachine", CsType.Void).WithBody(iniCode);
        var m = cl.AddMethod("UpdateStates", CsType.Void)
            .WithVisibility(Visibilities.Private)
            .WithBody(updateCode);
        m.AddParam("newValue", (CsType)"State");
        m.AddParam("initialChange", (CsType)"bool");

        void AddSetAndNotify(CsProperty prop, Visibilities visibility = Visibilities.Private)
        {
            prop.WithOwnSetterAsExpression($"SetAndNotify(ref {prop.PropertyFieldName}, value)");
            prop.SetterVisibility = visibility;
        }

        void SetAutoImplement(CsProperty prop)
        {
            prop.WithNoEmitField();
            prop.MakeAutoImplementIfPossible = true;
            prop.SetterVisibility            = Visibilities.Private;
        }
    }

    #region Fields

    private const string State = "State";
    private const string CurrentState = "CurrentState";

    #endregion

    private sealed class Wrapper
    {
        public Wrapper(CsProperty prop, CsCodeWriter code)
        {
            _prop         = prop;
            _code         = code;
            _variableName = prop.Name.FirstLower();
        }

        public Action WriteSetter(IReadOnlyDictionary<string, HashSet<string>> dictionary, string enumType)
        {
            _code.WriteLine($"// update {_prop.Name} property");
            _code.WriteLine($"var {_variableName} = {_prop.Name};");
            _code.Open(_prop.Name + " = CurrentState switch");
            foreach (var i in dictionary)
            {
                var values = string.Join(" | ", i.Value.Select(a => $"{enumType}.{a}"));
                _code.WriteLine($"State.{i.Key} => {values},");
            }

            _code.WriteLine($"_ => {enumType}.None");
            _code.Close("};");
            return () =>
            {
                _code.SingleLineIf($"{_variableName} != {_prop.Name}",
                    $"OnPropertyChanged(nameof({_prop.Name}));");
            };
        }

        #region Fields

        private readonly CsProperty _prop;
        private readonly CsCodeWriter _code;
        private readonly string _variableName;

        #endregion
    }

    private sealed class Graph
    {
        private Graph(IEnumerable<Item?> parseScript1)
        {
            Items = parseScript1?.Where(a => a is not null).ToArray() ?? Array.Empty<Item>();
            States = Items.SelectMany(a => a.FromState)
                .Concat(Items.Select(a => a.ToState))
                .Distinct()
                .ToArray();

            var movesAllowed      = new Dictionary<string, HashSet<string>>();
            var possibleNextState = new Dictionary<string, HashSet<string>>();
            foreach (var i in Items)
            {
                foreach (var j in i.FromState)
                {
                    if (!possibleNextState.TryGetValue(j, out var set))
                        possibleNextState[j] = set = new HashSet<string>();
                    set.Add(i.ToState);

                    if (!movesAllowed.TryGetValue(j, out set))
                        movesAllowed[j] = set = new HashSet<string>();
                    set.Add(i.Name);
                }
            }

            MovesAllowed      = movesAllowed;
            PossibleNextState = possibleNextState;
        }

        private static Item? ParseLine(string s, HashSet<string> commandsWithCustomActions)
        {
            var m = SplitRegex.Match(s);
            if (!m.Success)
                return null;
            var name = m.Groups[4].Value.Trim();
            return new Item
            {
                FromState    = m.Groups[1].Value.Split(',').Select(a => a.Trim()).ToArray(),
                ToState      = m.Groups[2].Value.Trim(),
                StepType     = Find(m.Groups[3].Value.Trim()),
                Name         = name,
                CustomAction = commandsWithCustomActions.Contains(name)
            };

            ItemType Find(string text)
            {
                if (text.Equals("command", StringComparison.OrdinalIgnoreCase))
                    return ItemType.Command;
                if (text.Equals("method", StringComparison.OrdinalIgnoreCase))
                    return ItemType.Method;
                throw new Exception("Unknown step type " + text);
            }
        }

        internal static Graph ParseScript(string script, IReadOnlyList<string>? commandsWithCustomActions)
        {
            var tmp = commandsWithCustomActions?.ToHashSet();
            return new Graph(ParseScript1(script, tmp));
        }

        public static IEnumerable<Item?> ParseScript1(string sript, HashSet<string>? commandsWithCustomActions)
        {
            commandsWithCustomActions ??= new HashSet<string>();
            foreach (var i in sript.Split('\n'))
                yield return ParseLine(i, commandsWithCustomActions);
        }

        #region Properties

        public IReadOnlyDictionary<string, HashSet<string>> MovesAllowed      { get; }
        public IReadOnlyDictionary<string, HashSet<string>> PossibleNextState { get; }

        public   IReadOnlyList<string> States { get; }
        internal IReadOnlyList<Item>   Items  { get; }

        #endregion

        #region Fields

        private const string SplitFilter = @"(.*)->\s*([^\s]+)\s*([^\s]+)\s*([^\s]+)";
        private static readonly Regex SplitRegex = new Regex(SplitFilter, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        #endregion
    }

    private sealed class Item
    {
        public override string ToString() => $"{string.Join(",", FromState)} -> {ToState} {StepType} {Name}";

        public IReadOnlyList<string> FromState { get; init; }
        public string                ToState   { get; init; }
        public ItemType              StepType  { get; init; }
        public string                Name      { get; init; }

        public string ExecuteDescription =>
            $"Zmienia stan na '{ToState}' jeśli bieżącm stanem jest {string.Join(" lub ", FromState)}";

        public bool CustomAction { get; init; }
    }

    private enum ItemType
    {
        Method, Command
    }
}
