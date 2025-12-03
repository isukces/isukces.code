namespace iSukces.Code;

public static class CsMethodParameterExtensions
{
    extension<T>(T src)
        where T : CsMethodParameter
    {
        public T WithConstValue(string constValue)
        {
            src.ConstValue = constValue;
            return src;
        }

        public T WithConstValueNull()
        {
            src.ConstValue = "null";
            return src;
        }

        public T WithDescription(string? description)
        {
            src.Description = description;
            return src;
        }

        public T WithIsReadOnly(bool isReadOnly = true)
        {
            src.IsReadOnly = isReadOnly;
            return src;
        }

        public T WithIsVolatile(bool isVolatile = true)
        {
            src.IsVolatile = isVolatile;
            return src;
        }
    }
}
