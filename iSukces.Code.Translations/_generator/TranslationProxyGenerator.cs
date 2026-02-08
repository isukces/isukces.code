using System;
using System.Collections.Generic;
using System.Linq;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Translations;

public static class TranslationProxyGenerator  
{

    public static void GenerateProxyProperties(CsClass csClass, IEnumerable<ITranslationProxyCreateRequest> req,
        bool fullThis, Type proxyType)
    {
        var notifyAllBody  = new CsCodeWriter();
        var holderInstance = "this";
        if (fullThis) 
            holderInstance = TranslationAutocodeConfig.Instance.GetInstanceHolderInstanceCsExpression(csClass);

        var dict = TranslationPropertiesDictionary.GetOrCreate(csClass.UserAnnotations);

        foreach (var createRequest in req.OrderBy(a => a.Key))
        {
            var existingPropertyForTranslationKey = dict.PropertyForKey(createRequest.Key);
            var propName                          = createRequest.ProxyPropertyName;
                
            if (createRequest.CanChangePropertyName)
            {
                if (string.IsNullOrEmpty(propName))
                    propName = existingPropertyForTranslationKey;
                if (string.IsNullOrEmpty(propName))
                    propName = dict.ForgeProxyPropertyName(createRequest.Key.Split('.').Last(), createRequest.Key);
            }

            if (string.IsNullOrEmpty(propName))
                throw new Exception("Empty property name");

            if (string.IsNullOrEmpty(existingPropertyForTranslationKey))
            {
                if (!dict.PropertyExists(propName))
                    dict.Register(createRequest.Key, propName);
                var p = csClass.AddProperty(propName, CsType.String);
                p.SetterType = PropertySetter.None;
                p.EmitField  = false;
                var getter = holderInstance + "[" + createRequest.Key.CsEncode() + "]";
                p.OwnGetter             = PropertyGetterCode.Value(getter);
            }
            notifyAllBody.WriteLine($"OnPropertyChanged(nameof({propName}));");
        }

        csClass.AddMethod("NotifyAll", CsType.Void).WithBody(notifyAllBody).WithVisibility(Visibilities.Private);
        if (fullThis && TranslationAutocodeConfig.Instance.TranslationManager != proxyType)
        {
            var body = "(a, b) => { NotifyAll(); };";
            body = $"{holderInstance}.OnChangeTranslations += {body}";
            csClass.AddConstructor().WithBody(body);
        }
    }

    public static string[] GetNamespaces()
    {
        var q = new[]
        {
            TranslationAutocodeConfig.Instance.TranslationHolder,
        };
        return q.Select(a => a.Namespace).Distinct().ToArray();
    }

    private static string GetTypeNameOriginal<T>(INamespaceContainer csClass)
    {
        var type = typeof(T);
        var a =csClass.IsKnownNamespace(type.Namespace ??"")
            ? type.Name
            : type.FullName;
        if (a is null)
            throw new Exception("Unable to get type name");
        return a;
    }
}