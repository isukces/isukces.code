using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using isukces.code.CodeWrite;
using isukces.code.interfaces;

namespace isukces.code.AutoCode
{
    public partial class Generators
    {
        internal class LazyGenerator : SingleClassGenerator, IAutoCodeGenerator
        {
            private static string CoalesceNotEmpty(ISet<string> accept, params string[] items)
            {
                if ((items == null) || (items.Length == 0)) return null;
                for (var index = 0; index < items.Length; index++)
                {
                    var tmp = items[index]?.Trim();
                    if (string.IsNullOrEmpty(tmp) || ((accept != null) && accept.Contains(tmp)))
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
                    if (n.ToLower().StartsWith(i))
                    {
                        n = n.Substring(i.Length);
                        if (!string.IsNullOrEmpty(n))
                            return n;
                    }
                    if (n.ToLower().EndsWith(i))
                    {
                        n = n.Substring(0, n.Length - i.Length);
                        if (!string.IsNullOrEmpty(n))
                            return n;
                    }
                }

                return mi.Name + "UNKNOWNNAME";
            }

            private static string GetCalculatedValue(MemberInfo mi)
            {
                var methodInfo = mi as MethodInfo;
                // todo: parametry
                if (methodInfo != null)
                    return $"{methodInfo.Name}()";
                var propertyInfo = mi as PropertyInfo;
                if (propertyInfo != null)
                    return $"{propertyInfo.Name}()";
                throw new NotSupportedException();
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
                if (mi is PropertyInfo)
                    return ((PropertyInfo)mi).PropertyType;
                if (mi is MethodInfo)
                    return ((MethodInfo)mi).ReturnType;
                throw new NotSupportedException(mi?.GetType().Name ?? "empty");
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

            private void ProcessLazy()
            {
                // todo: obsługa metod i własności z parametrami
                var pm = ScanMethods(Type);
                var properties = ScanProperties(Type);
                if ((pm.Count == 0) && (properties.Count == 0)) return;
                if (pm.Any())
                    foreach (var i in pm)
                        WriteSingle(i.Item1, i.Item2);

                if (properties.Any())
                    foreach (var i in properties)
                        WriteSingle(i.Item1, i.Item2);
            }

            private AssignStrategy GetAssignStrategy(Type t)
            {
                if (t
#if COREFX
                    .GetTypeInfo()
#endif
                    
                    .IsValueType)
                {
                    var ft = typeof(Tuple<>).MakeGenericType(t);
                    var ftn = Class.TypeName(ft);
                    return new AssignStrategy
                    {
                        FieldType = ft,
                        Format1 = "new " + ftn + "({0})",
                        Format2 = "{0}.Item1"
                    };
                }
                return new AssignStrategy
                {
                    FieldType = t
                };
            }

            private void WriteSingle(MemberInfo mi, Auto.LazyAttribute at)
            {
                var used = new HashSet<string>();
                // nazwa własności/metody
                var baseName = GetBaseName(mi, at);
                var syncName = CoalesceNotEmpty(used,
                    at.SyncObjectName, GeneratorsHelper.FieldName(baseName + "Sync"));
                var fieldName = CoalesceNotEmpty(used,
                    at.FieldName, GeneratorsHelper.FieldName(baseName), GeneratorsHelper.FieldName(baseName + "Data"));
                var resultType = GetResultType(mi);
                var assignS = GetAssignStrategy(resultType);
                var isProperty = GetIsProperty(mi, at);
                var callCalulatedValue = GetCalculatedValue(mi);

                // sync field
                if (at.DeclareAndCreateSyncObject)
                {
                    var f = Class.AddField(syncName, typeof(object));
                    f.IsStatic = at.StaticSyncObject;
                    f.IsReadOnly = true;
                    f.ConstValue = "new object()";
                    f.Visibility = Visibilities.Private;

                    f = Class.AddField(fieldName, assignS.FieldType);
                    f.IsVolatile = true;
                    f.Visibility = Visibilities.Private;
                }
                if (isProperty)
                {
                    var prop = Class.AddProperty(baseName, resultType);
                    prop.IsStatic = mi.IsMemberStatic();
                    prop.IsPropertyReadOnly = true;
                    prop.EmitField = false;
                    ICsCodeFormatter writer = new CsCodeFormatter();
                    {
                        writer.WriteLine("var result = {0};", fieldName);
                        writer.WriteLine("// ReSharper disable once InvertIf");
                        writer.WriteLine("if (result == null)");
                        writer.OpenBrackets();
                        {
                            writer.WriteLine("lock({0})", syncName);
                            writer.OpenBrackets();
                            {
                                writer.WriteLine("// ReSharper disable once ConditionIsAlwaysTrueOrFalse");
                                writer.WriteLine("if (result == null)");
                                writer.Indent++;
                                writer.WriteLine("{0} = result = {1};", fieldName,
                                    assignS.Assign1(callCalulatedValue));
                                writer.Indent--;
                            }
                            writer.CloseBrackets();
                        }
                        writer.CloseBrackets();
                        writer.WriteLine("return {0};", assignS.Assign2("result"));
                    }
                    prop.OwnGetter = writer.Code;
                }
            }


            private class AssignStrategy
            {
                public string Assign1(string x)
                {
                    return string.Format(Format1, x);
                }

                public string Assign2(string x)
                {
                    return string.Format(Format2, x);
                }

                public Type FieldType { get; set; }
                public string Format1 { get; set; } = "{0}";
                public string Format2 { get; set; } = "{0}";
            }

            public void Generate(Type type, IAutoCodeGeneratorContext context)
            {
                Setup(type, context);
                ProcessLazy();
            }


        }
    }
}