namespace AlinSpace.Development
{
    /// <summary>
    /// Represents the nuget service.
    /// </summary>
    public interface INuget
    {
        /// <summary>
        /// Cleans the nuget cache asynchronously.
        /// </summary>
        Task CleanCacheAsync();
    }
}
