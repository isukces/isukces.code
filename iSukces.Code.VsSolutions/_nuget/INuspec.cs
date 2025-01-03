namespace iSukces.Code.VsSolutions;

public interface INuspec
{
    string       Id             { get; }
    NugetVersion PackageVersion { get; }
}
    
public interface INuspec2:INuspec
{
    string TargetFramework { get; }
}