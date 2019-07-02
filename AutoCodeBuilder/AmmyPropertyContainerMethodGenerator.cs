using System;
using System.Collections.Generic;
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
            context.AddNamespace("isukces.code");
            context.AddNamespace<KeyValuePair<string, string>>();
            context.AddNamespace(typeof(AmmyHelper));
            context.AddNamespace<NotNullAttribute>();

            var cl = context.GetOrCreateClass(type);
            {
                var cf = CreateCode(nameof(AmmyPropertyContainerMethodGenerator), "G2")
                    .WriteLine("var name = CodeUtils.GetMemberPath(func);")
                    .WriteLine("this.WithProperty(name, value);")
                    .WriteLine("return this;");

                var m = CreateMethod("WithProperty<TValue>", type, cl, cf);
                m.AddParam("func", "Expression<Func<TPropertyBrowser, TValue>>");
                m.AddParam("value", "object");
            }
            {
                var cf = CreateCode(nameof(AmmyPropertyContainerMethodGenerator), "G3")
                    .WriteLine("var name = CodeUtils.GetMemberPath(func);")
                    .WriteLine("this.WithProperty(name, value);")
                    .WriteLine("return this;");

                var m = CreateMethod("WithPropertyGeneric<TValue>", type, cl, cf);
                m.AddParam("func", "Expression<Func<TPropertyBrowser, TValue>>");
                m.AddParam("value", "TValue");
            }
          
            {
                // ======== WithPropertyNotNull
                var cf = CreateCode(nameof(AmmyPropertyContainerMethodGenerator), "G4")
                    .WriteLine("var name = CodeUtils.GetMemberPath(func);")
                    .WriteLine("return this.WithPropertyNotNull(name, value);");

                var m = CreateMethod("WithPropertyNotNull<TValue>", type, cl, cf);
                m.AddParam("func", "Expression<Func<TPropertyBrowser, TValue>>");
                m.AddParam("value", "object");
            }
            {
                // ======== WithPropertyGenericNotNull
                var cf = CreateCode(nameof(AmmyPropertyContainerMethodGenerator),"G5")
                    .WriteLine("var name = CodeUtils.GetMemberPath(func);")
                    .WriteLine("return this.WithPropertyNotNull(name, value);");

                var m = CreateMethod("WithPropertyGenericNotNull<TValue>", type, cl, cf);
                m.AddParam("func", "Expression<Func<TPropertyBrowser, TValue>>");
                m.AddParam("value", "TValue");
            }

            {
                // ======== WithPropertyAncestorBind
                var cf = CreateCode(nameof(AmmyPropertyContainerMethodGenerator),"G6")
                    .WriteLine("var bindToPath   = ExpressionTools.GetBindingPath(bindToPathExpression);")
                    .WriteLine("var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);")
                    .WriteLine("return this.WithPropertyAncestorBind(propertyName, bindToPath, typeof(TAncestor), bindingSettings);");

                var m = CreateMethod("WithPropertyAncestorBind<TAncestor>", type, cl, cf);
                m.AddParam("propertyNameExpression", "Expression<Func<TPropertyBrowser, object>>");
                m.AddParam("bindToPathExpression", "Expression<Func<TAncestor, object>>");
                var p = m.AddParam("bindingSettings", "Action<AmmyBind>");
                p.Attributes.Add(new CsAttribute("CanBeNull"));
                p.ConstValue = "null";
            }
            {
                // ======== WithPropertyAncestorBind
                var cf = CreateCode(nameof(AmmyPropertyContainerMethodGenerator),"G6 ver2")
                    .WriteLine("var bindToPath   = ExpressionTools.GetBindingPath(bindToPathExpression);")
                    .WriteLine("var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);")
                    .WriteLine("return this.WithPropertyAncestorBind(propertyName, bindToPath, typeof(TAncestor), mode, bindingSettings);");

                var m = CreateMethod("WithPropertyAncestorBind<TAncestor>", type, cl, cf);
                m.AddParam("propertyNameExpression", "Expression<Func<TPropertyBrowser, object>>");
                m.AddParam("bindToPathExpression", "Expression<Func<TAncestor, object>>");
                m.AddParam("mode", cl.GetTypeName<DataBindingMode>());
                var p = m.AddParam("bindingSettings", "Action<AmmyBind>");
                p.Attributes.Add(new CsAttribute("CanBeNull"));
                p.ConstValue = "null";
            }
            {
                var cf = CreateCode(nameof(AmmyPropertyContainerMethodGenerator),"G9")
                    .WriteLine("var bindToPath   = ExpressionTools.GetBindingPath(bindToPathExpression);")
                    .WriteLine("var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);")
                    .WriteLine("return this.WithPropertyAncestorBind(propertyName, bindToPath, typeof(TAncestor), bindingSettings);");

                var m = CreateMethod("WithPropertyAncestorBind<TAncestor, TValue>", type, cl, cf);
                m.AddParam("propertyNameExpression", "Expression<Func<TPropertyBrowser, TValue>>");
                m.AddParam("bindToPathExpression", "Expression<Func<TAncestor, TValue>>");
                var p = m.AddParam("bindingSettings", "Action<AmmyBind>");
                p.Attributes.Add(new CsAttribute("CanBeNull"));
                p.ConstValue = "null";
            }
            {
                var cf = CreateCode(nameof(AmmyPropertyContainerMethodGenerator),"G9 ver2")
                    .WriteLine("var bindToPath   = ExpressionTools.GetBindingPath(bindToPathExpression);")
                    .WriteLine("var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);")
                    .WriteLine("return this.WithPropertyAncestorBind(propertyName, bindToPath, typeof(TAncestor), mode, bindingSettings);");

                var m = CreateMethod("WithPropertyAncestorBind<TAncestor, TValue>", type, cl, cf);
                m.AddParam("propertyNameExpression", "Expression<Func<TPropertyBrowser, TValue>>");
                m.AddParam("bindToPathExpression", "Expression<Func<TAncestor, TValue>>");
                m.AddParam("mode", cl.GetTypeName<DataBindingMode>());
                var p = m.AddParam("bindingSettings", "Action<AmmyBind>");
                p.Attributes.Add(new CsAttribute("CanBeNull"));
                p.ConstValue = "null";
            }
            {
                // ======== WithPropertyStaticResource
                var cf = CreateCode(nameof(AmmyPropertyContainerMethodGenerator),"G7")
                    .WriteLine("return this.WithProperty(propertyNameExpression, new AmmyStaticResource(resourceName));");

                var m = CreateMethod("WithPropertyStaticResource", type, cl, cf);
                var p = m.AddParam("propertyNameExpression", "Expression<Func<TPropertyBrowser, object>>");
                p.Attributes.Add(new CsAttribute("NotNull"));
                p = m.AddParam("resourceName", "string");
                p.Attributes.Add(new CsAttribute("NotNull"));
            }
            {
                // ======== WithPropertyStaticResource ver 2
                var cf = CreateCode(nameof(AmmyPropertyContainerMethodGenerator),"G8")
                    .WriteLine("(this as IAmmyPropertyContainer).Properties[propertyName] = new AmmyStaticResource(resourceName);")
                    .WriteLine("return this;");

                var m = CreateMethod("WithPropertyStaticResource", type, cl, cf);
                var p = m.AddParam("propertyName", "string");
                p.Attributes.Add(new CsAttribute("NotNull"));
                p = m.AddParam("resourceName", "string");
                p.Attributes.Add(new CsAttribute("NotNull"));
            }

          
        }

        public AmmyPropertyContainerMethodGenerator WithSkip<T>()
        {
            Skip.Add(typeof(T));
            return this;
        }
    }
}