using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace isukces.code.vssolutions
{
    public sealed class FrameworkVersion : IComparable<FrameworkVersion>, IComparable
    {
        public FrameworkVersion(string shortName, string version, string profile)
        {
            shortName      = shortName.ToLower();
            FrameworkGroup = RecognizeFrameworkVersionGroup(shortName);
            ShortName      = shortName;
            Version        = version;
            Profile        = profile;
            VersionCompare = version.Replace(".", "");
        }

        public static bool operator >(FrameworkVersion left, FrameworkVersion right)
        {
            return Comparer<FrameworkVersion>.Default.Compare(left, right) > 0;
        }

        public static bool operator >=(FrameworkVersion left, FrameworkVersion right)
        {
            return Comparer<FrameworkVersion>.Default.Compare(left, right) >= 0;
        }

        public static bool operator <(FrameworkVersion left, FrameworkVersion right)
        {
            return Comparer<FrameworkVersion>.Default.Compare(left, right) < 0;
        }

        public static bool operator <=(FrameworkVersion left, FrameworkVersion right)
        {
            return Comparer<FrameworkVersion>.Default.Compare(left, right) <= 0;
        }


        public static FrameworkVersion[] Parse(string x)
        {
            {
                var portable = "portable-";
                if (x.StartsWith(portable))
                {
                    var xx = x.Substring(portable.Length).Split('+');
                    return xx.Select(Parse1).ToArray();
                }
            }
            {
                var portable = "portable40-";
                if (x.StartsWith(portable))
                {
                    var xx = x.Substring(portable.Length).Split('+');
                    return xx.Select(Parse1).ToArray();
                }
            }


            return new[] {Parse1(x)};
        }

        public static FrameworkVersion Parse1(string x)
        {
            var m = VersionRegex.Match(x);
            if (m.Success)
            {
                var type    = m.Groups[1].Value;
                var version = m.Groups[2].Value;
                var profile = m.Groups[3].Value;
                if (type == "v")
                    type = "net";
                return new FrameworkVersion(type, version, profile);
            }

            return null;
        }

        private static FrameworkVersionGroup RecognizeFrameworkVersionGroup(string shortName)
        {
            // https://docs.microsoft.com/pl-pl/nuget/reference/target-frameworks
            if (shortName.StartsWith("xamarin", StringComparison.Ordinal))
                return FrameworkVersionGroup.Xamarin;
            switch (shortName)
            {
                case "net":
                    return FrameworkVersionGroup.Framework;
                case "netcore":
                    return FrameworkVersionGroup.NetCore;

                case "netcoreapp":
                case "aspnet": // Deprecated 
                case "aspnetcore": // Deprecated
                case "dnx": // Deprecated
                case "dnxcore": // Deprecated
                    return FrameworkVersionGroup.NetCoreApp;

                case "netstandard":
                case "dotnet": // Deprecated
                    return FrameworkVersionGroup.NetStandard;

                case "wp":
                case "wpa":
                    return FrameworkVersionGroup.WindowsPhone;
                case "sl":
                    return FrameworkVersionGroup.Silverlight;
                case "win":
                case "winrt": // Deprecated
                    return FrameworkVersionGroup.Windows;
                case "netmf":
                    return FrameworkVersionGroup.MicroFramework;
                case "uap":
                    return FrameworkVersionGroup.UniversalWindowsPlatform;
                case "tizen":
                    return FrameworkVersionGroup.Tizen;
                default:
                    return FrameworkVersionGroup.Other;
            }
        }


        public NugetLoadCompatibility CanLoad(FrameworkVersion nuget)
        {
            if (FrameworkGroup == nuget.FrameworkGroup)
            {
                var g = string.Compare(VersionCompare, nuget.VersionCompare, StringComparison.OrdinalIgnoreCase);
                return g == 0
                    ? NugetLoadCompatibility.Full
                    : g > 0
                        ? NugetLoadCompatibility.Possible
                        : NugetLoadCompatibility.None;
            }

            if (FrameworkGroup == FrameworkVersionGroup.UniversalWindowsPlatform)
                switch (nuget.FrameworkGroup)
                {
                    case FrameworkVersionGroup.Windows:
                    case FrameworkVersionGroup.WindowsPhone:
                        return nuget.VersionCompare == "81"
                            ? NugetLoadCompatibility.Partial
                            : NugetLoadCompatibility.None;
                    case FrameworkVersionGroup.NetCore:
                        return nuget.VersionCompare == "50"
                            ? NugetLoadCompatibility.Partial
                            : NugetLoadCompatibility.None;
                }

            return NugetLoadCompatibility.None;
        }

        public int CompareTo(FrameworkVersion other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var compareTypeComparison = FrameworkGroup.CompareTo(other.FrameworkGroup);
            if (compareTypeComparison != 0) return compareTypeComparison;
            return string.Compare(VersionCompare, other.VersionCompare, StringComparison.Ordinal);
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            if (ReferenceEquals(this, obj)) return 0;
            return obj is FrameworkVersion other
                ? CompareTo(other)
                : throw new ArgumentException($"Object must be of type {nameof(FrameworkVersion)}");
        }

        public override string ToString()
        {
            return Name;
        }

        public FrameworkVersionGroup FrameworkGroup { get; }
        public string                ShortName      { get; }
        public string                Version        { get; }
        public string                Profile        { get; }
        public string                VersionCompare { get; }

        public string Name
        {
            get
            {
                var name = ShortName + Version;
                if (string.IsNullOrEmpty(Profile))
                    return name;
                return name + "-" + Profile;
            }
        }

        private static readonly Regex VersionRegex =
            new Regex(VersionFilter, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        const string VersionFilter = @"^([a-z]+)(\d+(?:\.\d+(?:\.\d+)?)?)(?:-([a-z]+))?$";
    }
#if OLD
^([a-z]+)
(\d+(?:\.\d+(?:\.\d+)?)?)
(?:-([a-z]+))?$
#endif
}