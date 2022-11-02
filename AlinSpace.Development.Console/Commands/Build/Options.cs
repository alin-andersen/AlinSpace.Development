using CommandLine;

namespace AlinSpace.Development.Console.Commands.Build
{
    [Verb("build", HelpText = "Build solution.")]
    public class Options
    {
        [Option('p', "project", HelpText = "Only build the given project.")]
        public string? Project { get; set; }

        [Option('c', "configuration", Required = false, Default = "release", HelpText = "Set the configuration (e.g. release, debug, ...).")]
        public string? Configuration { get; set; }

        [Option('i', "increment", Required = false, HelpText = "Increment build number of all projects.")]
        public bool? Increment { get; set; }
    }
}
