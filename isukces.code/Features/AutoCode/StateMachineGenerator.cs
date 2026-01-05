using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using iSukces.Code.Interfaces;

namespace iSukces.Code.AutoCode;

[AttributeUsage(AttributeTargets.Class)]
[Conditional("AUTOCODE_ANNOTATIONS")]
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
    ///     Call method after exexuting command
    /// </summary>
    public string[]? CommandsWithCustomActions { get; set; }
}

public class StateMachineGenerator : Generators.SingleClassGenerator<StateMachineAttribute>
{
    private static void AddEnumsAndFlags(CsClass cl, Graph graph)
    {
        // === enums and flags
        AddEnumState(cl, graph);
        AddFlags(cl, "MachineMove", graph.Items.Select(a => a.Name));
        AddFlags(cl, "PossibleState", graph.States);
    }

    private static void AddEnumState(CsClass cl, Graph graph)
    {
        var en = new CsEnum(State)
        {
            UnderlyingType = CsType.Byte
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
            <= 0xFF => CsType.Byte,
            <= 0xFFFF => CsType.Ushort,
            _ => CsType.UInt
        };

        cl.AddEnum(en);
    }

    private ICollection<CsProperty> AddProperties(CsClass cl, Graph graph,
        out CsProperty currentStateProperty)
    {
        // === state property
        currentStateProperty = cl.AddProperty(CurrentState, (CsType)State);
        currentStateProperty.WithOwnSetterAsExpression("UpdateStates(value, false)");
        // === enable property
        var w = new CsCodeWriter()
            .SingleLineIf("field == value", "return;")
            .WriteLine("RunInternal(() => { field = value; });");
        //.WriteLine("field, value;")
        //.Close("});");

        w.WriteLine($"OnPropertyChanged(nameof({IsEnabled}));");
        w.WriteLine(InalidateRequerySuggested);
        cl.AddProperty(IsEnabled, CsType.Bool)
            .WithMakeAutoImplementIfPossible()
            .WithBackingField()
            .WithOwnSetter(w.Code)
            .ConstValue = "true";



        var list = new List<CsProperty>(graph.Items.Count);
        foreach (var i in graph.Items)
        {
            string condition;
            if (i.FromState.Count == 0)
                condition = IsEnabled;
            else if (i.FromState.Count == 1)
            {
                var one = i.FromState[0];
                condition = $"{IsEnabled} && {CurrentState} == State.{one}";
            }
            else
            {
                condition = string.Join(" or ", i.FromState.Select(a => $"State.{a}"));
                condition = $"{IsEnabled} && {CurrentState} is {condition}";
            }

            // var condition = string.Join(" || ", Conditions(i));
            var property = cl.AddProperty(i.Name + "CanExecute", CsType.Bool)
                .WithNoEmitField()
                .WithOwnGetterAsExpression(condition)
                .WithIsPropertyReadOnly();
            list.Add(property);
        }

        return list;
    }

    public string InalidateRequerySuggested { get; set; } = "XCommandManager.InvalidateRequerySuggested();";


