#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using iSukces.Code.Interfaces;

namespace iSukces.Code.AutoCode;

public partial class Generators
{
    public class LazyGenerator : SingleClassGenerator, IAutoCodeGenerator
    {
        // private CsMethod _initLazyMethod;

        private static string? CoalesceNotEmpty(ISet<string> accept, params string[]? items)
        {
            if (items == null || items.Length == 0) return null;
            for (var index = 0; index < items.Length; index++)
            {
                var tmp = items[index]?.Trim();
                if (string.IsNullOrEmpty(tmp) || accept != null && accept.Contains(tmp))
                    continue;
                accept?.Add(tmp);
                return tmp;
            }

            return null;
        }

        private static string GetBaseName(MemberInfo mi, Auto.LazyAttribute at)
        {
            var n = at.Name?.Trim();
            if (!string.IsNullOrEmpty(n))
                return n;
            n = mi.Name;
            foreach (var i in "lazyinternal,internallazy,lazy,internal".Split(','))
            {
                if (n.ToLower().StartsWith(i, StringComparison.Ordinal))
                {
                    n = n.Substring(i.Length);
                    if (!string.IsNullOrEmpty(n))
                        return n;
                }

                if (n.ToLower().EndsWith(i, StringComparison.Ordinal))
                {
                    n = n.Substring(0, n.Length - i.Length);
                    if (!string.IsNullOrEmpty(n))
                        return n;
                }
            }

            return $"{mi.Name}UNKNOWNNAME";
        }

        private static string GetCalculatedValue(MemberInfo mi)
        {
            switch (mi)
            {
                // todo: parametry
                case MethodInfo methodInfo:
                    return $"{methodInfo.Name}()";
                case PropertyInfo propertyInfo:
                    return $"{propertyInfo.Name}()";
                default:
                    throw new NotSupportedException();
            }
        }

        private static string GetFullPropertyGetterImplementation(string fieldName, string syncName,
            AssignStrategy assignS, string callCalulatedValue)
        {
            ICsCodeWriter writer = new CsCodeWriter();
            writer.WriteLine("var result = {0};", fieldName);
            // writer.WriteLine("// ReSharper disable once InvertIf");
            writer.WriteLine("if (result == null)");
            writer.IncIndent();
            {
                writer.WriteLine("lock({0})", syncName);
                writer.IncIndent();
                {
                    // writer.WriteLine("// ReSharper disable once ConditionIsAlwaysTrueOrFalse");
                    writer.WriteLine("if (result == null)");
                    writer.Indent++;
                    writer.WriteLine("{0} = result = {1};", fieldName,
                        assignS.Assign1(callCalulatedValue));
                    writer.Indent--;
                }
                writer.DecIndent();
            }
            writer.DecIndent();
            writer.WriteLine("return {0};", assignS.Assign2("result"));
            return writer.Code;
        }

        private static bool GetIsProperty(MemberInfo mi, Auto.LazyAttribute at)
        {
            var type = at.Target;
            return type == Auto.LazyMemberType.Auto
                ? mi is PropertyInfo
                : type == Auto.LazyMemberType.Property;
        }

        private static Type GetResultType(MemberInfo mi)
        {
            switch (mi)
            {
                case PropertyInfo info:
                    return info.PropertyType;
                case MethodInfo info:
                    return info.ReturnType;
                default:
                    throw new NotSupportedException(mi?.GetType().Name ?? "empty");
            }
        }

        private static List<Tuple<MethodInfo, Auto.LazyAttribute>> ScanMethods(Type type)
        {
            var list = new List<Tuple<MethodInfo, Auto.LazyAttribute>>();
            foreach (var methodInfo in type
#if COREFX
                    .GetTypeInfo()
#endif
                         .GetMethods(GeneratorsHelper.All))
            {
                if (methodInfo.DeclaringType != type) continue;
                var attribute = methodInfo.GetCustomAttribute<Auto.LazyAttribute>();
                if (attribute != null)
                    list.Add(Tuple.Create(methodInfo, attribute));
            }

            return list;
        }

        private static List<Tuple<PropertyInfo, Auto.LazyAttribute>> ScanProperties(Type type)
        {
            var list = new List<Tuple<PropertyInfo, Auto.LazyAttribute>>();
            foreach (var propertyInfo in type
#if COREFX
                    .GetTypeInfo()
#endif
                         .GetProperties(GeneratorsHelper.All))
            {
                if (propertyInfo.DeclaringType != type) continue;
                var attribute = propertyInfo.GetCustomAttribute<Auto.LazyAttribute>();
                if (attribute != null)
                    list.Add(Tuple.Create(propertyInfo, attribute));
            }

            return list;
        }

        protected override void GenerateInternal()
        {
            // todo: obsługa metod i własności z parametrami
            var pm         = ScanMethods(Type);
            var properties = ScanProperties(Type);
            if (pm.Count == 0 && properties.Count == 0) return;
            if (pm.Any())
                foreach (var i in pm)
                    WriteSingle(i.Item1, i.Item2);

            if (properties.Any())
                foreach (var i in properties)
                    WriteSingle(i.Item1, i.Item2);
            //_initLazyMethod = null;
        }

