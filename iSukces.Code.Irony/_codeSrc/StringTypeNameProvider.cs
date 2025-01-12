using iSukces.Code.Interfaces;

#nullable disable
namespace iSukces.Code.Irony
{
    public class StringTypeNameProvider : ITypeNameProvider
    {
        public StringTypeNameProvider(string className) => ClassName = className;

        public FullTypeName GetTypeName(ITypeNameResolver resolver, string baseNamespace)
        {
            if (ClassName.StartsWith("."))
                return new FullTypeName((CsType)(baseNamespace + ClassName));
            return new FullTypeName((CsType)ClassName);
        }

        public string ClassName { get; }
    }
}

