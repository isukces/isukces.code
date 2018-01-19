using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using isukces.code.CodeWrite;
using isukces.code.interfaces;

namespace isukces.code.AutoCode
{
    internal partial class Generators
    {
        public class CopyFromGenerator : SingleClassGenerator, IAutoCodeGenerator
        {
            // private readonly AutoCodeGeneratorConfiguration _configuration;


            private void CloneWithValuesProcessor(PropertyInfo pi, ICodeWriter writer, ITypeNameResolver resolver)
            {
                var wm = GeneratorsHelper.GetWriteMemeberName(pi);
#if !COREFX

                var isCloneable = (pi.PropertyType == typeof(ICloneable))
                                  || pi.PropertyType.GetInterfaces().Any(a => a == typeof(ICloneable));
                if (isCloneable)
                {
                    writer.WriteLine("if (source.{0} != null)", pi.Name);
                    writer.WriteLine("    {0} = ({1})(source.{2} as {3}).Clone();",
                        wm,
                        resolver.TypeName(pi.PropertyType),
                        pi.Name,
                        resolver.TypeName(typeof(ICloneable)));
                    writer.WriteLine("else");
                    writer.WriteLine("    {0} = null;", wm);
                    return;
                }
#endif

                if (_configuration.CustomCloneMethod == null)
                    throw new Exception("Unable to clone value of type " + pi.PropertyType);
                writer.WriteLine("{0} = {1}.{2}(source.{3}); // {4}",
                    wm,
                    resolver.TypeName(_configuration.CustomCloneMethod.DeclaringType),
                    _configuration.CustomCloneMethod.Name,
                    pi.Name,
                    pi.PropertyType);
            }

            private static void CopyArray(PropertyInfo pi, string type, ICodeWriter writer)
            {
                var wm = GeneratorsHelper.GetWriteMemeberName(pi);
                writer.WriteLine("if (source.{0} == null)", pi.Name);
                writer.WriteLine("    {0} = null;", wm);
                writer.WriteLine("else {");
                if (type == "System.Windows.Point")
                {
                    writer.WriteLine("#if COREFX");
                    writer.WriteLine($"    {wm} = new Compat.{type}[source.{pi.Name}.Length];");
                    writer.WriteLine("#else");
                    writer.WriteLine($"    {wm} = new {type}[source.{pi.Name}.Length];");
                    writer.WriteLine("#endif");
                }
                else
                {
                    writer.WriteLine($"    {wm} = new {type}[source.{pi.Name}.Length];");
                }
                writer.WriteLine(
                    "    for (var index = 0; index < source.{0}.Length; index++) {1}[index] = source.{0}[index];",
                    pi.Name, wm);
                writer.WriteLine("}");
            }


            private static void GenerateMethodClone(Type type, CsClass csClass)
            {
                csClass.ImplementedInterfaces.Add("ICloneable");
                var cm = csClass.AddMethod("Clone", "object", "Makes clone of object");
                ICodeWriter writer = new CodeWriter();
                writer.WriteLine("var a = new {0}();", type);
                writer.WriteLine("a.CopyFrom(this);");
                writer.WriteLine("return a;");
                cm.Body = writer.Code;
            }

            private void GenerateInternal()
            {
                if (!_doCloneable && (_copyFromAttribute == null))
                    return;
                {
                    var cm = Class.AddMethod("CopyFrom", "void", null);
                    cm.AddParam("source", Class.TypeName(Type)); // reduce type
                    ICodeWriter writer = new CodeWriter();
                    writer.WriteLine("if (ReferenceEquals(source, null))");
                    writer.WriteLine("    throw new ArgumentNullException(nameof(source));");
                    var properties = Class.DotNetType
#if COREFX
                        .GetTypeInfo()
#endif
                        .GetProperties();
                    foreach (var i in properties)
                        ProcessProperty(i, _copyFromAttribute, writer);
                    cm.Body = writer.Code;
                }
                if (_doCloneable)
                    GenerateMethodClone(Type, Class);
            }


