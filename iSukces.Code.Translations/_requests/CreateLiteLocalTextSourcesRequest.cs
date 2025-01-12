using System;
using System.Linq;
using System.Text;

namespace iSukces.Code.Translations;

/// <summary>
///     Żądanie dla autokodu, aby stworzył pole typu LiteLocalTextSource
/// </summary>
public sealed class CreateLiteLocalTextSourcesRequest
{
    public CreateLiteLocalTextSourcesRequest(string key, string originalText, string? hint = null)
    {
        Key             = Camelize(key.Trim());
        FieldName       = DefaultFieldName(Key);
        OriginalText    = originalText.Trim();
        TranslationHint = hint;
    }

    public static string DefaultFieldName(string key)
    {
        return "Str" + key.Split('.').Last();
    }

    public static implicit operator CreateLiteLocalTextSourcesRequest(string x)
    {
        var parts = x.Split('|');
        if (parts.Length < 2)
            throw new Exception("Invalid text");
        return new CreateLiteLocalTextSourcesRequest(parts[0], parts[1], parts.Length > 2 ? parts[2] : null);
    }

    private static string Camelize(string key)
    {
        var sb    = new StringBuilder(key.Length);
        var upper = true;
        foreach (var i in key)
        {
            if (i == ' ')
            {
                upper = true;
                continue;
            }

            sb.Append(upper ? char.ToUpper(i) : i);
            upper = false;
        }

        return sb.ToString();
    }

    public string  FieldName       { get; set; }
    public string  Key             { get; }
    public string? TranslationHint { get; set; }
    public string  OriginalText    { get; set; }
}