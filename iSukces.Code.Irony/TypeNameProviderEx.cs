using System;
using JetBrains.Annotations;

#nullable disable
namespace iSukces.Code.Irony
{
    public class TypeNameProviderEx
    {
        public TypeNameProviderEx([NotNull] ITypeNameProvider provider, TypeNameProviderFlags flags)
        {
            Provider = provider ?? throw new ArgumentNullException(nameof(provider));
            Flags    = flags;
        }

        public static TypeNameProviderEx MakeBuiltIn<T>()
        {
            var flags = TypeNameProviderFlags.BuildIn;
            if (typeof(T).IsInterface)
                flags |= TypeNameProviderFlags.IsInterface;
            return new TypeNameProviderEx(new TypeNameProvider(typeof(T)), flags);
        }

        [NotNull]
        public ITypeNameProvider Provider { get; }

        public TypeNameProviderFlags Flags { get; }

        public bool CreateAutoCode
        {
            get { return (Flags & TypeNameProviderFlags.CreateAutoCode) != 0; }
        }

        public bool IsInterface
        {
            get { return (Flags & TypeNameProviderFlags.IsInterface) != 0; }
        }

        public bool BuiltInOrAutocode
        {
            get
            {
                const TypeNameProviderFlags mask = TypeNameProviderFlags.BuildIn | TypeNameProviderFlags.CreateAutoCode;
                return (Flags & mask) != 0;
            }
        }
    }

    [Flags]
    public enum TypeNameProviderFlags : byte
    {
        None = 0,
        IsInterface = 1,
        BuildIn = 2,
        CreateAutoCode = 4
    }
}

