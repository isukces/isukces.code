using System.Collections.Generic;
using System.Linq;
using iSukces.Code.Interfaces;
using iSukces.Translation;
using JetBrains.Annotations;

namespace iSukces.Code.Translations;

public delegate void AfterCreatedDelegate(string fieldName, string sourceTextToTranslate);


public static class LiteLocalTextSourceGenerator
{
    public static void Create(CsClass csClass, IList<ICreateLiteLocalTextRequest> requests,
        AfterCreatedDelegate afterCreated)
    {
        if (requests.Count == 0)
            return;
        requests = requests.OrderBy(a => a.Key).ToList();
        var writer = new CsMethodCodeWriter(csClass);
        writer.Location = SourceCodeLocation.Make()
            .WithGeneratorClass(typeof(LiteLocalTextSourceGenerator));
        writer.WriteLine("// generator : " + writer.Location.ToString());
        //writer.WriteLine("#if DEBUG");
        var tLiteLocalTextSource                = csClass.GetTypeName<LiteLocalTextSource>();
        var tCreateLiteLocalTextSources_Request = csClass.GetTypeName<CreateLiteLocalTextSources_Request>();
            

        {
            var code =CsCodeWriter.Create(typeof(LiteLocalTextSourceGenerator));
            foreach (var request in requests)
                code.WriteLine($"{request.FieldName}.{nameof(LiteLocalTextSource.Attach)}(th);");

            var m = csClass.AddMethod("AttachAllTextSources", CsType.Void)
                .WithVisibility(Visibilities.Public)
                .WithStatic()
                .WithBody(code);
            m.AddParam<ITranslationHolder>("th", csClass)
                .WithAttribute(csClass, typeof(NotNullAttribute));
            TranslationAutocodeConfig.Instance.AddAppInitAttribute?.Invoke(m, csClass);

        }
            
        foreach (var request in requests)
        {
            var sourceTextToTranslate = request.GetSourceTextToTranslate();
            {
                var field = csClass.AddField(request.FieldName, tLiteLocalTextSource);
                field.IsReadOnly  = true;
                field.IsStatic    = true;
                field.Visibility  = Visibilities.Public;
                field.ConstValue  = tLiteLocalTextSource.New(request.Key.CsEncode(), sourceTextToTranslate.CsEncode()); 
                field.Description = $"Text: {sourceTextToTranslate}\r\n{csClass.Name.GetMemberCode(request.FieldName)}";
            }
            afterCreated(request.FieldName, sourceTextToTranslate);

                
            var properties = new List<string>();

            void Add(string name, string code)
            {
                if (code != "null")
                    properties.Add(name + "= " + code);
            }

            if (request.FieldName != CreateLiteLocalTextSources_Request.DefaultFieldName(request.Key))
            {
                var code = "nameof(" + request.FieldName + ")";
                Add(nameof(CreateLiteLocalTextSources_Request.FieldName), code);
            }

            var constructorArguments =
                request.Key.CsEncode() + ", " + request.GetSourceTextToTranslate().CsEncode();
            if (!string.IsNullOrEmpty(request.TranslationHint))
                constructorArguments += ", " + request.TranslationHint.CsEncode();

            var propertySettings = string.Join(", ", properties);
            if (!string.IsNullOrEmpty(propertySettings))
                propertySettings = " {" + propertySettings + "}";

            var ex1       = tCreateLiteLocalTextSources_Request.New(constructorArguments);
            var yieldCode = $"yield return {ex1}{propertySettings};";
            writer.WriteLine(yieldCode);
        }

        // writer.WriteLine("#else").WriteLine("yield break;").WriteLine("#endif");

        var tResult = csClass.GetTypeName<IEnumerable<CreateLiteLocalTextSources_Request>>();
        var method = csClass.AddMethod(LiteLocalTextSourceScannerBase.MethodName, tResult)
            .WithBody(writer)
            .WithStatic()
            .WithVisibility(Visibilities.Public);
        method.CompilerDirective = "DEBUG";
    }
}