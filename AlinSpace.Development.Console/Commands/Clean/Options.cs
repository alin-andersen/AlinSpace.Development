using CommandLine;

namespace AlinSpace.Development.Console.Commands.Clean
{
    [Verb("clean", HelpText = "Clean all local files (nuget packages, debug files, ...).")]
    public class Options
    {
        [Option('a', "all", HelpText = "Clean all files (not just from this solution).")]
        public bool? All { get; set; }
    }
}
