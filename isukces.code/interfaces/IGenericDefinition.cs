using System.Linq;

namespace iSukces.Code.Interfaces
{
    public interface IGenericDefinition
    {
        CsGenericArguments GenericArguments { get; set; }
    }

    public static class GenericDefinitionExtensions
    {
        public static string GetTriangleBracketsInfo(this CsGenericArguments genericArguments)
        {
            var types = genericArguments?.Types;
            if (types == null || types.Count == 0)
                return string.Empty;
            return "<" + types.CommaJoin() + ">";
        }

        public static bool HasConstraints(this CsGenericArguments self)
        {
            if (self is null || self.Constraints.Count == 0)
                return false;
            var types = self.Types.ToHashSet();
            return self.Constraints.Any(i => types.Contains(i.TypeName));
        }
    }
}