using System.Diagnostics;

namespace AlinSpace.Development.Cli
{
    public static class CommandLineInterface
    {
        public static async Task ExecuteAsync(string command, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(command))
                throw new Exception();

            var command2 = command.Split(" ").FirstOrDefault();
            var parameters = command.Substring(command2.Length);

            var processStartInfo = new ProcessStartInfo(command2, parameters)
            {
                
            };

            var process = Process.Start(processStartInfo);

            if (process == null)
                throw new Exception("Unable to start process.");

            while (!process.HasExited)
                await process.WaitForExitAsync(cancellationToken);

            if (process.ExitCode != 0)
                throw new Exception($"Process exited with {process.ExitCode}");
        }
    }
}
