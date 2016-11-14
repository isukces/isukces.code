﻿#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using isukces.code.interfaces;

#endregion

namespace isukces.code.AutoCode
{
    internal class GeneratorsHelper
    {
        #region Static Methods

        public static HashSet<T> MakeCopy<T>(IEnumerable<T> source, IEnumerable<T> append = null,
            IEnumerable<T> remove = null)
        {
            var s = new HashSet<T>();
            if (source != null)
                foreach (var i in source)
                    s.Add(i);
            if (append != null)
                foreach (var i in append)
                    s.Add(i);
            if (remove != null)
                foreach (var i in remove)
                    s.Remove(i);
            return s;
        }

        public static string TypeName(Type type, INamespaceContainer container)
        {
            //todo: Generic types
            if (type == null)
                return null;

            var simple = SimpleTypeName(type);
            if (!String.IsNullOrEmpty(simple))
                return simple;
            if (type.DeclaringType != null)
                return TypeName(type.DeclaringType, container) + "." + type.Name;

            string fullName;
            {
                if (type.IsGenericType)
                {
                    var gt = type.GetGenericTypeDefinition();
                    var args = type.GetGenericArguments();
                    var args2 = String.Join(",", args.Select(a => TypeName(a, container)));
                    fullName = gt.FullName.Split('`')[0] + "<" + args2 + ">";
                }
                else
                    fullName = type.FullName;
            }
            var typeNamespace = type.Namespace;
            return !String.IsNullOrEmpty(typeNamespace) &&
                   (container?.GetNamespaces(true)?.Contains(typeNamespace) ?? false)
                ? fullName.Substring(typeNamespace.Length + 1)
                : fullName;
        }

        private static string SimpleTypeName(Type t)
        {
            if (t == typeof(string)) return "string";
            if (t == typeof(int)) return "int";
            if (t == typeof(uint)) return "uint";
            if (t == typeof(double)) return "double";
            if (t == typeof(short)) return "short";
            if (t == typeof(ushort)) return "ushort";
            if (t == typeof(object)) return "object";
            if (t == typeof(bool)) return "bool";
            Type[] args = null;
            if (t.IsGenericType)
            {
                var gt = t.GetGenericTypeDefinition();
                args = t.GetGenericArguments();
                if (gt == typeof(Nullable<>))
                    return SimpleTypeName(args[0]) + "?";
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

        #endregion

        public static string GetWriteMemeberName(PropertyInfo pi)
        {
            var props = pi.GetCustomAttribute<Auto.WriteMemberAttribute>();
            return !String.IsNullOrEmpty(props?.Name) ? props.Name : pi.Name;
        }

        public const BindingFlags All =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        public static string FieldName(string x)
        {
            return "_" + x.Substring(0, 1).ToLower() + x.Substring(1);
        }

        public static bool IsMemberStatic(MemberInfo mi)
        {
            //todo: move to another type
            var methodInfo = mi as MethodInfo;
            if (methodInfo != null)
                return methodInfo.IsStatic;
            var propertyInfo = mi as PropertyInfo;
            if (propertyInfo != null)
            {
                if (propertyInfo.CanRead)
                {
                    var tmp = propertyInfo.GetGetMethod();
                    if (tmp == null)
                        tmp = propertyInfo.GetSetMethod();
                    if (tmp == null)
                    {
                        var pi = mi.GetType()
                            .GetField("m_bindingFlags", BindingFlags.Instance | BindingFlags.NonPublic);
                        if (pi != null)
                        {
                            var v = (BindingFlags)pi.GetValue(mi);
                            return v.HasFlag(BindingFlags.Static);
                        }
                    }
                    if (tmp == null)
                        throw new NotSupportedException();
                    return IsMemberStatic(tmp);
                }
            }
            throw new NotSupportedException();
        }
    }
}