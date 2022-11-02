using CommandLine;
using DryIoc;
using Serilog;
using System.Reflection;

namespace AlinSpace.Development.Console
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try 
            {
                RegisterServices.To(RootContainer.Instance);

                var logger = RootContainer.Instance.Resolve<ILogger>();

                logger.Information($"AlinSpace.Development.Console (version {Assembly.GetExecutingAssembly().GetName().Version})");
                logger.Information($"");
                logger.Information($"-- BEGIN --");
                logger.Information($"");

                var types = Assembly
                    .GetExecutingAssembly()
                    .GetTypes()
                    .Where(x => x.GetCustomAttribute<VerbAttribute>() != null)
                    .OrderBy(x => x.Name)
                    .ToArray();

                await Parser
                    .Default
                    .ParseArguments(args, types)
                    .WithParsedAsync(RunAsync);

                logger.Information($"");
                logger.Information($"-- END --");
                logger.Information($"");
            }
            finally
            {
                RootContainer.Instance.Dispose();
                await Log.CloseAndFlushAsync();
            }
        }

        private static async Task RunAsync(object obj)
        {
            switch (obj)
            {
                case Commands.Build.Options options:
                    await new Commands.Build.Command().ExecuteAsync(options);
                    break;

                case Commands.Configuration.Options options:
                    await new Commands.Configuration.Command().ExecuteAsync(options);
                    break;
            }
        }
    }
}