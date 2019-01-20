using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using isukces.code;
using isukces.code.interfaces;

namespace AutoCodeBuilder
{
    internal class BaseGenerator
    {
        protected static bool CheckIfAnonymousType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            // HACK: The only way to detect anonymous types right now.
            return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
                   && type.IsGenericType && type.Name.Contains("AnonymousType")
                   && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
                   && (type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic;
        }

        protected static CsCodeWriter CreateCode(string generator, string extraInfo)
        {
            var code = new CsCodeWriter();
            code.WriteLine("// generator : " + (generator + " " + extraInfo).Trim());
            return code;
        }

        protected static CsMethod CreateMethod(string name, Type type, CsClass cl, CodeWriter cf)
        {
            var m = cl.AddMethod(name, cl.TypeName(type))
                // .WithStatic()
                .WithBody(cf);
            m.WithAttribute(cl, typeof(AutocodeGeneratedAttribute));
            return m;
        }

       

        protected static bool NotImplements<TInterface>(Type type)
        {
            if (type.GetTypeInfo().IsGenericType)
            {
                if (!typeof(TInterface).IsAssignableFrom(type)) return true;
            }
            else
            {
                if (!typeof(TInterface).IsAssignableFrom(type)) return true;
            }

            return false;
        }

        protected bool IgnoreType(Type type)
        {
            if (Skip.Contains(type))
                return true;
            if (type.IsInterface)
                return true;
            if (type.IsAbstract)
                return true;
            if (CheckIfAnonymousType(type))
                return true;
            return false;
        }

        public HashSet<Type> Skip { get; } = new HashSet<Type>();
    }
}