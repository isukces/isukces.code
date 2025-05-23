#nullable disable
namespace iSukces.Code.Irony
{
    internal sealed class Exchange
    {
        public const string MethodForEvaluatingPropertyKey = "=MethodForEvaluatingProperty=";
        public const string X = "=MethodForEvaluatingProperty=";

        public sealed class MethodForEvaluatingProperty
        {
            public MethodForEvaluatingProperty(string propertyName, string methodName, bool needThreadVariable)
            {
                PropertyName = propertyName;
                MethodName   = methodName;
                NeedThread   = needThreadVariable;
            }

            public string PropertyName { get; }
            public string MethodName   { get; }
            public bool   NeedThread   { get; }
        }

        public sealed class Du
        {
            public string GetNodeKindMethodName { get; }

            public Du(string getNodeKindMethodName)
            {
                GetNodeKindMethodName = getNodeKindMethodName;
                
            }
        }
    }
}

