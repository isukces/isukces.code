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

        public bool ResolveSeparateLines(string propertyName, IAmmyCodePiece value, object sourceObject)
        {
            var h = OnResolveSeparateLines;
            if (h == null)
                return value.WriteInSeparateLines;
            var a = new ResolveSeparateLinesEventArgs
            {
                PropertyName         = propertyName,
                Value                = value,
                WriteInSeparateLines = value.WriteInSeparateLines,
                SourceObject         = sourceObject
            };
            h.Invoke(this, a);
            return a.WriteInSeparateLines;
        }

        public IAmmyNamespaceProvider NamespaceProvider { get; }
        public bool                   FullNamespaces    { get; set; }

        public event EventHandler<ResolveSeparateLinesEventArgs> OnResolveSeparateLines;

        public class ResolveSeparateLinesEventArgs : EventArgs
        {
            public string         PropertyName         { get; set; }
            public IAmmyCodePiece Value                { get; set; }
            public bool           WriteInSeparateLines { get; set; }
            public object         SourceObject         { get; set; }
        }
    }
}