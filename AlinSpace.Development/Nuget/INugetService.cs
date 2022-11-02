namespace AlinSpace.Development.Nuget
{
    /// <summary>
    /// Represents the nuget service.
    /// </summary>
    public interface INugetService
    {
        /// <summary>
        /// Cleans the nuget cache asynchronously.
        /// </summary>
        Task CleanCacheAsync();
    }
}