        private void AddSyncField(Auto.LazyAttribute at, string fieldName)
        {
            if (!at.DeclareAndCreateSyncObject) return;
            var existing = Class.Fields.FirstOrDefault(a => a.Name == fieldName);
            if (existing != null)
            {
                if (existing.IsStatic == at.StaticSyncObject)
                    return;
                throw new Exception($"Sync object {fieldName} can't be both static and instance");
            }

            var f = Class.AddField(fieldName, typeof(object))
                .WithStatic(at.StaticSyncObject)
                .WithIsReadOnly()
                .WithVisibility(Visibilities.Private)
                .WithConstValue("new object()");
        }

        private AssignStrategy GetAssignStrategy(Type t, bool canUseLazy)
        {
            if (canUseLazy)
            {

                var ft  = typeof(Lazy<>).MakeGenericType(t);
                var ftn = Class.GetTypeName(ft).Declaration;
                return new AssignStrategy
                {
                    FieldType = ft,
                    Format1   = $"new {ftn}({{0}}) /*a*/",
                    Format2   = "{0}.Item1 *b*/"
                };

            }

            if (t
#if COREFX
                    .GetTypeInfo()
#endif
                .IsValueType)
            {
                var ft  = typeof(Tuple<>).MakeGenericType(t);
                var ftn = Class.GetTypeName(ft).Declaration;
                return new AssignStrategy
                {
                    FieldType = ft,
                    Format1   = $"new {ftn}({{0}})",
                    Format2   = "{0}.Item1"
                };
            }

            return new AssignStrategy
            {
                FieldType = t
            };
        }

        const string ErrorMessageConstName = "LazyNotInitializedMessage";

        private void WriteSingle(MemberInfo mi, Auto.LazyAttribute at)
        {
            var useSystemLazy = at.UseLazyObject
                                && string.IsNullOrEmpty(at.SyncObjectName)
                                && string.IsNullOrEmpty(at.ClearMethodName);

            var used = new HashSet<string>();
            // nazwa własności/metody
            var baseName = GetBaseName(mi, at);
            var syncName = CoalesceNotEmpty(used,
                at.SyncObjectName, GeneratorsHelper.FieldName($"{baseName}Sync"));
            var fieldName = CoalesceNotEmpty(used,
                at.FieldName, GeneratorsHelper.FieldName(baseName), GeneratorsHelper.FieldName($"{baseName}Data"));

            var resultType         = GetResultType(mi);
            var isProperty         = GetIsProperty(mi, at);
            var callCalulatedValue = GetCalculatedValue(mi);

            if (!useSystemLazy)
                AddSyncField(at, syncName);

            var assignS = GetAssignStrategy(resultType, useSystemLazy);
            var fieldType = useSystemLazy
                ? typeof(Lazy<>).MakeGenericType(resultType)
                : assignS.FieldType;
            var f = Class.AddField(fieldName, fieldType)
                .WithVisibility(Visibilities.Private)
                .WithIsVolatile(!useSystemLazy);
            if (useSystemLazy)
            {
                f.IsReadOnly = true;
                var typeName = Class.GetTypeName(assignS.FieldType);
                var init     = typeName.New(mi.Name);
                //var init     = $"new {typeName.Declaration}({mi.Name})";
                if (mi.IsMemberStatic())
                    f.ConstValue = init;
                else
                {
                    f.IsReadOnly = false;
                    GeneratorsHelper.AddInitCode(Class, $"{f.Name} = {init};");
                }
            }

            string code;
            if (useSystemLazy)
            {
                    

                if (Class.Fields.All(a => a.Name != ErrorMessageConstName))
                {
                    const string msg = $"Lazy not initialized. Call {GeneratorsHelper.AutoCodeInitMethodName} method in constructor.";
                    Class.AddField(ErrorMessageConstName, CsType.String)
                        .WithVisibility(Visibilities.Private)
                        .WithConstValue(msg.CsEncode()).IsConst = true;
                }

                var exception = Class.GetTypeName<Exception>().New(ErrorMessageConstName);
                var cw = new CsCodeWriter()
                    .WriteLine($"if (ReferenceEquals({fieldName}, null)) throw {exception};")
                    .WriteLine($"return {fieldName}.Value;");
                code = cw.Code;
            }
            else
                code = GetFullPropertyGetterImplementation(fieldName, syncName, assignS, callCalulatedValue);
            if (isProperty)
            {
                var prop = Class.AddProperty(baseName, resultType);
                prop.IsStatic           = mi.IsMemberStatic();
                prop.IsPropertyReadOnly = true;
                prop.EmitField          = false;
                prop.OwnGetter          = code;
            }
            else
                Class.AddMethod(baseName, resultType)
                    .WithStatic(mi.IsMemberStatic())
                    .WithBody(code);
        }

        private class AssignStrategy
        {
            public string Assign1(string x) => string.Format(Format1, x);

            public string Assign2(string x) => string.Format(Format2, x);

            public Type   FieldType { get; set; }
            public string Format1   { get; set; } = "{0}";
            public string Format2   { get; set; } = "{0}";
        }
    }
}