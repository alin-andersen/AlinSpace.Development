using Serilog;

namespace AlinSpace.Development.Nuget
{
    public class NugetService : INugetService
    {
        private readonly ILogger logger;

        public NugetService(ILogger logger)
        {
            this.logger = logger;
        }

        public Task CleanCacheAsync()
        {
            throw new NotImplementedException();
        }
    }
}
