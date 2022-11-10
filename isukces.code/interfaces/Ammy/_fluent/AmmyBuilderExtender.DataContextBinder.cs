#if AMMY
using System;
using System.Linq.Expressions;
using iSukces.Code.Ammy;
using iSukces.Code.Compatibility.System.Windows.Data;
using JetBrains.Annotations;

namespace iSukces.Code.Interfaces.Ammy
{
    public sealed partial class AmmyBuilderExtender<TBuilder, TPropertyBrowser>
    {
        public sealed class DataContextBinder<TDataContext> : IAmmyCodePieceConvertible
        {
            public DataContextBinder(AmmyBuilderExtender<TBuilder, TPropertyBrowser> extender)
            {
                _extender = extender;
            }

            public DataContextBinder<TDataContext> Bind(
                Expression<Func<TPropertyBrowser, object>> propertyNameExpression,
                Expression<Func<TDataContext, object>> bindToPathExpression,
                XBindingMode? mode = null,
                [CanBeNull] Action<AmmyBind> bindingSettings = null)
            {
                var bindToPath = ExpressionTools.GetBindingPath(bindToPathExpression);
                return Bind(propertyNameExpression, bindToPath, mode, bindingSettings);
            }
            
            public DataContextBinder<TDataContext> Bind(
                string propertyName,
                Expression<Func<TDataContext, object>> bindToPathExpression,
                XBindingMode? mode = null,
                [CanBeNull] Action<AmmyBind> bindingSettings = null)
            {
                var bindToPath = ExpressionTools.GetBindingPath(bindToPathExpression);
                return Bind(propertyName, bindToPath, mode, bindingSettings);
            }

            public DataContextBinder<TDataContext> Bind(
                Expression<Func<TPropertyBrowser, object>> propertyNameExpression,
                string bindToPath,
                XBindingMode? mode = null,
                [CanBeNull] Action<AmmyBind> bindingSettings = null)
            {
                var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);
                return Bind(propertyName, bindToPath, mode, bindingSettings);
            }

            public DataContextBinder<TDataContext> Bind(
                string propertyName,
                string bindToPath,
                XBindingMode? mode = null,
                [CanBeNull] Action<AmmyBind> bindingSettings = null)
            {
                var bind = new AmmyBind(bindToPath);
                if (mode != null)
                    bind.WithMode(mode.Value);
                bindingSettings?.Invoke(bind);
                _extender.Builder.WithProperty(propertyName, bind);
                return this;
            }


            public AmmyBuilderExtender<TBuilder, TPropertyBrowser> CloseDataContext()
            {
                return _extender;
            }


            public IAmmyCodePiece ToAmmyCode(IConversionCtx ctx)
            {
                return _extender.ToAmmyCode(ctx);
            }

            private readonly AmmyBuilderExtender<TBuilder, TPropertyBrowser> _extender;

         
        }
    }
}
#endif