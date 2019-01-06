using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using isukces.code;
using isukces.code.Ammy;
using isukces.code.AutoCode;
using isukces.code.interfaces;
using isukces.code.interfaces.Ammy;
using JetBrains.Annotations;

namespace AutoCodeBuilder
{
    internal class AmmyPropertyContainerMethodGenerator : IAutoCodeGenerator
    {
        private static bool CheckIfAnonymousType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            // HACK: The only way to detect anonymous types right now.
            return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
                   && type.IsGenericType && type.Name.Contains("AnonymousType")
                   && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
                   && (type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic;
        }

        private static CsMethod CreateMethod(string name, Type type, CsClass cl, CodeWriter cf)
        {           
            var m = cl.AddMethod(name, cl.TypeName(type))
                // .WithStatic()
                .WithBody(cf);
            m.WithAttribute(cl, typeof(AutocodeGeneratedAttribute));
            return m;
        }


        public void Generate(Type type, IAutoCodeGeneratorContext context)
        {
            if (Skip.Contains(type))
                return;
            if (type.IsInterface)
                return;
            if (type.IsAbstract)
                return;
            if (CheckIfAnonymousType(type))
                return;

            if (type.GetTypeInfo().IsGenericType)
            {
                if (!typeof(IAmmyPropertyContainer).IsAssignableFrom(type)) return;
            }
            else
            {
                if (!typeof(IAmmyPropertyContainer).IsAssignableFrom(type)) return;
            }

            context.AddNamespace("System.Linq.Expressions");
            context.AddNamespace("System");
            context.AddNamespace("isukces.code.interfaces.Ammy");
            context.AddNamespace("isukces.code.Ammy");
            context.AddNamespace<KeyValuePair<string, string>>();
            context.AddNamespace(typeof(AmmyHelper));
            context.AddNamespace<NotNullAttribute>();
            
            
            var cl = context.GetOrCreateClass(type);
            {
                var cf = new CsCodeWriter();
                cf.WriteLine("var mi = AmmyHelper.GetMemberInfo(func);");
                cf.WriteLine("this.WithProperty(mi.Member.Name, value);");
                cf.WriteLine("return this;");

                var m = CreateMethod("WithProperty<TValue>", type, cl, cf);
                m.AddParam("func","Expression<Func<TPropertyBrowser, TValue>>");
                m.AddParam("value", "object");
            }
            {
                var cf = new CsCodeWriter();
                cf.WriteLine("var mi = AmmyHelper.GetMemberInfo(func);");
                cf.WriteLine("this.WithProperty(mi.Member.Name, value);");
                cf.WriteLine("return this;");

                var m = CreateMethod("WithPropertyGeneric<TValue>", type, cl, cf);
                m.AddParam("func","Expression<Func<TPropertyBrowser, TValue>>");
                m.AddParam("value", "TValue");
            }
            {
                // ======== WithPropertyNotNull
                var cf = new CsCodeWriter();
                cf.WriteLine("var mi = AmmyHelper.GetMemberInfo(func);");
                cf.WriteLine("return this.WithPropertyNotNull(mi.Member.Name, value);" +
                             "" +
                             "");

                var m = CreateMethod("WithPropertyNotNull<TValue>", type, cl, cf);
                m.AddParam("func","Expression<Func<TPropertyBrowser, TValue>>");
                m.AddParam("value", "object");
            }
            {
                // ======== WithPropertyGenericNotNull
                var cf = new CsCodeWriter();
                cf.WriteLine("var mi = AmmyHelper.GetMemberInfo(func);");
                cf.WriteLine("return this.WithPropertyNotNull(mi.Member.Name, value);");

                var m = CreateMethod("WithPropertyGenericNotNull<TValue>", type, cl, cf);
                m.AddParam("func","Expression<Func<TPropertyBrowser, TValue>>");
                m.AddParam("value", "TValue");
            }
                        
            {
                // ======== WithPropertyAncestorBind
                var cf = new CsCodeWriter();
                cf.WriteLine("var bindToPath   = ExpressionTools.GetBindingPath(bindToPathExpression);");
                cf.WriteLine("var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);");
                cf.WriteLine(
                    "return this.WithPropertyAncestorBind(propertyName, bindToPath, typeof(TAncestor), bindingSettings);");

                var m = CreateMethod("WithPropertyAncestorBind<TAncestor>", type, cl, cf);
                m.AddParam("propertyNameExpression","Expression<Func<TPropertyBrowser, object>>");
                m.AddParam("bindToPathExpression","Expression<Func<TPropertyBrowser, object>>");
                var p =m.AddParam("bindingSettings", "KeyValuePair<string, string>[]");
                p.Attributes.Add(new CsAttribute("CanBeNull"));
                p.ConstValue = "null";
            }
        }

        public AmmyPropertyContainerMethodGenerator WithSkip<T>()
        {
            Skip.Add(typeof(T));
            return this;
        }

        public HashSet<Type> Skip { get; } = new HashSet<Type>();
    }
}