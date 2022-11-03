using System.Diagnostics;

namespace AlinSpace.Development.Cli
{
    /// <summary>
    /// Represents the command line interface.
    /// </summary>
    public static class CommandLineInterface
    {
        /// <summary>
        /// Execute asynchronously.
        /// </summary>
        /// <param name="command">Command.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public static async Task ExecuteAsync(string command, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(command))
                throw new Exception();

            var executable = command.Split(" ").FirstOrDefault();
            var parameters = command.Substring(executable.Length);

            var processStartInfo = new ProcessStartInfo(executable, parameters)
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
