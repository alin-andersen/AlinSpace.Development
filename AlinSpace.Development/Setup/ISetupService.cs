namespace AlinSpace.Development.Setup
{
    /// <summary>
    /// Represents the setup service interface.
    /// </summary>
    public interface ISetupService
    {
        /// <summary>
        /// Setups asynchronously.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        Task SetupAsync(Configuration.Configuration configuration);
    }
}
