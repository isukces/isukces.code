#nullable enable
using System.Reflection;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;

namespace iSukces.Code;

public class GetCodeFilePathGenerator : Generators.SingleClassGenerator
{
    protected override void GenerateInternal()
    {
        const string name1  = "GetCodeFilePath";
        var          name   = Type.GetCustomAttribute<AutocodeCustomOutputMethodAttribute>(false)?.MethodName ?? name1;
        var          method = Type.GetMethod(name, GeneratorsHelper.AllStatic);
        if (method is null)
            return;
        var m = Class
            .AddMethod(name, CsType.String)
            .WithStatic()
            .WithBodyAsExpression("CodeFileUtils.GetCallerFilePath()");
        m.CompilerDirective = "DEBUG";
        m.WithAttribute(CsAttribute.Make<JetBrains.Annotations.UsedImplicitlyAttribute>(Class));

        var t = Type;
        while (t.IsNested)
        {
            t = t.DeclaringType;
            if (t is null)
                return;
            method = t.GetMethod(name, GeneratorsHelper.AllStatic);
            if (method != null)
            {
                var attribute = new CsAttribute("SuppressMessage")
                    .WithArgumentCode("ReSharper".CsEncode())
                    .WithArgumentCode("MemberHidesStaticFromOuterClass".CsEncode());
                m.WithAttribute(attribute);
                return;
            }
                
        }
            
    }
}