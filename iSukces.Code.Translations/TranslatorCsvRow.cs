using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using iSukces.Code.IO;

namespace iSukces.Code.Translations;

/// <summary>
/// </summary>
public sealed class TranslatorCsvRow : IComparable<TranslatorCsvRow>, IComparable
{
    public static TranslatorCsvRow? Create(List<string> cs)
    {
        while (cs.Count < 9)
            cs.Add(string.Empty);
        if (string.IsNullOrWhiteSpace(cs[1]))
            return null;

        return new TranslatorCsvRow
        {
            NeedToBeTranslated = string.Equals(cs[0], "yes", StringComparison.OrdinalIgnoreCase),
            Key                = cs[1],
            SourcePolish       = cs[2],
            SourceEnglish      = cs[3],
            Translation        = cs[4],
            Hint               = cs[5],
            TranslationComment = cs[6],
            NotLongerUsed      = cs[7] == NotLongerUsedText,
            Machine            = cs[8]
        };
    }

    public static TranslatorCsvRow? Deserialize(string text)
    {
        if (string.IsNullOrEmpty(text))
            return null;
        var cs = CsvHelper.DecodeLine(text, CsvSeparator);
        if (cs.Count < 2)
            return null;
        if (cs.Count < 3)
            return null;
        // throw new Exception("IncompleteLine");
        while (cs.Count < 9)
            cs.Add(string.Empty);

        cs[2] = TranslationTools.TranslationDecode(cs[2]);
        cs[3] = TranslationTools.TranslationDecode(cs[3]);
        cs[4] = TranslationTools.TranslationDecode(cs[4]);
        return Create(cs);
    }

    public static List<TranslatorCsvRow> Load(string fileName)
    {
        if (!File.Exists(fileName))
            return new List<TranslatorCsvRow>();
        var lines  = File.ReadLines(fileName);
        var result = new List<TranslatorCsvRow>();
        var cnt    = 0;
        foreach (var line in lines)
        {
            if (cnt++ == 0 && line.StartsWith("#", StringComparison.Ordinal))
                continue;
            var x = Deserialize(line);
            if (x != null)
                result.Add(x);
        }

        return result;
    }

    public static bool Save(string filename, List<TranslatorCsvRow>? items)
    {
        if (items == null || items.Count == 0)
            return CodeFileUtils.SaveIfDifferent("", filename);

        var header = Headers;

        var l = new List<string>(items.Count + 1)
        {
            CsvHelper.Encode(header, CsvSeparator) + "\r\n"
        };

        l.AddRange(items.Select(i => i.Serialize().TrimEnd(CsvSeparator) + "\r\n"));

        var content = string.Join("", l);
        return CodeFileUtils.SaveIfDifferent(content, filename);
    }

    public static void SortBeforeSave(List<TranslatorCsvRow> csvRows)
    {
        string GetKey(TranslatorCsvRow a)
        {
            var argSourceEnglish = a.SourcePolish + "-##-" + a.SourceEnglish;
            return argSourceEnglish;
        }

        var need = csvRows.Where(a => !a.NeedToBeTranslated)
            .Select(GetKey)
            .ToHashSet();

        csvRows.Sort((a, b) =>
        {
            {
                var aa = need.Contains(GetKey(a));
                var bb = need.Contains(GetKey(b));
                var c  = aa.CompareTo(bb);
                if (c != 0)
                    return c;
            }
            {
                var c = b.NeedToBeTranslated.CompareTo(a.NeedToBeTranslated);
                if (c != 0)
                    return c;
            }
            {
                var c = string.Compare(a.SourcePolish, b.SourcePolish, StringComparison.Ordinal);
                if (c != 0)
                    return c;
            }
            {
                var c = string.Compare((a.SourceEnglish), b.SourceEnglish, StringComparison.Ordinal);
                if (c != 0)
                    return c;
            }
            return string.Compare(a.Key, b.Key, StringComparison.Ordinal);
        });
    }

    public int CompareTo(TranslatorCsvRow? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        var needToBeTranslatedComparison = NeedToBeTranslated.CompareTo(other.NeedToBeTranslated);
        if (needToBeTranslatedComparison != 0) return -needToBeTranslatedComparison;
        return string.Compare(Key, other.Key, StringComparison.Ordinal);
    }

    public int CompareTo(object? obj)
    {
        if (ReferenceEquals(null, obj)) return 1;
        if (ReferenceEquals(this, obj)) return 0;
        return obj is TranslatorCsvRow other
            ? CompareTo(other)
            : throw new ArgumentException($"Object must be of type {nameof(TranslatorCsvRow)}");
    }

    public string[] GetColumnsValues()
    {
        var fields = new[]
        {
            NeedToBeTranslated ? "yes" : "",
            Key,
            TranslationTools.TranslationEncode(SourcePolish),
            TranslationTools.TranslationEncode(SourceEnglish),
            TranslationTools.TranslationEncode(Translation),
            Hint ?? "",
            TranslationComment ?? "",
            NotLongerUsed ? NotLongerUsedText : "",
            Machine ?? ""
        };
        return fields;
    }

    private string Serialize()
    {
        var fields = GetColumnsValues();
        return CsvHelper.Encode(fields, CsvSeparator);
    }

    public override string ToString() => SourcePolish + " => " + Translation;

    #region Properties

    public static string[] Headers
    {
        get
        {
            var header = new[]
            {
                "#translate",
                "#key",
                "#polish source",
                "#english source",
                "#translation",
                "#hint for translator",
                "#comment from translator",
                "#still required",
                "#machine"
            };
            return header;
        }
    }


    public string Key
    {
        get => _key ?? "";
        set
        {
            if (value is null)
                throw new Exception("Key is empty");
            value = value.Trim();
            if (string.IsNullOrEmpty(value))
                throw new Exception("Key is empty");
            _key = value;
        }
    }

    public string SourcePolish
    {
        get => _sourcePolish ?? "";
        set
        {
            if (value == "[Choose one]")
                throw new Exception("To nie wygląda na polski język");
            _sourcePolish = value;
        }
    }

    public string SourceEnglish
    {
        get => _sourceEnglish ?? "";
        set
        {
            // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
            value ??= "";
            if (value.IndexOf("\\\\", StringComparison.Ordinal) >= 0)
                Debug.Write("");
            _sourceEnglish = value;
        }
    }

    public string Translation
    {
        get => _translation ?? "";
        set
        {
            // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
            value ??= "";
            if (value.IndexOf("\\\\", StringComparison.Ordinal) >= 0)
                Debug.Write("");
            _translation = value;
        }
    }

    public string? Hint { get; set; }
    public bool    NotLongerUsed { get; set; }
    public string? Machine       { get; set; }

    /// <summary>
    ///     Wskazówka dla tłumacza
    /// </summary>
    /// <summary>
    ///     Komentarz od tłumacza
    /// </summary>
    public string? TranslationComment { get; set; }

    public bool NeedToBeTranslated { get; set; }

    #endregion

    #region Fields

    private const string NotLongerUsedText = "not longer used";
    private const char CsvSeparator = ';';
    private string? _sourcePolish;
    private string? _key;
    private string? _sourceEnglish;
    private string? _translation;

    #endregion
}
