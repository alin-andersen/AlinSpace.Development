namespace AlinSpace.Development.Build
{
    /// <summary>
    /// Represents the build service interface.
    /// </summary>
    public interface IBuildService
    {
        /// <summary>
        /// Builds the solution asynchronously.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        /// <param name="incrementBuild">Increment build number.</param>
        Task BuildSolutionAsync(Configuration.Configuration configuration, bool? incrementBuild = null);
    }
}
