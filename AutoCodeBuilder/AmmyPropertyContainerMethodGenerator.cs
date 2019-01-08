using System;
using System.Collections.Generic;
using System.Reflection;
using isukces.code;
using isukces.code.Ammy;
using isukces.code.AutoCode;
using isukces.code.interfaces;
using isukces.code.interfaces.Ammy;
using JetBrains.Annotations;

namespace AutoCodeBuilder
{
    internal class AmmyPropertyContainerMethodGenerator : BaseGenerator, IAutoCodeGenerator
    {
        public void Generate(Type type, IAutoCodeGeneratorContext context)
        {
            if (IgnoreType(type)) return;
            if (NotImplements<IAmmyPropertyContainer>(type)) return;

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
                m.AddParam("func", "Expression<Func<TPropertyBrowser, TValue>>");
                m.AddParam("value", "object");
            }
            {
                var cf = new CsCodeWriter();
                cf.WriteLine("var mi = AmmyHelper.GetMemberInfo(func);");
                cf.WriteLine("this.WithProperty(mi.Member.Name, value);");
                cf.WriteLine("return this;");

                var m = CreateMethod("WithPropertyGeneric<TValue>", type, cl, cf);
                m.AddParam("func", "Expression<Func<TPropertyBrowser, TValue>>");
                m.AddParam("value", "TValue");
            }
            {
                // ======== WithPropertyNotNull
                var cf = new CsCodeWriter();
                cf.WriteLine("var mi = AmmyHelper.GetMemberInfo(func);");
                cf.WriteLine("return this.WithPropertyNotNull(mi.Member.Name, value);" );

                var m = CreateMethod("WithPropertyNotNull<TValue>", type, cl, cf);
                m.AddParam("func", "Expression<Func<TPropertyBrowser, TValue>>");
                m.AddParam("value", "object");
            }
            {
                // ======== WithPropertyGenericNotNull
                var cf = new CsCodeWriter();
                cf.WriteLine("var mi = AmmyHelper.GetMemberInfo(func);");
                cf.WriteLine("return this.WithPropertyNotNull(mi.Member.Name, value);");

                var m = CreateMethod("WithPropertyGenericNotNull<TValue>", type, cl, cf);
                m.AddParam("func", "Expression<Func<TPropertyBrowser, TValue>>");
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
                m.AddParam("propertyNameExpression", "Expression<Func<TPropertyBrowser, object>>");
                m.AddParam("bindToPathExpression", "Expression<Func<TPropertyBrowser, object>>");
                var p = m.AddParam("bindingSettings", "KeyValuePair<string, string>[]");
                p.Attributes.Add(new CsAttribute("CanBeNull"));
                p.ConstValue = "null";
            }
        }

        public AmmyPropertyContainerMethodGenerator WithSkip<T>()
        {
            Skip.Add(typeof(T));
            return this;
        }
    }
}