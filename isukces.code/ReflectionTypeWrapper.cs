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
            _type = type;
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
            if (IsEnum)
                return typeof(int?);
            if (IsValueType)
                return typeof(Nullable<>).MakeGenericType(_type);
            return _type;
        }

        public Type UnwrapNullable(bool nullIfNotNullable = false)
        {
            if (_type == null) return null;
            if (!_typeInfo.IsGenericType) return nullIfNotNullable ? null : _type;
            var gt = _typeInfo.GetGenericTypeDefinition();
            if (gt != typeof(Nullable<>)) return nullIfNotNullable ? null : _type;
            var args = _typeInfo.GetGenericArguments();
            return args[0];
        }

        public bool IsEnum        => _typeInfo?.IsEnum ?? false;
        public bool IsValueType   => _typeInfo?.IsValueType ?? false;
        public bool IsGenericType => _typeInfo?.IsGenericType ?? false;

        private readonly Type _type;
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