using System;

namespace isukces.code.Ammy
{
    public class AmmyGlobals
    {
        private AmmyGlobals()
        {
        }

        public bool TryResolveTypeName(Type type, out string o)
        {
            o = null;
            var handler = ResolveTypeName;
            if (handler is null) return false;

            var args = new ResolveTypeNameEventArgs(type);
            handler(this, args);
            if (!args.Handled || string.IsNullOrEmpty(args.TypeName))
                return false;
            o = args.TypeName;
            return true;
        }

        public static AmmyGlobals Instance
        {
            get { return InstanceHolder.SingleInstance; }
        }


        public event EventHandler<ResolveTypeNameEventArgs> ResolveTypeName;

        private sealed class InstanceHolder
        {
            public static readonly AmmyGlobals SingleInstance = new AmmyGlobals();
        }

        public sealed class ResolveTypeNameEventArgs : EventArgs
        {
            public ResolveTypeNameEventArgs(Type type) => RequestedType = type;

            public void Accept(string typeName)
            {
                Handled  = true;
                TypeName = typeName;
            }

            public Type   RequestedType { get; }
            public bool   Handled       { get; private set; }
            public string TypeName      { get; private set; }
        }
    }
}