            private void ProcessProperty(PropertyInfo pi, Auto.CopyFromAttribute attr, ICodeWriter writer)
            {
                ITypeNameResolver resolver = Class;
                if (pi.PropertyType
#if COREFX
                        .GetTypeInfo()
#endif
                        .IsValueType || (pi.PropertyType == typeof(string)))
                {
                    if (!pi.CanWrite || !pi.CanRead)
                        return;

                    writer.WriteLine("{0} = source.{0}; // {1}", pi.Name, resolver.TypeName(pi.PropertyType));
                    return;
                }
                if (pi.PropertyType.IsArray)
                {
                    var eltype = pi.PropertyType.GetElementType();
                    CopyArray(pi, eltype.FullName, writer);
                    return;
                }
                {
                    var tmp = pi.GetCustomAttribute<Auto.CopyBy.CloneableAttribute>();
                    if (tmp != null)
                    {
                        writer.WriteLine("{0} = ({1})((ICloneable)source.{0})?.Clone(); // BY Icloneable {1}", pi.Name,
                            pi.PropertyType);
                        return;
                    }
                }
                {
                    if ((attr != null) && attr.HasSkip(pi.Name))
                        return;
                    var tmp = pi.GetCustomAttribute<Auto.CopyBy.ReferenceAttribute>();
                    if ((tmp != null) || ((attr != null) && attr.HasCopyByReference(pi.Name)))
                    {
                        writer.WriteLine("{0} = source.{0}; // BY REF {1}", pi.Name, pi.PropertyType);
                        return;
                    }
                }
                {
                    var tmp = pi.GetCustomAttribute<Auto.CopyBy.ValuesProcessorAttribute>();
                    if (tmp != null)
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
                    var other = string.Format("Tuple.Create(source.{0}.Item1, source.{0}.Item2)", writeMemeber);
                    writer.WriteLine("{0} = source.{0} == null ? null : {0};", writeMemeber, other);
                    return;
                }
                if (ptg == typeof(List<>))
                {
                    var wm = GeneratorsHelper.GetWriteMemeberName(pi);
                    if (pi.CanWrite)
                    {
                        writer.WriteLine("if (source.{0} == null)", wm);
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
                if (ptg == typeof(double[]))
                {
                    CopyArray(pi, "double", writer);
                    return;
                }
#if !COREFX
                if (ptg == typeof(Point[]))
                {
                    CopyArray(pi, "System.Windows.Point", writer); // todo: external copy
                    return;
                }
                {
                    var interf = pi.PropertyType.GetInterfaces();
                    if (interf.Any(a => a == typeof(ICloneable)))
                    {
                        writer.WriteLine("{0} = ({1})((ICloneable)source.{0})?.Clone();", pi.Name, pi.PropertyType);
                        return;
                    }
                }
#endif
                if (pi.PropertyType == typeof(Dictionary<string, string>))
                {
                    var wm = GeneratorsHelper.GetWriteMemeberName(pi);
                    writer.WriteLine("if (source.{0} == null)", pi.Name);
                    writer.WriteLine("    {0} = null;", wm);
                    writer.WriteLine("else {");
                    writer.WriteLine("    if ({0} == null)", wm);
                    writer.WriteLine("        {0} = new System.Collections.Generic.Dictionary<string, string>();", wm);
                    writer.WriteLine("    else");
                    writer.WriteLine("        {0}.Clear();", wm);
                    writer.WriteLine("    foreach(var tmp in source.{0}) {1}[tmp.Key] = tmp.Value;",
                        pi.Name, wm);
                    writer.WriteLine("}");
                    return;
                }
                throw new NotSupportedException(string.Format("{0}.{1}", pi.DeclaringType, pi.Name));
                // writer.WriteLine("// {0} {1}", pi.Name, pi.PropertyType);
            }

            private void AddRange(ICodeWriter writer, string target, string source)
            {
                if (_configuration.ListExtension == null)
                    throw new NotImplementedException("AddRange");
                var m = _configuration.ListExtension
#if COREFX
                    .GetTypeInfo()
#endif
                    .GetMethod("AddRange", BindingFlags.Static | BindingFlags.Public);
                if (m == null)
                    throw new Exception("Unable to find AddRange method");
                var isExtension = m.IsDefined(typeof(ExtensionAttribute), true);
                if (isExtension)
                {
                    var t = m.DeclaringType;
                    if (t != null)
                    {
                        while (t.DeclaringType != null)
                            t = t.DeclaringType;
                        object o = Class.Owner;
                        var nsCollection = o as INamespaceCollection;
                        while (nsCollection == null && o != null)
                        {
                            if (o is CsClass)
                                o = ((CsClass)o).Owner;
                            else if (o is CsNamespace) // can probably be removed
                                o = ((CsNamespace)o).Owner;
                            else
                                break;
                            nsCollection = o as INamespaceCollection;
                        }
                        if (nsCollection != null)
                        {
                            nsCollection.AddImportNamespace(t.Namespace);
                            writer.WriteLine("{0}.AddRange({1});", target, source);
                            return;
                        }

                    }
                }
                var typeName = Class.TypeName(_configuration.ListExtension);
                writer.WriteLine("{0}.AddRange({1}, {2});", typeName, target, source);
            }

            private Auto.CopyFromAttribute _copyFromAttribute;
            private bool _doCloneable;
            private CopyFromGeneratorConfiguration _configuration;

            public void Generate(Type type, IAutoCodeGeneratorContext context)
            {
                Setup(type, context);

                _copyFromAttribute = type
#if COREFX
                    .GetTypeInfo()
#endif
                    .GetCustomAttribute<Auto.CopyFromAttribute>();
                _doCloneable = type
#if COREFX
                                   .GetTypeInfo()
#endif                                   
                                   .GetCustomAttribute<Auto.Cloneable>(false) != null;
                _configuration = context.ResolveConfig<CopyFromGeneratorConfiguration>();
                GenerateInternal();
            }
        }
    }
}