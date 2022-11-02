namespace AlinSpace.Development.Configuration
{
    /// <summary>
    /// Represents the configuration service interface.
    /// </summary>
    public interface IConfigurationService
    {
        /// <summary>
        /// Creates the default configuration asynchronously.
        /// </summary>
        /// <param name="workingDirectory">Working directory.</param>
        /// <param name="defaultConfiguration">Default configuration.</param>
        /// <returns>Configuration.</returns>
        Task<Configuration> CreateDefaultAsync(string? workingDirectory = null, Configuration? defaultConfiguration = null);

        /// <summary>
        /// Gets configuration or default asynchronously.
        /// </summary>
        /// <param name="workingDirectory">Working directory.</param>
        /// <returns>Configuration or default.</returns>
        Task<Configuration?> GetOrDefaultAsync(string? workingDirectory = null);

        /// <summary>
        /// Shows the configuration file asynchronously.
        /// </summary>
        /// <param name="workingDirectory">Working directory.</param>
        /// <returns>Configuration content.</returns>
        Task<string[]?> ShowConfigurationAsync(string? workingDirectory = null);

        /// <summary>
        /// Gets or creates default configuration asynchronously.
        /// </summary>
        /// <param name="workingDirectory">Working directory.</param>
        /// <param name="defaultConfiguration">Default configuration.</param>
        /// <returns>Configuration.</returns>
        Task<Configuration> GetOrCreateDefaultAsync(string? workingDirectory = null, Configuration? defaultConfiguration = null);

        /// <summary>
        /// Deletes the configuration asynchronously.
        /// </summary>
        /// <param name="workingDirectory">Working directory.</param>
        Task DeleteAsync(string? workingDirectory = null);
    }
}
