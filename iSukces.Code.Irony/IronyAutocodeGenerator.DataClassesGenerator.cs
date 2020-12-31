using System;
using System.Collections.Generic;
using System.Linq;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Irony
{
    public partial class IronyAutocodeGenerator
    {
        private class DataClassesGenerator : NestedGeneratorBase
        {
            public DataClassesGenerator(IAutoCodeGeneratorContext context, IronyAutocodeGeneratorModel cfg) : base(
                context, cfg)
            {
            }


            public void Add_DataClasses()
            {
                /*
                void Add_GetMap(CsClass c, NonTerminalInfo i)
                {
                    c.BaseClass = i.GetBaseClass(_cfg.NonTerminals, c.Owner, _cfg.TargetNamespace);
                    var map = (i.Rule as IMap12)?.Map;
                    if (map == null || map.Count <= 0) return;
                    var map2 = string.Join(", ", map.Select(a => a.Index));
                    c.AddMethod("GetMap", "int[]")
                        .WithVisibility(Visibilities.Protected)
                        .WithOverride()
                        .WithBody($"return new [] {{ {map2} }};");
                }
                */

                foreach (var i in _cfg.NonTerminals)
                {
                    if (SkipCreateClass(i))
                        continue;
                    _terminal = i;
                    Create();
                }
            }

            private void Create()
            {
                if (!_terminal.CreateDataClass)
                    return;
                var fullClassName = GetFileLevelTypeNameData(_terminal.DataClassName)?.Name;
                if (string.IsNullOrEmpty(fullClassName))
                    return;
                var c = _context
                    .GetOrCreateClass(fullClassName, CsNamespaceMemberKind.Class)
                    .WithVisibility(Visibilities.Public);

                {
                    var dataClassName = _terminal.DataBaseClassName;
                    if (dataClassName != null)
                    {
                        var baseClassName = dataClassName.GetTypeName(c.Owner, c.GetNamespace());
                        c.BaseClass = baseClassName.Name;
                    }
                }

                void AddProperty(ITerminalNameSource el, string propertyName, bool collection)
                {
                    propertyName = propertyName?.Trim();
                    if (string.IsNullOrEmpty(propertyName))
                        throw new ArgumentException("propertyName is empty");

                    var w            = _cfg.GetAstTypesInfoDelegate(el)?.Invoke(c);
                    var propertyType = w?.DataType;
                    if (string.IsNullOrEmpty(propertyType))
                        throw new Exception(el + " has no data type");
                    propertyType = c.ReduceTypenameIfPossible(propertyType);
                    if (collection)
                        propertyType = MakeList(c, propertyType, typeof(IReadOnlyList<>));

                    ProcessProperty(c.AddProperty(propertyName, propertyType), true);
                }

                c.Description = GetDebugFromRule(_terminal, c, _cfg);
                switch (_terminal.Rule)
                {
                    case RuleBuilder.PlusOrStar plusOrStar:
                        AddProperty(plusOrStar.Element, "Items", true);
                        break;
                    case RuleBuilder.SequenceRule sequenceRule:
                        foreach (var tmp in sequenceRule.Enumerate())
                            if (tmp.Expression is ITerminalNameSource tns)
                            {
                                var propertyName = tmp.Map?.PropertyName;
                                if (string.IsNullOrEmpty(propertyName))
                                    continue;
                                AddProperty(tns, propertyName, false);
                            }

                        break;
                }

                {
                    var builder = new ConstructorBuilder(c);
                    switch (_terminal.DataBaseClassName)
                    {
                        case MethodTypeNameProvider methodCsExpression:
                            break;
                        case StringTypeNameProvider stringTypeNameProvider:
                            break;
                        case TypeTypeNameProvider typeTypeNameProvider:
                            var constructor = typeTypeNameProvider.Type.GetConstructors().FirstOrDefault();
                            builder.AddBaseConstructor(constructor);

                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    foreach (var pi in c.Properties)
                        builder.AddPropertyToSet(pi);

                    builder.CreateConstructor();
                }
            }


            private NonTerminalInfo _terminal;
        }
    }
}