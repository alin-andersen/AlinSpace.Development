using Serilog;

namespace AlinSpace.Development.Setup
{
    public class SetupService : ISetupService
    {
        private readonly ILogger logger;

        public SetupService(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task SetupAsync(Configuration.Configuration configuration)
        {

        }
    }
}
