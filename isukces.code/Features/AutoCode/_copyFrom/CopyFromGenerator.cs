#if !COREFX || NET80
#define HAS_ICLONEABLE
#endif
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using iSukces.Code.Interfaces;

namespace iSukces.Code.AutoCode;

public class CopyFromGenerator : Generators.SingleClassGenerator, IAutoCodeGenerator
{
    protected static ConstructorInfo GetConstructor(Type type)
    {
        var c = type.GetConstructors(GeneratorsHelper.AllInstance);
        if (c.Length == 1)
            return c[0];

        var constructors2 = c
            .Where(a => a.GetCustomAttribute<Auto.CloneableConstructorAttribute>() is not null)
            .ToArray();
        if (constructors2.Length == 1)
            return constructors2[0];
        if (constructors2.Length == 0)
        {
            constructors2 = c.Where(a => a.GetParameters().Length == 0).ToArray();
            if (constructors2.Length == 1)
                return constructors2[0];
            throw new UnableToFindConstructorException(type, "Mark one with Auto.CloneableConstructor");
        }

        throw new UnableToFindConstructorException(type, "Too many marked with Auto.CloneableConstructor");
    }

    protected static string GetConstructorParameters(Type type, ConstructorInfo constr)
    {
        var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .ToDictionary(a => a.Name, a => a, StringComparer.OrdinalIgnoreCase);
        var l = constr.GetParameters()
            .Select(i =>
            {
                if (props.TryGetValue(i.Name ?? throw new InvalidOperationException(), out var pi))
                    return pi.Name;
                throw new Exception("Unable to find property related to constructor parameter " + i.Name);
            }).ToArray();
        var pars = l.CommaJoin();
        return pars;
    }

    private static void CopyArray(PropertyInfo pi, string type, ICsCodeWriter writer, ITypeNameResolver res)
    {
        var wm = GeneratorsHelper.GetWriteMemeberName(pi);
        writer.WriteLine("if (source.{0} is null)", pi.Name);
        writer.WriteLine("    {0} = null;", wm);
        writer.WriteLine("else {");
        writer.IncIndent();
        {
            var target = "target" + pi.Name;
            var source = "source" + pi.Name;
            writer.WriteLine($"var {source} = source.{pi.Name};");
            if (type == "System.Windows.Point")
            {
                var len = $"[{source}.Length]";
                writer.WritelineNoIndent(CompilerDirectives.If + " COREFX");
                writer.WriteLine($"var {target} = new Compat.{type}{len};");
                writer.WritelineNoIndent(CompilerDirectives.Else);
                writer.WriteLine($"var {target} = new {type}{len};");
                writer.WritelineNoIndent(CompilerDirectives.EndIf);
            }
            else
            {
                writer.WriteLine($"var {target} = new {type}[{source}.Length];");
            }

            var arrayCopy = res.GetTypeName<Array>().GetMemberCode(nameof(Array.Copy));
            writer.WriteLine($"{arrayCopy}({source}, 0, {target}, 0, {source}.Length);");
            writer.WriteLine($"{wm} = {target};");
        }

        writer.DecIndent();
        writer.WriteLine("}");
    }

    private static void GenerateMethodClone(Type type, CsClass csClass)
    {
        csClass.ImplementedInterfaces.Add((CsType)nameof(ICloneable));
        var cm     = csClass.AddMethod("Clone", CsType.Object, "Makes clone of object");
        var constr = GetConstructor(type);
        var pars   = GetConstructorParameters(type, constr);

        ICsCodeWriter writer = new CsCodeWriter();
        writer.WriteLine("var a = new {0}({1});", type.Name, pars);
        writer.WriteLine("a.CopyFrom(this);");
        writer.WriteLine("return a;");
        cm.Body = writer.Code;
    }
        
    protected internal static Type? TryGetRankOneArrayElement(Type type)
    {
        if (!type.IsArray) return null;
        var rank = type.GetArrayRank();
        if (rank != 1) return null;
        var el = type.GetElementType();
        if (el is null) return null;
        return el.IsValueType || el == typeof(string) ? el : null;
    }

