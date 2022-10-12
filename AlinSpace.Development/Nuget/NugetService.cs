namespace AlinSpace.Development.Nuget
{
    public class NugetService : INugetService
    {
        public NugetService Instance { get; } = new NugetService();

        public Task CleanCacheAsync()
        {
            throw new NotImplementedException();
        }
    }
}
