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

                UpdateProjectInformation(configuration, project);

                await BuildProjectAsync(configuration, project, incrementBuild.GetValueOrDefault());

                await CopyNupkgFileAsync(project, configuration);
                await CopyPdbFileAsync(project, configuration);
            });
        }

        /// <summary>
        /// Updates dependency versions of the project.
        /// </summary>
        void UpdateDependencyVersions(IProject project, IEnumerable<IProject> dependencies)
        {
            var projectDependencies = project.GetDependencies()
                .Where(x => dependencies.Any(x2 => x.Name == x2.Name));

            foreach (var projectDependency in projectDependencies)
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

        /// <summary>
        /// Builds project asynchornously.
        /// </summary>
        async Task BuildProjectAsync(Configuration.Configuration configuration, IProject project, bool incrementBuild)
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

        /// <summary>
        /// Updates project information.
        /// </summary>
        void UpdateProjectInformation(Configuration.Configuration configuration, IProject project)
        {
            var projectConfiguration = configuration.Projects.FirstOrDefault(x => x.Name == project.Name);

            if (projectConfiguration == null)
                return;

            logger.Information($"Updating project information ...");

            #region Authors

            var authors = configuration.Authors;

            if (string.IsNullOrWhiteSpace(authors))
            {
                authors = projectConfiguration.Authors;
            }

            if (!string.IsNullOrWhiteSpace(authors))
            {
                project.Authors = authors;
            }

            #endregion

            #region Copyright

            var copyright = configuration.Copyright;

            if (string.IsNullOrWhiteSpace(copyright))
            {
                copyright = projectConfiguration.Authors;
            }

            if (!string.IsNullOrWhiteSpace(copyright))
            {
                project.Copyright = copyright;
            }

            #endregion

            #region PackageTags

            var tags = $"{configuration.Tags?.Trim(' ', ';') ?? ""}, {projectConfiguration.Tags?.Trim(' ', ';') ?? ""}".Trim(' ', ';');

            if (!string.IsNullOrWhiteSpace(tags))
            {
                project.PackageTags = tags;
            }

            #endregion

            #region PackageProjectUrl

            var packageProjectUrl = configuration.PackageProjectUrl;

            if (string.IsNullOrWhiteSpace(packageProjectUrl))
            {
                packageProjectUrl = projectConfiguration.PackageProjectUrl;
            }

            if (!string.IsNullOrWhiteSpace(packageProjectUrl))
            {
                project.PackageProjectUrl = new Uri(packageProjectUrl);
            }

            #endregion

            #region RepositoryUrl

            var repositoryUrl = configuration.RepositoryUrl;

            if (string.IsNullOrWhiteSpace(repositoryUrl))
            {
                repositoryUrl = projectConfiguration.RepositoryUrl;
            }

            if (!string.IsNullOrWhiteSpace(repositoryUrl))
            {
                project.RepositoryUrl = new Uri(repositoryUrl);
            }

            #endregion

            project.Save();
        }

        /// <summary>
        /// Copy nupkg file asynchronously.
        /// </summary>
        Task CopyNupkgFileAsync(IProject project, Configuration.Configuration configuration)
        {
            var pathToNupkg = Path.Combine(
                Path.GetDirectoryName(project.PathToProjectFile) ?? throw new Exception(),
                "bin",
                "Release",
                $"{project.Name}.{project.Version}.nupkg");

            logger.Information($"{{NugetPackageFilePath}} ...", pathToNupkg);

            if (!File.Exists(pathToNupkg))
            {
                logger.Debug($"No nuget file produced.");
                return Task.CompletedTask;
            }

            File.Copy(pathToNupkg, Path.Combine(configuration.PathToLocalNugetFolder, $"{project.Name}.{project.Version}.nupkg"), true);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Copy PDB file asynchronously.
        /// </summary>
        Task CopyPdbFileAsync(IProject project, Configuration.Configuration configuration)
        {
            var pathToDebugFile = Path.Combine(
                Path.GetDirectoryName(project.PathToProjectFile) ?? throw new Exception(),
                "bin",
                "Release",
                "net6.0",
               $"{project.Name}.pdb");

            if (!File.Exists(pathToDebugFile))
            {
                logger.Warning($"Program database file not found: {{NugetPackageFilePath}}", pathToDebugFile);
                return Task.CompletedTask;
            }

            var pathToDebugFiles = PathHelper.MakeRoot(configuration.PathToDebugFiles ?? configuration.PathToLocalNugetFolder ?? "");

            if (string.IsNullOrWhiteSpace(pathToDebugFiles))
                throw new Exception();

            var pathToDebugFileDestination = Path.Combine(
                pathToDebugFiles,
                $"{project.Name}.pdb");

            if (!Directory.Exists(pathToDebugFiles))
                Directory.CreateDirectory(pathToDebugFiles);

            File.Copy(pathToDebugFile, pathToDebugFileDestination, true);

            logger.Information($"Copying program database file {{NugetPackageFilePath}} ...", pathToDebugFile);

            return Task.CompletedTask;
        }
    }
}
