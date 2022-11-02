using Serilog;

namespace AlinSpace.Development.Build
{
    public class BuildService : IBuildService
    {
        private readonly ILogger logger;

        public BuildService(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task BuildSolutionAsync(Configuration.Configuration configuration, bool? incrementBuild = null)
        {
            logger.Information("Building solution ...");
            logger.Information("Creating build graph ...");

            if (string.IsNullOrWhiteSpace(configuration.PathToSolutionFile))
                throw new Exception("Path to solution not set.");

            var solution = Solution.Read(configuration.PathToSolutionFile);

            var buildGraph = await BuildGraph.NewAsync(logger, solution);

            await buildGraph.BuildAsync(async (project, dependencies) =>
            {
                if (incrementBuild.HasValue && incrementBuild.Value)
                {
                    UpdateDependencyVersions(project, dependencies);
                }

                if (!project.GeneratePackageOnBuild ?? false)
                {
                    logger.Debug($"Setting GeneratePackageOnBuild to true ...");

                    project.GeneratePackageOnBuild = true;
                    project.Save();
                }

                await BuildProjectAsync(project, configuration, incrementBuild.GetValueOrDefault());

                await CopyNupkgFileAsync(project, configuration);
                await CopyPdbFileAsync(project, configuration);
            });
        }

        void UpdateDependencyVersions(IProject project, IEnumerable<IProject> dependencies)
        {
            var projectDependencies = project.GetDependencies()
                .Where(x => dependencies.Any(x2 => x.Name == x2.Name));

            foreach(var projectDependency in projectDependencies)
            {
                var dependency = dependencies.FirstOrDefault(x => x.Name == projectDependency.Name);

                if (dependency == null)
                {
                    logger.Error($"Unable to find dependency: {{DependencyName}}", projectDependency.Name);
                    throw new Exception($"Unable to find dependency: {projectDependency.Name}");
                }

                logger.Debug(
                    $"Updating version of dependency {{DependencyName}} from {{VersionFrom}} to {{VersionTo}} ...", 
                    projectDependency.Name, 
                    projectDependency.Version, 
                    dependency.Version);

                projectDependency.Version = dependency.Version;
            }

            project.Save();
        }

        private async Task BuildProjectAsync(IProject project, Configuration.Configuration configuration, bool incrementBuild)
        {
            if (incrementBuild)
            {
                project.VersionIncrementBuild();
                project.Save();
            }

            logger.Debug($"Building project {{ProjectName}} ...", project.Name);

            await Cli.CommandLineInterface.ExecuteAsync($"dotnet build {project.PathToProjectFile} -c Release");

            logger.Debug($"Packing project {{ProjectName}} ...", project.Name);

            await Cli.CommandLineInterface.ExecuteAsync($"dotnet pack {project.PathToProjectFile} -c Release");
        }

        private async Task CopyNupkgFileAsync(IProject project, Configuration.Configuration configuration)
        {
            var pathToNupkg = Path.Combine(
                Path.GetDirectoryName(project.PathToProjectFile),
                "bin",
                "Release",
                $"{project.Name}.{project.Version}.nupkg");

            logger.Information($"{{NugetPackageFilePath}} ...", pathToNupkg);

            if (!File.Exists(pathToNupkg))
            {
                logger.Debug($"No nuget file produced.");
                return;
            }

            File.Copy(pathToNupkg, Path.Combine(configuration.PathToLocalNugetFolder, $"{project.Name}.{project.Version}.nupkg"), true);
        }

        private async Task CopyPdbFileAsync(IProject project, Configuration.Configuration configuration)
        {
            var pathToDebugFile = Path.Combine(
                Path.GetDirectoryName(project.PathToProjectFile),
                "bin",
                "Release",
                "net6.0",
               $"{project.Name}.pdb");

            if (!File.Exists(pathToDebugFile))
            {
                logger.Warning($"Program database file not found: {{NugetPackageFilePath}}", pathToDebugFile);
                return;
            }

            var pathToDebugFiles = PathHelper.MakeRoot(configuration.PathToDebugFiles ?? configuration.PathToLocalNugetFolder);

            var pathToDebugFileDestination = Path.Combine(
                pathToDebugFiles,
                $"{project.Name}.pdb");

            if (!Directory.Exists(pathToDebugFiles))
                Directory.CreateDirectory(pathToDebugFiles);

            File.Copy(pathToDebugFile, pathToDebugFileDestination, true);

            logger.Information($"Copying program database file {{NugetPackageFilePath}} ...", pathToDebugFile);
        }
    }
}
