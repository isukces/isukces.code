using System;
using System.Collections.Generic;
using System.Linq;
using iSukces.Code.Interfaces;

namespace iSukces.Code;

internal sealed class CsClassWriter
{
    private readonly bool _allowReferenceNullable;

    public CsClassWriter(CsClass @class)
    {
        Class = @class;
        _allowReferenceNullable = @class.AllowReferenceNullable();
    }

    internal bool WriteMethodAction<T>(ICsCodeWriter writer, IEnumerable<T> list, string region, Action<T> action)
    {
        var enumerable = list as IList<T> ?? list.ToList();
        if (!enumerable.Any()) return false;
        var hasRegions = Features.Flags.HasFlag(CodeFormattingFeatures.Regions);
        if (hasRegions)
        {
            writer.WriteLine("#region " + region);
            writer.EmptyLine();
        }

        foreach (var i in enumerable)
            action(i);
        if (hasRegions)
            writer.WriteLine("#endregion");
        return hasRegions;
    }

    public bool WriteMethods(ICsCodeWriter writer, bool addEmptyLineBeforeRegion, IEnumerable<CsMethod> m,
        string region)
    {
        var csMethods = m as CsMethod[] ?? m.ToArray();
        if (!csMethods.Any()) return addEmptyLineBeforeRegion;
        writer.EmptyLine(!addEmptyLineBeforeRegion);
        
        addEmptyLineBeforeRegion = WriteMethodAction(writer, csMethods.OrderBy(a => a.Visibility).ThenBy(a => a.Name), region,
            i =>
            {
                var maker = new CsMethodWriter(i, _allowReferenceNullable);
                maker.MakeCode(writer, Class);
                writer.EmptyLine();
            }
        );
        return addEmptyLineBeforeRegion;
    }

    #region Properties

    public CsClass Class { get; }

    private CodeFormatting Features => Class.Formatting;

    #endregion
}
