using System;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Ammy
{
    public class ConversionCtx : IConversionCtx
    {
        public ConversionCtx(IAmmyNamespaceProvider namespaceProvider, bool fullNamespaces = false)
        {
            NamespaceProvider = namespaceProvider;
            FullNamespaces    = fullNamespaces;
        }

        public bool ResolveSeparateLines(string propertyName, IAmmyCodePiece code, object sourceValue, object sourceValueHost)
        {
            var h = OnResolveSeparateLines;
            if (h == null)
                return code.WriteInSeparateLines;
            var a = new ResolveSeparateLinesEventArgs
            {
                PropertyName         = propertyName,
                Code                 = code,
                WriteInSeparateLines = code.WriteInSeparateLines,
                SourceValue          = sourceValue,
                SourceValueHost      = sourceValueHost
            };
            h.Invoke(this, a);
            return a.WriteInSeparateLines;
        }

        public IAmmyNamespaceProvider NamespaceProvider { get; }
        public bool                   FullNamespaces    { get; set; }

        public event EventHandler<ResolveSeparateLinesEventArgs> OnResolveSeparateLines;

        public class ResolveSeparateLinesEventArgs : EventArgs
        {
            public string PropertyName { get; set; }

            /// <summary>
            ///     Code of converted SourceValue
            /// </summary>
            public IAmmyCodePiece Code { get; set; }

            public bool WriteInSeparateLines { get; set; }

            /// <summary>
            ///     Value that was converted to Code
            /// </summary>
            public object SourceValue { get; set; }
            
            /// <summary>
            /// Object that: SourceValue.PropertyName = SourceValue
            /// </summary>
            public object SourceValueHost { get; set; }

            public bool Handled { get; set; }
        }
    }
}