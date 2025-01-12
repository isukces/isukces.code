using System.Collections.Generic;

namespace iSukces.Code;

public class GlobalUsingsConfiguration : NamespacesHolder
{
    public GlobalUsingsConfiguration()
        : base(null)
    {
    }

    public GlobalUsingsConfiguration WithAsp()
    {
        Add(Asp);
        return this;
    }

    public GlobalUsingsConfiguration WithStandard()
    {
        Add(Standard);
        return this;
    }

    #region Fields

    /// <summary>
    ///     https://endjin.com/blog/2021/09/dotnet-csharp-10-implicit-global-using-directives
    /// </summary>
    public static IReadOnlyList<string> Standard = new[]
    {
        "System",
        "System.Collections.Generic",
        "System.IO",
        "System.Linq",
        "System.Net.Http",
        "System.Threading",
        "System.Threading.Tasks"
    };


    public static IReadOnlyList<string> Asp = new[]
    {
        "System",
        "System.Collections.Generic",
        "System.IO",
        "System.Linq",
        "System.Net.Http",
        "System.Threading",
        "System.Threading.Tasks",
        "System.Net.Http.Json",
        "Microsoft.AspNetCore.Builder",
        "Microsoft.AspNetCore.Hosting",
        "Microsoft.AspNetCore.Http",
        "Microsoft.AspNetCore.Routing",
        "Microsoft.Extensions.Configuration",
        "Microsoft.Extensions.DependencyInjection",
        "Microsoft.Extensions.Hosting",
        "Microsoft.Extensions.Logging"
    };

    private readonly HashSet<string> _namespaces = [];

    #endregion
}
