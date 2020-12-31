using iSukces.Code.Interfaces;

namespace iSukces.Code.Irony
{
    public class StringTypeNameProvider : ITypeNameProvider
    {
        public StringTypeNameProvider(string className) => ClassName = className;

        public FullTypeName GetTypeName(ITypeNameResolver resolver, string baseNamespace)
        {
            if (ClassName.StartsWith("."))
                return new FullTypeName(baseNamespace + ClassName);
            return new FullTypeName(ClassName);
        }

        public string ClassName { get; }
    }
}