    protected override void GenerateInternal()
    {
        _copyFromAttribute = GetCustomAttribute<Auto.CopyFromAttribute>();
        _doCloneable       = GetCustomAttribute<Auto.CloneableAttribute>() is not null;

        if (!_doCloneable && _copyFromAttribute is null)
            return;
        {
            var cm = Class.AddMethod("CopyFrom", CsType.Void);
            cm.AddParam("source", Class.GetTypeName(Type)); // reduce type
            ICsCodeWriter writer = new CsCodeWriter();
            writer.WriteLine("if (ReferenceEquals(source, null))");
            writer.WriteLine("    throw new ArgumentNullException(nameof(source));");
            var properties = Class.DotNetType
#if COREFX
                                      .GetTypeInfo()
#endif
                .GetProperties(GeneratorsHelper.AllInstance);
            properties = SortProperties(properties);
            foreach (var i in properties)
                ProcessProperty(i, _copyFromAttribute, writer);
            cm.Body = writer.Code;
        }
        if (_doCloneable)
            GenerateMethodClone(Type, Class);
    }
        
        
    protected virtual void ProcessProperty(PropertyInfo pi, Auto.CopyFromAttribute? attr, ICsCodeWriter writer)
    {
        ITypeNameResolver resolver               = Class;
        var               allowReferenceNullable = Class.AllowReferenceNullable();
        {
            var at = pi.GetCustomAttribute<Auto.CopyFromByMethodAttribute>();
            if (at is not null)
            {
                var typename1    = resolver.GetTypeName<CopyPropertyValueArgs>();
                var typenameDot2 = at.Type != Type ? resolver.GetTypeName(at.Type) + "." : "";
                var arg          = typename1.New($"source, this, nameof({pi.Name})");
                var c            = $"{typenameDot2}{at.MethodName}({arg});";
                writer.WriteLine(c);
                return;
            }
        }
        if (pi.GetCustomAttribute<CopyByReferenceAttribute>() is not null)
        {
            writer.WriteLine($"{pi.Name} = source.{pi.Name}; // by reference");
            return;
        }
        if (pi.PropertyType
#if COREFX
                  .GetTypeInfo()
#endif
                .IsValueType || pi.PropertyType == typeof(string))
        {
            if (!pi.CanWrite || !pi.CanRead)
                return;

            writer.WriteLine("{0} = source.{0}; // {1}", pi.Name, resolver.GetTypeName(pi.PropertyType).Modern);
            return;
        }

        if (pi.PropertyType.IsArray)
        {
            var eltype = pi.PropertyType.GetElementType();
            var type   = resolver.GetTypeName(eltype).AsString(allowReferenceNullable);
            CopyArray(pi, type, writer, resolver);
            return;
        }

        {
            var tmp = pi.GetCustomAttribute<Auto.CopyBy.CloneableAttribute>();
            if (tmp is not null)
            {
                writer.WriteLine("{0} = ({1})((ICloneable)source.{0})?.Clone(); // BY Icloneable {1}", pi.Name,
                    pi.PropertyType);
                return;
            }
        }
        {
            if (attr is not null && attr.HasSkip(pi.Name))
                return;
            var tmp = pi.GetCustomAttribute<Auto.CopyBy.ReferenceAttribute>();
            if (tmp is not null || attr is not null && attr.HasCopyByReference(pi.Name))
            {
                writer.WriteLine("{0} = source.{0}; // BY REF {1}", pi.Name, pi.PropertyType);
                return;
            }
        }
        {
            var tmp = pi.GetCustomAttribute<Auto.CopyBy.ValuesProcessorAttribute>();
            if (tmp is not null)
            {
                CloneWithValuesProcessor(pi, writer, resolver);
                return;
            }
        }
        if (pi.PropertyType
#if COREFX
                  .GetTypeInfo()
#endif
            .IsInterface)
        {
            CloneWithValuesProcessor(pi, writer, resolver);
            return;
        }

        var ptg = pi.PropertyType;
        if (pi.PropertyType
#if COREFX
                  .GetTypeInfo()
#endif
            .IsGenericType)
            ptg = pi.PropertyType.GetGenericTypeDefinition();

        if (ptg == typeof(ObservableCollection<>))
        {
            var writeMemeber = GeneratorsHelper.GetWriteMemeberName(pi);
            writer.WriteLine("{0}.Clear();", writeMemeber);
            AddRange(writer, writeMemeber, "source." + pi.Name);
            return;
        }

        if (ptg == typeof(Tuple<,>))
        {
            var writeMemeber = GeneratorsHelper.GetWriteMemeberName(pi);
            var other        = string.Format("Tuple.Create(source.{0}.Item1, source.{0}.Item2)", writeMemeber);
            writer.WriteLine("{0} = source.{0} is null ? null : {0};", writeMemeber, other);
            return;
        }

        if (ptg == typeof(List<>))
        {
            var wm = GeneratorsHelper.GetWriteMemeberName(pi);
            if (pi.CanWrite)
            {
                writer.WriteLine("if (source.{0} is null)", wm);
                writer.WriteLine("\t{0} = null;", wm);
                writer.WriteLine("else");
                writer.WriteLine("{");
                writer.WriteLine("\t{0} = new System.Collections.Generic.List<{1}>();", wm,
                    pi.PropertyType
#if COREFX
                          .GetTypeInfo()
#endif
                        .GetGenericArguments()[0]);
                writer.Indent++;
                AddRange(writer, wm, "source." + pi.Name);
                writer.Indent--;
                writer.WriteLine("}");
            }
            else
            {
                writer.WriteLine("{0}.Clear();", wm);
                AddRange(writer, wm, "source." + pi.Name);
            }

            return;
        }

        {
            var el = TryGetRankOneArrayElement(ptg);
            if (el is not null)
            {
                var tn = resolver.GetTypeName(el).AsString(allowReferenceNullable);
                CopyArray(pi, tn, writer, resolver);
                return;
            }
        }

#if HAS_ICLONEABLE
        {
            var interf = pi.PropertyType.GetInterfaces();
            if (interf.Any(a => a == typeof(ICloneable)))
            {
                var castTypeName = resolver.GetTypeName(pi.PropertyType);
                if (pi.PropertyType.IsExplicityImplementation<ICloneable>(nameof(ICloneable.Clone)))
                    writer.WriteLine($"{pi.Name} = ({castTypeName.Declaration})((ICloneable)source.{pi.Name})?.Clone();");
                else
                    writer.WriteLine($"{pi.Name} = ({castTypeName.Declaration})source.{pi.Name}?.Clone();");

                return;
            }
        }
#endif
        if (pi.PropertyType == typeof(Dictionary<string, string>))
        {
            var wm = GeneratorsHelper.GetWriteMemeberName(pi);
            writer.WriteLine("if (source.{0} is null)", pi.Name);
            writer.WriteLine("    {0} = null;", wm);
            writer.WriteLine("else {");
            writer.WriteLine("    if ({0} is null)", wm);
            writer.WriteLine("        {0} = new System.Collections.Generic.Dictionary<string, string>();", wm);
            writer.WriteLine("    else");
            writer.WriteLine("        {0}.Clear();", wm);
            writer.WriteLine("    foreach(var tmp in source.{0}) {1}[tmp.Key] = tmp.Value;",
                pi.Name, wm);
            writer.WriteLine("}");
            return;
        }

        throw new NotSupportedException(string.Format(
            "CopyFromGenerator is unable to find a way how to copy value of property {0}.{1}", pi.DeclaringType,
            pi.Name));
        // writer.WriteLine("// {0} {1}", pi.Name, pi.PropertyType);
    }

