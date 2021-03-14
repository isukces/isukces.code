using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using iSukces.Code.Ammy;
using iSukces.Code.Compatibility.System.Windows.Data;
using JetBrains.Annotations;

namespace iSukces.Code.Interfaces.Ammy
{
    /// <summary>
    ///     Wrapping clas for make fluent generics methods easier
    /// </summary>
    /// <typeparam name="TBuilder"></typeparam>
    /// <typeparam name="TPropertyBrowser"></typeparam>
    public sealed partial class AmmyBuilderExtender<TBuilder, TPropertyBrowser> : IAmmyPropertyContainer,
        IAmmyCodePieceConvertible
        where TBuilder : IAmmyCodePieceConvertible, IAmmyPropertyContainer<TPropertyBrowser>, IAmmyContentItemsContainer
    {
        public AmmyBuilderExtender(TBuilder builder) => Builder = builder;

        public Browser<T> Browse<T>() => new Browser<T>(this);

        public IAmmyCodePiece ToAmmyCode(IConversionCtx ctx) => Builder.ToAmmyCode(ctx);

        public DataContextBinder<TDataContext> WithDataContext<TDataContext>(AmmyBind bind)
        {
            Builder.WithProperty(DataContextPropertyName, bind);
            return new DataContextBinder<TDataContext>(this);
        }

        public DataContextBinder<TDataContext> WithDataContextFromAncestor<TAncestor, TDataContext>(
            Expression<Func<TAncestor, TDataContext>> propertyNameExpression,
            XBindingMode mode = XBindingMode.OneWay)
        {
            var bind = AmmyBind.FromAncestor<TAncestor>(ExpressionTools.GetBindingPath(propertyNameExpression))
                .WithMode(mode);
            return WithDataContext<TDataContext>(bind);
        }

        public DataContextBinder<TDataContext> WithDataContextFromAncestor<TDataContext>(
            XBindingMode mode = XBindingMode.OneWay)
        {
            return WithDataContextFromAncestor<TDataContext, TDataContext>(a => a, mode);
        }


        public DataContextBinder<TDataContext>
            WithDataContextFromFromElementName<TOwner, TElement, TDataContext>(
                Expression<Func<TOwner, TElement>> elementNameExpression,
                Expression<Func<TElement, TDataContext>> pathExpression,
                XBindingMode mode = XBindingMode.OneWay)
        {
            var elementName = ExpressionTools.GetBindingPath(elementNameExpression);
            var path        = ExpressionTools.GetBindingPath(pathExpression);
            var bind = new AmmyBindBuilder(path)
            {
                From = new ElementNameBindingSource(elementName),
                Mode = mode
            }.Build();
            Builder.WithProperty(DataContextPropertyName, bind);
            return new DataContextBinder<TDataContext>(this);
        }

        public DataContextBinder<TDataContext>
            WithDataContextFromFromElementName<TElement, TDataContext>(string elementName,
                Expression<Func<TElement, TDataContext>> pathExpression, XBindingMode mode = XBindingMode.OneWay)
        {
            var path = ExpressionTools.GetBindingPath(pathExpression);
            var bind = new AmmyBindBuilder(path)
            {
                From = new ElementNameBindingSource(elementName),
                Mode = mode
            }.Build();
            Builder.WithProperty(DataContextPropertyName, bind);
            return new DataContextBinder<TDataContext>(this);
        }


        public DataContextBinder<TDataContext> WithDataContextSilent<TDataContext>() =>
            new DataContextBinder<TDataContext>(this);


        public AmmyBuilderExtender<TBuilder, TPropertyBrowser> WithPropertyFromElementName<TOwner, TElement>(
            Expression<Func<TPropertyBrowser, object>> propertyExpression,
            Expression<Func<TOwner, TElement>> elementNameExpression,
            Expression<Func<TElement, object>> pathExpression,
            XBindingMode mode, [CanBeNull] Action<AmmyBind> bindingSettings = null)
        {
            var elementName = ExpressionTools.GetBindingPath(elementNameExpression);
            return WithPropertyFromElementName2(propertyExpression, elementName,
                pathExpression, mode, bindingSettings);
        }

        public AmmyBuilderExtender<TBuilder, TPropertyBrowser> WithPropertyFromElementName2<TElement>(
            Expression<Func<TPropertyBrowser, object>> propertyExpression,
            string elementName,
            Expression<Func<TElement, object>> pathExpression,
            XBindingMode mode, [CanBeNull] Action<AmmyBind> bindingSettings = null)
        {
            var path = ExpressionTools.GetBindingPath(pathExpression);
            var bind = new AmmyBindBuilder(path)
            {
                From = new ElementNameBindingSource(elementName),
                Mode = mode
            }.Build();
            if (bindingSettings != null)
                bindingSettings(bind);
            var propertyName = ExpressionTools.GetBindingPath(propertyExpression);
            Builder.WithProperty(propertyName, bind);
            return this;
        }

        public TBuilder Builder { get; }

        public IDictionary<string, object> Properties => Builder.Properties;
        public const string DataContextPropertyName = "DataContext";


        public sealed class Browser<TSelf> : IAmmyCodePieceConvertible
        {
            public Browser(AmmyBuilderExtender<TBuilder, TPropertyBrowser> extender) => Extender = extender;

            public Element<TElement> ChooseElement<TElement>(Expression<Func<TSelf, TElement>> propertyNameExpression)
            {
                var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);
                return new Element<TElement>(propertyName, this);
            }

            public IAmmyCodePiece ToAmmyCode(IConversionCtx ctx) => Extender.ToAmmyCode(ctx);

            public AmmyBuilderExtender<TBuilder, TPropertyBrowser> Extender { get; }

            public sealed class Element<TElement>:IAmmyCodePieceConvertible
            {
                public Element(string propertyName, Browser<TSelf> browser)
                {
                    PropertyName = propertyName;
                    Browser      = browser;
                }

                public Element<TElement> BindFromElement(Expression<Func<TPropertyBrowser, object>> func1,
                    Expression<Func<TElement, object>> func2,
                    XBindingMode? mode = null)
                {
                    var propertyName = ExpressionTools.GetBindingPath(func1);
                    var path2                   = ExpressionTools.GetBindingPath(func2);
                    var bind = new AmmyBindBuilder(path2)
                    {
                        From = new ElementNameBindingSource(PropertyName),
                        Mode = mode
                    }.Build();
                    var extender = Browser.Extender;
                    extender.Builder.WithProperty(propertyName, bind);
                    return this;
                }

                public DataContextBinder<TDataContext> WithDataContext<TDataContext>(
                    Expression<Func<TElement, TDataContext>> elementToDataContextExpression,
                    XBindingMode mode = XBindingMode.OneWay)
                {
                    /*var elementName = ExpressionTools.GetBindingPath(elementNameExpression);
                    */
                    var path = ExpressionTools.GetBindingPath(elementToDataContextExpression);
                    var bind = new AmmyBindBuilder(path)
                    {
                        From = new ElementNameBindingSource(PropertyName),
                        Mode = mode
                    }.Build();
                    var extender = Browser.Extender;
                    extender.Builder.WithProperty(DataContextPropertyName, bind);
                    return new DataContextBinder<TDataContext>(extender);
                }

                public string         PropertyName { get; }
                public Browser<TSelf> Browser      { get; }
                public IAmmyCodePiece ToAmmyCode(IConversionCtx ctx) => Browser.ToAmmyCode(ctx);
            }
        }
    }
}