using System;
using System.Collections.Generic;
using iSukces.Code;
using iSukces.Code.Ammy;
using iSukces.Code.AutoCode;
using iSukces.Code.Compatibility.System.Windows.Data;
using iSukces.Code.Interfaces;
using iSukces.Code.Interfaces.Ammy;
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
            context.AddNamespace("iSukces.Code.Interfaces.Ammy");
            context.AddNamespace("iSukces.Code.Ammy");
            context.AddNamespace("iSukces.Code");
            context.AddNamespace<KeyValuePair<string, string>>();
            context.AddNamespace(typeof(AmmyHelper));
            context.AddNamespace<NotNullAttribute>();

            var cl = context.GetOrCreateClass(type);
            {
                var cf = CreateCodeWriter()
                    .WriteLine("var name = CodeUtils.GetMemberPath(func);")
                    .WriteLine("this.WithProperty(name, value);")
                    .WriteLine("return this;");

                var m = CreateMethod("WithProperty<TValue>", type, cl, cf);
                m.AddParam("func", "Expression<Func<TPropertyBrowser, TValue>>");
                m.AddParam("value", "object");
            }
            {
                var cf = CreateCodeWriter()
                    .WriteLine("var name = CodeUtils.GetMemberPath(func);")
                    .WriteLine("this.WithProperty(name, value);")
                    .WriteLine("return this;");

                var m = CreateMethod("WithPropertyGeneric<TValue>", type, cl, cf);
                m.AddParam("func", "Expression<Func<TPropertyBrowser, TValue>>");
                m.AddParam("value", "TValue");
            }
          
            {
                // ======== WithPropertyNotNull
                var cf = CreateCodeWriter()
                    .WriteLine("var name = CodeUtils.GetMemberPath(func);")
                    .WriteLine("return this.WithPropertyNotNull(name, value);");

                var m = CreateMethod("WithPropertyNotNull<TValue>", type, cl, cf);
                m.AddParam("func", "Expression<Func<TPropertyBrowser, TValue>>");
                m.AddParam("value", "object");
            }
            {
                // ======== WithPropertyGenericNotNull
                var cf = CreateCodeWriter()
                    .WriteLine("var name = CodeUtils.GetMemberPath(func);")
                    .WriteLine("return this.WithPropertyNotNull(name, value);");

                var m = CreateMethod("WithPropertyGenericNotNull<TValue>", type, cl, cf);
                m.AddParam("func", "Expression<Func<TPropertyBrowser, TValue>>");
                m.AddParam("value", "TValue");
            }

            {
                // ======== WithPropertyAncestorBind
                var cf = CreateCodeWriter()
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
                var cf = CreateCodeWriter()
                    .WriteLine("var bindToPath   = ExpressionTools.GetBindingPath(bindToPathExpression);")
                    .WriteLine("var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);")
                    .WriteLine("return this.WithPropertyAncestorBind(propertyName, bindToPath, typeof(TAncestor), mode, bindingSettings);");

                var m = CreateMethod("WithPropertyAncestorBind<TAncestor>", type, cl, cf);
                m.AddParam("propertyNameExpression", "Expression<Func<TPropertyBrowser, object>>");
                m.AddParam("bindToPathExpression", "Expression<Func<TAncestor, object>>");
                m.AddParam("mode", cl.GetTypeName<XBindingMode>());
                var p = m.AddParam("bindingSettings", "Action<AmmyBind>");
                p.Attributes.Add(new CsAttribute("CanBeNull"));
                p.ConstValue = "null";
            }
            {
                var cf = CreateCodeWriter()
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
                var cf = CreateCodeWriter()
                    .WriteLine("var bindToPath   = ExpressionTools.GetBindingPath(bindToPathExpression);")
                    .WriteLine("var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);")
                    .WriteLine("return this.WithPropertyAncestorBind(propertyName, bindToPath, typeof(TAncestor), mode, bindingSettings);");

                var m = CreateMethod("WithPropertyAncestorBind<TAncestor, TValue>", type, cl, cf);
                m.AddParam("propertyNameExpression", "Expression<Func<TPropertyBrowser, TValue>>");
                m.AddParam("bindToPathExpression", "Expression<Func<TAncestor, TValue>>");
                m.AddParam("mode", cl.GetTypeName<XBindingMode>());
                var p = m.AddParam("bindingSettings", "Action<AmmyBind>");
                p.Attributes.Add(new CsAttribute("CanBeNull"));
                p.ConstValue = "null";
            }
            // SELF
            {
                var cf = CreateCodeWriter()
                    
                    .WriteLine("var b          = new AmmyBind(bindToPath).WithBindFromSelf();")
                    .WriteLine("bindingSettings?.Invoke(b);")
                    .WriteLine("return this.WithProperty(propertyName, b);");                

                var m = CreateMethod("WithPropertySelfBind", type, cl, cf);
                m.AddParam("propertyName", "string");
                m.AddParam("bindToPath", "string");
                var p = m.AddParam("bindingSettings", "Action<AmmyBind>");
                p.Attributes.Add(new CsAttribute("CanBeNull"));
                p.ConstValue = "null";
            }
            {
                var cf = CreateCodeWriter()
                    .WriteLine("var bindToPath = ExpressionTools.GetBindingPath(bindToPathExpression);")
                    .WriteLine("return WithPropertySelfBind(propertyName, bindToPath, bindingSettings);");

                var m = CreateMethod("WithPropertySelfBind<TSelf>", type, cl, cf);
                m.AddParam("propertyName", "string");
                m.AddParam("bindToPathExpression", "Expression<Func<TSelf, object>>");
                var p = m.AddParam("bindingSettings", "Action<AmmyBind>");
                p.Attributes.Add(new CsAttribute("CanBeNull"));
                p.ConstValue = "null";
            }

            {
                var cf = CreateCodeWriter()
                    .WriteLine("var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);")
                    .WriteLine("var bindToPath = ExpressionTools.GetBindingPath(bindToPathExpression);")
                    .WriteLine("return WithPropertySelfBind(propertyName, bindToPath, bindingSettings);");

                var m = CreateMethod("WithPropertySelfBind<TSelf>", type, cl, cf);
                m.AddParam("propertyNameExpression", "Expression<Func<TPropertyBrowser, object>>");
                m.AddParam("bindToPathExpression", "Expression<Func<TSelf, object>>");
                var p = m.AddParam("bindingSettings", "Action<AmmyBind>");
                p.Attributes.Add(new CsAttribute("CanBeNull"));
                p.ConstValue = "null";
            }

            {
                // ======== WithPropertyStaticResource
                var cf = CreateCodeWriter()
                    .WriteLine("return this.WithProperty(propertyNameExpression, new AmmyStaticResource(resourceName));");

                var m = CreateMethod("WithPropertyStaticResource", type, cl, cf);
                var p = m.AddParam("propertyNameExpression", "Expression<Func<TPropertyBrowser, object>>");
                p.Attributes.Add(new CsAttribute("NotNull"));
                p = m.AddParam("resourceName", "string");
                p.Attributes.Add(new CsAttribute("NotNull"));
            }
            {
                // ======== WithPropertyStaticResource ver 2
                var cf = CreateCodeWriter()
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