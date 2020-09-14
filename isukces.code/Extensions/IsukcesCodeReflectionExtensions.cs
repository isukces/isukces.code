using System;
using System.Reflection;

namespace iSukces.Code
{
    public static class IsukcesCodeReflectionExtensions
    {
        [Obsolete("Use ReflectionTypeWrapper.UnwrapNullable(true)")]
        public static Type GetNullableType(this Type type)
        {
            return new ReflectionTypeWrapper(type).UnwrapNullable(true);
        }

        public static bool IsMemberStatic(this MemberInfo mi)
        {
            //todo: move to another type
            if (mi is MethodInfo methodInfo)
                return methodInfo.IsStatic;
            if (mi is PropertyInfo propertyInfo)
            {
                if (propertyInfo.CanRead)
                {
                    var tmp = propertyInfo.GetGetMethod();
                    if (tmp == null)
                        tmp = propertyInfo.GetSetMethod();
                    if (tmp == null)
                    {
                        var pi = mi.GetType()
#if COREFX
                            .GetTypeInfo()
#endif
                            .GetField("m_bindingFlags", BindingFlags.Instance | BindingFlags.NonPublic);
                        if (pi != null)
                        {
                            var v = (BindingFlags)pi.GetValue(mi);
                            return v.HasFlag(BindingFlags.Static);
                        }
                    }

                    if (tmp == null)
                        throw new NotSupportedException();
                    // ReSharper disable once TailRecursiveCall
                    return IsMemberStatic(tmp);
                }
            }

            throw new NotSupportedException();
        }

        public static string SimpleTypeName(this Type t)
        {
            if (t == typeof(string)) return "string";
            if (t == typeof(int)) return "int";
            if (t == typeof(uint)) return "uint";
            if (t == typeof(double)) return "double";
            if (t == typeof(float)) return "float";
            if (t == typeof(short)) return "short";
            if (t == typeof(ushort)) return "ushort";
            if (t == typeof(long)) return "long";
            if (t == typeof(ulong)) return "ulong";
            if (t == typeof(object)) return "object";
            if (t == typeof(bool)) return "bool";
            if (t == typeof(decimal)) return "decimal";
            if (t == typeof(byte)) return "byte";
            if (t == typeof(sbyte)) return "sbyte";

            {
                // nullable support 
                var nullableType = new ReflectionTypeWrapper(t).UnwrapNullable(true);
                if (nullableType != null)
                {
                    var simple = SimpleTypeName(nullableType);
                    if (string.IsNullOrEmpty(simple))
                        return null;
                    return simple + "?";
                }
            }

            return null;

            // var ns = t.Namespace;
            // var fullName = t.FullName.Split('`')[0];

            // System.Nullable`1[[System.Int16, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]

            /*
            foreach (var i in CopyMeMaker.GetNameSpaces().OrderByDescending(a => a.Length))
            {
                if (ns != i) continue;
                var c = fullName.Substring(ns.Length + 1);
                if (!c.Contains("."))
                    fullName = c;
                else
                    fullName = c;
                break;
            }
            if (args != null)
            {
                fullName += "<" + String.Join(",", args.Select(TypeName)) + ">";
            }
            if (fullName.Contains("+"))
                fullName = fullName.Replace("+", ".");
            return fullName;
            */
        }
    }
}