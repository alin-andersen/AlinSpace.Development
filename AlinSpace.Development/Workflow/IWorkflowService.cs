namespace AlinSpace.Development.Workflow
{
    /// <summary>
    /// Represents the workflow service interface.
    /// </summary>
    public interface IWorkflowService
    {
        /// <summary>
        /// Builds asynchronously.
        /// </summary>
        /// <param name="workingDirectory">Working directory.</param>
        /// <param name="incrementBuild">Increment build.</param>
        Task BuildAsync(string? workingDirectory = null, bool? incrementBuild = null);

        /// <summary>
        /// Shows configuration asynchronously.
        /// </summary>
        /// <param name="workingDirectory">Working directory.</param>
        Task ShowConfigurationAsync(string? workingDirectory = null);
    }
}
