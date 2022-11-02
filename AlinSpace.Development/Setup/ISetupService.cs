namespace AlinSpace.Development.Setup
{
    public interface ISetupService
    {
        Task SetupAsync(Configuration.Configuration configuration);
    }
}
