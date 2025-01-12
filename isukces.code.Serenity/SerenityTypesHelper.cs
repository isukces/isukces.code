#nullable disable
using System;
using System.Reflection;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Serenity
{
    public enum SerenityClassRole
    {
        Unknown,
        Row,
        Form
    }

    public class SerenityTypesHelper
    {
        
        public SerenityTypesHelper(Type clrType, SerenityClassRole role)
        {
            ClrType = clrType;
            var at = new AbstractType(clrType);
            var ns = ProcessNamespace(at.Namespace, role);
            TsType = at.MoveToNs(ns);
        }

        public static bool IsRowType(Type rowType)
        {
            return SerenityCodeSettings.GetSerenityRowType()
                .GetTypeInfo().IsAssignableFrom(rowType);
        }

        private static string ProcessNamespace(string ns, SerenityClassRole role)
        {
            switch (role)
            {
                case SerenityClassRole.Row:
                    return TrimEnd(ns, ".Entities");
                case SerenityClassRole.Form:
                    return TrimEnd(ns, ".Forms");
                case SerenityClassRole.Unknown:
                    return ns;
                default:
                    throw new ArgumentOutOfRangeException(nameof(role), role, null);
            }
        }
        
        private static string TrimEnd(string text, string ending)
        {
            if (string.IsNullOrEmpty(ending))
                return text;
            if (text.EndsWith(ending, StringComparison.OrdinalIgnoreCase))
                text = text.Substring(0, text.Length - ending.Length);
            return text;
        }


        public AbstractType TsType { get; }

        public Type ClrType { get; }
    }
}
