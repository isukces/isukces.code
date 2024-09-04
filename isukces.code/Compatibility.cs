#nullable enable
#nullable enable
#nullable enable
#nullable enable
#if NET6_0_OR_GREATER

#if false
     sealed class Fejk
{
    #region Fields

    System.Runtime.CompilerServices.CompilerFeatureRequiredAttribute a;
    System.Runtime.CompilerServices.RequiredMemberAttribute b;

    #endregion
} 
  #endif

#else
namespace System.Runtime.CompilerServices
{
    public sealed class CompilerFeatureRequiredAttribute : Attribute
    {
        public CompilerFeatureRequiredAttribute(string featureName)
        {
            FeatureName = featureName;
        }

        public string FeatureName { get; set; }
    }

    public sealed class RequiredMemberAttribute : Attribute
    {
    }

    public static class IsExternalInit
    {
    }
}


#endif
