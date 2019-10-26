namespace isukces.code.vssolutions
{
    public interface INuspec
    {
        string       Id             { get; }
        NugetVersion PackageVersion { get; }
    }
}