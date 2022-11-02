using AlinSpace.Development.Build;
using AlinSpace.Development.Configuration;
using Serilog;

namespace AlinSpace.Development.Workflow
{
    public class WorkflowService : IWorkflowService
    {
        private readonly ILogger logger;
        private readonly IConfigurationService configurationService;
        private readonly IBuildService buildService;

        public WorkflowService(
            ILogger logger,
            IConfigurationService configurationService,
            IBuildService buildService)
        {
            this.logger = logger;
            this.configurationService = configurationService;
            this.buildService = buildService;
        }

        public async Task BuildAsync(string? workingDirectory = null, bool? incrementBuild = false)
        {
            logger.Information($"Starting build workflow ...");
            logger.Information($"IncrementBuild = {incrementBuild ?? false}");

            #region Get or create default configuration

            var configuration = await configurationService.GetOrDefaultAsync(workingDirectory);

            if (configuration == null)
            {
                logger.Warning("Configuration file not found.");
                await configurationService.CreateDefaultAsync(workingDirectory);
                logger.Warning("Please setup the configuration file, then run command again.");
                return;
            }

            #endregion

            logger.Information($"");

            await buildService.BuildSolutionAsync(configuration, incrementBuild);
        }

        public async Task ShowConfigurationAsync(string? workingDirectory = null)
        {
            logger.Information($"Starting build workflow ...");

            #region Get or create default configuration

            var configurationLines = await configurationService.ShowConfigurationAsync(workingDirectory);

            if (configurationLines == null)
            {
                logger.Warning("Configuration file not found.");
                return;
            }

            #endregion

            logger.Information($"");

            foreach (var configurationLine in configurationLines)
            {
                logger.Information(configurationLine);
            }

            logger.Information($"");
        }
    }
}
