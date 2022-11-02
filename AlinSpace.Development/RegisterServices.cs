using DryIoc;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace AlinSpace.Development
{
    public static class RegisterServices
    {
        public static void To(IContainer container)
        {
            container.Register<Files.IFilesService, Files.FilesService>(
                reuse: Reuse.Singleton,
                ifAlreadyRegistered: IfAlreadyRegistered.Replace);

            container.Register<Build.IBuildService, Build.BuildService>(
                reuse: Reuse.Singleton,
                ifAlreadyRegistered: IfAlreadyRegistered.Replace);

            container.Register<Nuget.INugetService, Nuget.NugetService>(
                reuse: Reuse.Singleton,
                ifAlreadyRegistered: IfAlreadyRegistered.Replace);

            container.Register<Setup.ISetupService, Setup.SetupService>(
                reuse: Reuse.Singleton,
                ifAlreadyRegistered: IfAlreadyRegistered.Replace);

            container.Register<Configuration.IConfigurationService, Configuration.ConfigurationService>(
                reuse: Reuse.Singleton,
                ifAlreadyRegistered: IfAlreadyRegistered.Replace);

            container.Register<Workflow.IWorkflowService, Workflow.WorkflowService>(
                reuse: Reuse.Singleton,
                ifAlreadyRegistered: IfAlreadyRegistered.Replace);

            container.RegisterDelegate<ILogger>(
                factoryDelegate: context =>
                {
                    var loggerConfiguration = new LoggerConfiguration();

                    loggerConfiguration.MinimumLevel.Debug();
                    loggerConfiguration.WriteTo.Console(theme: AnsiConsoleTheme.Code);

                    return loggerConfiguration.CreateLogger();
                },
                reuse: Reuse.Singleton,
                ifAlreadyRegistered: IfAlreadyRegistered.Replace);
        }
    }
}