    protected virtual PropertyInfo[] SortProperties(PropertyInfo[] properties)
    {
        var p = properties.OrderBy(a =>
        {
            var attribute = a.GetCustomAttribute<Auto.CopyFromOrderAttribute>();
            return attribute?.Order ?? 0;
        }).ToArray();
        return p;
    }

    private void AddRange(ICsCodeWriter writer, string target, string source)
    {
        var listExtension = Configuration?.ListExtension;
        if (listExtension is null)
            throw new NotImplementedException("Configuration.ListExtension is null");
        var m = listExtension
#if COREFX
                                  .GetTypeInfo()
#endif
            .GetMethod("AddRange", BindingFlags.Static | BindingFlags.Public);
        if (m is null)
            throw new Exception("Unable to find AddRange method");
        var isExtension = m.IsDefined(typeof(ExtensionAttribute), true);
        if (isExtension)
        {
            var t = m.DeclaringType;
            if (t is not null)
            {
                while (t.DeclaringType is not null)
                    t = t.DeclaringType;
                object? owner = Class.Owner;
                var nsCollection = owner as
                    INamespaceCollection;
                while (nsCollection is null && owner is not null)
                {
                    if (owner is CsClass csClass)
                        owner = csClass.Owner;
                    else
                        break;
                    nsCollection = owner as INamespaceCollection;
                }

                if (nsCollection is not null)
                {
                    nsCollection.AddImportNamespace(t.Namespace);
                    writer.WriteLine("{0}.AddRange({1});", target, source);
                    return;
                }
            }
        }

        var typeName = Class.GetTypeName(listExtension);
        writer.WriteLine("{0}.AddRange({1}, {2});", typeName, target, source);
    }

    private void CloneWithValuesProcessor(PropertyInfo pi, ICsCodeWriter writer, ITypeNameResolver resolver)
    {
        var wm = GeneratorsHelper.GetWriteMemeberName(pi);
#if HAS_ICLONEABLE

        var isCloneable = pi.PropertyType == typeof(ICloneable)
                          || pi.PropertyType.GetInterfaces().Any(a => a == typeof(ICloneable));
        if (isCloneable)
        {
            writer.WriteLine("if (source.{0} is not null)", pi.Name);
            writer.WriteLine("    {0} = ({1})(source.{2} as {3}).Clone();",
                wm,
                resolver.GetTypeName(pi.PropertyType),
                pi.Name,
                resolver.GetTypeName(typeof(ICloneable)));
            writer.WriteLine("else");
            writer.WriteLine("    {0} = null;", wm);
            return;
        }
#endif

        var cloneMethod = Configuration?.CustomCloneMethod;
        if (cloneMethod is null)
            throw new Exception("Unable to clone value of type " + pi.PropertyType);
        writer.WriteLine("{0} = {1}.{2}(source.{3}); // {4}",
            wm,
            resolver.GetTypeName(cloneMethod.DeclaringType),
            cloneMethod.Name,
            pi.Name,
            pi.PropertyType);
    }

    public CopyFromGeneratorConfiguration? Configuration { get; set; }

    private Auto.CopyFromAttribute _copyFromAttribute;
    private bool _doCloneable;
}

// ReSharper disable once ClassNeverInstantiated.Global
