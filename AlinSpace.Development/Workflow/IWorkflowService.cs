namespace AlinSpace.Development.Workflow
{
    public interface IWorkflowService
    {
        Task BuildAsync(string? workingDirectory = null, bool? incrementBuild = null);

        Task ShowConfigurationAsync(string? workingDirectory = null);
    }
}
