#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Db;

public class DbGegeneratorsHelper
{
    
    private static bool MarkCode(CsClass csClass, SourceCodeLocation codeLocation, string propertyName)
    {
        var key = $"{codeLocation}//{propertyName}";
        if (csClass.UserAnnotations.TryGetValue(key, out var info))
        {
            Debug.WriteLine(info);
            return true;
        }

        csClass.UserAnnotations[key] = Environment.StackTrace;

        return false;
    }
    
    /// <summary>
    /// Creates inverse property for data entities
    /// </summary>
    /// <param name="context"></param>
    /// <param name="propertyType"></param>
    /// <param name="attr"></param>
    /// <param name="addOptionalAttributes"></param>
    public  static void CreateInverseNavigationProperty(
        IAutoCodeGeneratorContext context, 
        Type propertyType,
        AutoNavigationAttribute attr,
        Action<CsClass, CsProperty>? addOptionalAttributes = null)
    {
        if (attr.GenerateInverse == InverseKind.None)
            return;
        var isCollection           = attr.GenerateInverse == InverseKind.Collection;
        var csClassI               = context.GetOrCreateClass(attr.Type);
        var allowReferenceNullable = csClassI.AllowReferenceNullable();

        var propertyName = attr.Inverse;
        var codeLocation = SourceCodeLocation.Make();
        if (MarkCode(csClassI, codeLocation, propertyName))
            return;
        // var hostType = ;
        var typeName = GetGenericType(typeof(ICollection<>), propertyType);
        var invp = csClassI.AddProperty(propertyName, typeName)
            .WithAttributeBuilder(csClassI)
            .WithAttribute<NavigationPropertyAttribute>()
            .End();
        if (addOptionalAttributes is not null)
            addOptionalAttributes(csClassI, invp);
        
        // invp.Attributes.Add(new CsAttribute(csClassI.GetTypeName<JsonIgnoreAttribute>()));
        invp.Description                 = SourceCodeLocation.Make().ToString();
        invp.MakeAutoImplementIfPossible = true;

        if (!isCollection) return;
        typeName        = GetGenericType(typeof(List<>), propertyType);
        invp.ConstValue = $"new {typeName.Declaration}()";
        return;

        CsType GetGenericType(Type tList, Type tElement)
        {
            if (isCollection)
                tElement = tList.MakeGenericType(tElement);
            return csClassI.GetTypeName(tElement);
        }
    }
}
