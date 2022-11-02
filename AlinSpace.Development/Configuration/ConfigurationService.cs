using Serilog;
using System.Text.Json;

namespace AlinSpace.Development.Configuration
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly ILogger logger;

        public ConfigurationService(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task<Configuration> CreateDefaultAsync(string? workingDirectory = null, Configuration? defaultConfiguration = null)
        {
            defaultConfiguration ??= Defaults.Default;

            var filePath = PrepareConfigurationFilePath(workingDirectory);

            var jsonText = JsonSerializer.Serialize(
                value: defaultConfiguration,
                options: new JsonSerializerOptions()
                {
                    WriteIndented = true,
                });

            logger.Information($"Creating default configuration file: {{FilePath}}", filePath);

            await File.WriteAllTextAsync(filePath, jsonText);

            return defaultConfiguration;
        }

        public async Task<Configuration?> GetOrDefaultAsync(string? workingDirectory = null)
        {
            var filePath = PrepareConfigurationFilePath(workingDirectory);

            logger.Information($"Reading configuration file: {{FilePath}}", filePath);

            var jsonText = await File.ReadAllTextAsync(filePath);

            var configuration = JsonSerializer.Deserialize<Configuration>(jsonText);

            return configuration;
        }

        public async Task<string[]?> ShowConfigurationAsync(string? workingDirectory = null)
        {
            var filePath = PrepareConfigurationFilePath(workingDirectory);

            logger.Information($"Reading configuration file: {{FilePath}}", filePath);

            return await File.ReadAllLinesAsync(filePath);
        }

        public async Task<Configuration> GetOrCreateDefaultAsync(string? workingDirectory = null, Configuration? defaultConfiguration = null)
        {
            var configuration = await GetOrDefaultAsync(workingDirectory);

            if (configuration == null)
            {
                logger.Warning($"Configuration file not found.");

                configuration = await CreateDefaultAsync(workingDirectory, defaultConfiguration);
            }

            return configuration;
        }

        public Task DeleteAsync(string? workingDirectory = null)
        {
            var filePath = PrepareConfigurationFilePath(workingDirectory);

            if (!File.Exists(filePath))
                return Task.CompletedTask;

            logger.Information($"Deleting configuration file: {{FilePath}}", filePath);

            File.Delete(filePath);

            return Task.CompletedTask;
        }

        #region Helper

        string PrepareWorkingDirectory(string? workingDirectory)
        {
            if (string.IsNullOrWhiteSpace(workingDirectory))
                return Directory.GetCurrentDirectory();

            workingDirectory = PathHelper.MakeRoot(workingDirectory);

            if (!Directory.Exists(workingDirectory))
                throw new Exception("Working directory is not a directory.");

            logger.Information($"Working directory: {{Path}}", workingDirectory);

            return workingDirectory;
        }

        string PrepareConfigurationFilePath(string? workingDirectory)
        {
            return Path.Combine(PrepareWorkingDirectory(workingDirectory), Constants.DefaultConfigurationFileName);
        }

        #endregion
    }
}
