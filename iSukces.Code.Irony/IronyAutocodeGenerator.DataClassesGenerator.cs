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
                foreach (var i in _cfg.NonTerminals)
                {
                    if (SkipCreateClass(i))
                        continue;
                    _token = i;
                    Create();
                }
            }

            private void AddProperty(ITokenNameSource el, string propertyName, bool collection)
            {
                var a = _cfg.GetTokenInfoByName(el);
                if (a != null)
                    if (a.IsNoAst)
                        return;

                propertyName = propertyName?.Trim();
                if (string.IsNullOrEmpty(propertyName))
                    throw new ArgumentException("propertyName is empty");

                var w            = _cfg.GetAstTypesInfoDelegate(el)?.Invoke(_dataClass);
                var propertyType = w?.DataType;
                if (string.IsNullOrEmpty(propertyType))
                    throw new Exception(el + " has no data type");
                propertyType = _dataClass.ReduceTypenameIfPossible(propertyType);
                if (collection)
                    propertyType = MakeList(_dataClass, propertyType, typeof(IReadOnlyList<>));

                ProcessProperty(_dataClass.AddProperty(propertyName, propertyType), true);
            }

            private void AddToString(string expression)
            {
                _dataClass.AddMethod("ToString", "string")
                    .WithOverride()
                    .WithVisibility(Visibilities.Public)
                    .WithBodyAsExpression(expression);
            }

            private void Create()
            {
                if (!_token.DataClass.CreateAutoCode)
                    return;
                var fullClassName = GetFileLevelTypeNameData(_token.DataClass.Provider)?.Name;
                if (string.IsNullOrEmpty(fullClassName))
                    return;
                _dataClass = _context
                    .GetOrCreateClass(fullClassName, CsNamespaceMemberKind.Class)
                    .WithVisibility(Visibilities.Public);
                _token.CreationInfo.DataClass = _dataClass;

                {
                    var dataClassName = _token.DataBaseClassName;
                    if (dataClassName != null)
                    {
                        var baseClassName = dataClassName.GetTypeName(_dataClass.Owner, _dataClass.GetNamespace());
                        _dataClass.BaseClass = baseClassName.Name;
                    }
                }

                _dataClass.Description = GetDebugFromRule(_token, _dataClass, _cfg);
                switch (_token.Rule)
                {
                    case RuleBuilder.Alternative alternative:
                        Process_Alternative(alternative);
                        break;
                    case RuleBuilder.PlusOrStar plusOrStar:
                        Process_PlusOrStar(plusOrStar);
                        break;
                    case RuleBuilder.SequenceRule sequenceRule:
                        foreach (var tmp in sequenceRule.Enumerate())
                            if (tmp.Expression is ITokenNameSource tns)
                            {
                                var propertyName = tmp.Map?.PropertyName;
                                if (string.IsNullOrEmpty(propertyName))
                                    continue;
                                AddProperty(tns, propertyName, false);
                            }

                        break;
                }

                {
                    var constructorBuilder = new ConstructorBuilder(_dataClass);
                    switch (_token.DataBaseClassName)
                    {
                        case MethodTypeNameProvider methodCsExpression:
                            break;
                        case StringTypeNameProvider stringTypeNameProvider:
                            break;
                        case TypeNameProvider typeTypeNameProvider:
                            var constructor = typeTypeNameProvider.Type.GetConstructors().FirstOrDefault();
                            constructorBuilder.AddBaseConstructor(constructor);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    foreach (var pi in _dataClass.Properties)
                        constructorBuilder.AddPropertyToSet(pi);

                    _token.CreationInfo.DataConstructor = constructorBuilder;
                    constructorBuilder.CreateConstructor();
                }
            }

            private void Process_Alternative(RuleBuilder.Alternative rule)
            {
                var propertyName = "TmpValue";
                var p            = ProcessProperty(_dataClass.AddProperty(propertyName, "object"), false);
                var alts         = rule.GetAlternatives();
                // p.Description = alternative.AlternativeInterfaceName;
                var e = rule.CreationInfo.Enum1;
                if (e != null)
                {
                    var n  = e.GetFullName();
                    var nn = _dataClass.ReduceTypenameIfPossible(n.FullName);
                    ProcessProperty(_dataClass.AddProperty("NodeKind", nn), true);
                }

                AddToString($"{propertyName}?.ToString() ?? string.Empty");
            }

            private void Process_PlusOrStar(RuleBuilder.PlusOrStar rule)
            {
                AddProperty(rule.Element, "Items", true);
                AddToString("string.Join(\".\", Items)");
            }


            private NonTerminalInfo _token;
            private CsClass _dataClass;
        }
    }
}