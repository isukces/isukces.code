using System;
using System.Reflection;

namespace isukces.code
{
    public partial struct ReflectionTypeWrapper
    {
        public ReflectionTypeWrapper(Type type)
        {
#if COREFX
            _typeInfo = type?.GetTypeInfo();
#else
            _typeInfo = type;
#endif
            Type = type;
        }

        public Type[] GetGenericArguments()
        {
            return _typeInfo.GetGenericArguments();
        }

        public Type GetGenericTypeDefinition()
        {
            return _typeInfo.GetGenericTypeDefinition();
        }

        public Type MakeNullableIfPossible()
        {
            if (IsValueType)
                return typeof(Nullable<>).MakeGenericType(Type);
            return Type;
        }

        public Type UnwrapNullable(bool nullIfNotNullable = false)
        {
            if (Type == null) return null;
            if (!_typeInfo.IsGenericType) return nullIfNotNullable ? null : Type;
            var gt = _typeInfo.GetGenericTypeDefinition();
            if (gt != typeof(Nullable<>)) return nullIfNotNullable ? null : Type;
            var args = _typeInfo.GetGenericArguments();
            return args[0];
        }

        public bool IsEnum        => _typeInfo?.IsEnum ?? false;
        public bool IsValueType   => _typeInfo?.IsValueType ?? false;
        public bool IsGenericType => _typeInfo?.IsGenericType ?? false;
        public bool IsGenericTypeDefinition => _typeInfo?.IsGenericTypeDefinition ?? false;

        public Type Type { get; }
    }


#if COREFX
    public partial struct ReflectionTypeWrapper
    {
        private readonly TypeInfo _typeInfo;
    }
#else
    public partial struct ReflectionTypeWrapper
    {
        private readonly Type _typeInfo;
        
    }
#endif
}