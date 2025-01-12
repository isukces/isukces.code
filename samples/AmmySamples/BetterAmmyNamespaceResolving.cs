#nullable disable
#if DEBUG
#if ACTIVE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Markup;
using iSukces.Code.Ammy;

namespace AmmySamples
{
    public class BetterAmmyNamespaceResolving
    {
        /// <summary>
        /// Use this method to attach global resolver
        /// </summary>
        public static void Init()
        {
            AmmyGlobals.Instance.ResolveTypeName += (a, b) =>
            {
                var canUseShortName = CanUseShortName(b.RequestedType);
                if (canUseShortName)
                    b.Accept(b.RequestedType.Name);
            };
        }

        private static bool CanUseShortName(Type type)
        {
            var ass = type.Assembly;
            if (!_presentationFrameworkXamlNamespaces.TryGetValue(ass, out var dict))
                _presentationFrameworkXamlNamespaces[ass] = dict = new Dictionary<string, bool>();

            var ns = type.Namespace;
            if (string.IsNullOrEmpty(ns))
                return false;

            if (dict.TryGetValue(ns, out var useShortName))
                return useShortName;
            var ats = ass.GetCustomAttributes<XmlnsDefinitionAttribute>();

            var my = ats
                .FirstOrDefault(aa => aa.ClrNamespace == ns
                                      && aa.XmlNamespace == presentationNs);
            dict[ns] = useShortName = my != null;
            return useShortName;
        }

        private static readonly Dictionary<Assembly, Dictionary<string, bool>> _presentationFrameworkXamlNamespaces =
            new Dictionary<Assembly, Dictionary<string, bool>>();

        private const string presentationNs = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";
    }
}
#endif
#endif
