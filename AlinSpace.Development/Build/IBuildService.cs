namespace AlinSpace.Development.Build
{
    public interface IBuildService
    {
        Task BuildSolutionAsync(Configuration.Configuration configuration, bool? incrementBuild = null);
    }
}
