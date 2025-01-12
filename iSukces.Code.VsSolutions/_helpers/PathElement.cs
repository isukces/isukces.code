namespace iSukces.Code.VsSolutions;

internal class PathElement
{
    public static PathElement? FromString(string? s)
    {
        s = s?.Trim();
        if (string.IsNullOrEmpty(s))
            return null;
        return new PathElement
        {
            ElementName = s
        };
    }

    public required string ElementName { get; init; }
}