    private static void AddRunInternal(CsClass cl, ICollection<CsProperty> cp)
    {
        var     code   = new CsCodeWriter();
        Action? flushX = null;

        var         left             = cp.Count;
        var         thisVariableLeft = cp.Count;
        MaskIntType t                = null!;

        var variable   = "";
        var variableNr = 0;

        ulong mask = 1;
        var variabled = new List<string>();
        foreach (var p in cp)
        {
            if (string.IsNullOrEmpty(variable))
            {
                t        = MaskIntType.Create(Math.Max(17, left));
                variable = variableNr > 0 ? $"saved{variableNr}" : "saved";
                variableNr++;
                mask             =  1;
                thisVariableLeft =  t.Bits - 1;
                left             -= thisVariableLeft;
                variabled.Add(variable);
            }

            var mask1 = t.ConvertToString(mask);
            if (mask == 1)
                code.WriteLine($"var {variable} = {p.Name} ? {t.ConvertToString(1)} : {t.ConvertToString(0)};");
            else
                code.SingleLineIf(p.Name, $"{variable} |= {mask1};");

            var condition = t.GetAnd(variable, mask);
            condition = $"({condition}) ^ {p.Name}";
            flushX += () =>
            {
                code.SingleLineIf(condition,
                    $"OnPropertyChanged(nameof({p.Name}));");
            };
            mask += mask;
            if (--thisVariableLeft == 0)
                variable = "";
        }

        code.WriteLine("action();");
        flushX?.Invoke();
        /*{
            var variables = variabled.Select(q => q + " > 0");
            var b         = string.Join(" || ", variables);
            code.WriteLine("return " + b + ";");
        }*/
        cl.AddMethod("RunInternal", CsType.Void)
            .WithBody(code)
            .WithParameter("action", (CsType)"Action");
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
        var graph = Graph.ParseScript(Attribute.Script, Attribute.CommandsWithCustomActions);
        var cl    = Class;
        cl.AddComment("Created by " + GetType().FullName);
        var canProperties = AddProperties(cl, graph, out var currentStateProperty);
        AddRunInternal(cl, canProperties);
        AddEnumsAndFlags(cl, graph);


        var     currentStateField = currentStateProperty.PropertyFieldName;
        var     updateCode   = new CsCodeWriter();
        var     initCode     = new CsCodeWriter();
        Action? flush        = null;
        updateCode.SingleLineIf($"newValue == {currentStateField} && !initialChange",
            "return;");
        updateCode.Open("RunInternal(() =>");
        initCode.WriteLine("UpdateStates(State." + Attribute.BeginState + ", true);");
        foreach (var i in graph.Items)
        {
            var canExecute = i.Name + "CanExecute";
            switch (i.StepType)
            {
                case ItemType.Command:
                    var p = cl.AddProperty($"{i.Name}Command", (CsType)"ICommand")
                        .WithNoEmitField()
                        .WithMakeAutoImplementIfPossible();
                    p.SetterVisibility = Visibilities.Private;

                    initCode.Open($"{p.Name} = new ActionUiCommand(_ =>");
                    initCode.WriteLine("// " + i.ExecuteDescription);
                    initCode.SingleLineIf($"!{canExecute}", "return;");
                    if (i.CustomAction)
                        initCode.WriteLine(i.Name + "CommandExecuting();");
                    initCode.WriteLine($"{CurrentState} = {State}.{i.ToState};");
                    initCode.Close($"}}, _ => {canExecute});");
                    break;
                case ItemType.Method:
                    CreateExecuteMethod(i, cl);
                    break;
            }
        }

        updateCode.WriteLine($"{currentStateField} = newValue;");

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

        flush.Invoke();

        updateCode.WriteLine($"OnPropertyChanged(nameof({CurrentState}));");
        updateCode.WriteLine(InalidateRequerySuggested);

        updateCode.Close("});");
        // updateCode.SingleLineIf("changed", InalidateRequerySuggested);
        {
            cl.AddMethod("InitStateMachine", CsType.Void)
                .WithBody(initCode);

        }
        {
            var m = cl.AddMethod("UpdateStates", CsType.Void)
                .WithVisibility(Visibilities.Private)
                .WithBody(updateCode);

            m.AddParam("newValue", (CsType)"State");
            m.AddParam("initialChange", CsType.Bool);
        }
        return;

        void SetAutoImplement(CsProperty prop)
        {
            prop.WithNoEmitField();
            prop.MakeAutoImplementIfPossible = true;
            prop.SetterVisibility            = Visibilities.Private;
        }
    }

    private const string State = nameof(State);
    private const string CurrentState = nameof(CurrentState);
    private const string IsEnabled = nameof(IsEnabled);

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
            _code.Open($"{_prop.Name} = {CurrentState} switch");
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

        private readonly CsProperty _prop;
        private readonly CsCodeWriter _code;
        private readonly string _variableName;
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
            foreach (var i in sript.SplitToLines())
                yield return ParseLine(i, commandsWithCustomActions);
        }

        public IReadOnlyDictionary<string, HashSet<string>> MovesAllowed      { get; }
        public IReadOnlyDictionary<string, HashSet<string>> PossibleNextState { get; }

        public   IReadOnlyList<string> States { get; }
        internal IReadOnlyList<Item>   Items  { get; }

        private const string SplitFilter = @"(.*)->\s*([^\s]+)\s*([^\s]+)\s*([^\s]+)";
        private static readonly Regex SplitRegex = new Regex(SplitFilter, RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }

    private sealed class Item
    {
        public override string ToString()
        {
            return $"{string.Join(",", FromState)} -> {ToState} {StepType} {Name}";
        }

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