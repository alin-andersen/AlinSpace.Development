//using AlinSpace.ProjectManipulator;
//using System;
//using System.Linq;

//namespace AlinSpace.Tools.Development.Commands.Update
//{
//    /// <summary>
//    /// Represents the project update info command.
//    /// </summary>
//    /// <remarks>
//    /// This command applies all project information from the 
//    /// configuration file.
//    /// </remarks>
//    public class Command
//    {
//        public void Execute(Context context, Options options)
//        {
//            var solution = Solution.Read(context.Configuration.PathToSolutionFile);

//            if (options.Project == null)
//            {
//                foreach (var project in solution.Projects)
//                {
//                    var projectConfiguration = context
//                        .Configuration
//                        .Projects
//                        .Where(x => !context.Configuration.IgnoredProjects.Any(x2 => x2 == x.Name))
//                        .FirstOrDefault(x => x.Name == project.Name);

//                    if (projectConfiguration == null)
//                    {
//                        ConsolePrinter.WriteLine($"✗ No project configuration found for project {projectConfiguration.Name}.", ConsoleColor.Red);
//                        ConsolePrinter.WriteLine($"✗ Skipping ...", ConsoleColor.Red);
//                        continue;
//                    }

//                    UpdateProjectInfo(context, project, projectConfiguration);
//                }
//            }
//            else
//            {
//                var projectConfiguration = context
//                    .Configuration
//                    .Projects
//                    .FirstOrDefault(x => x.Name == options.Project);

//                if (projectConfiguration == null)
//                {
//                    ConsolePrinter.WriteLine($"✗ No project configuration found for project {projectConfiguration.Name}.", ConsoleColor.Red);
//                    ConsolePrinter.WriteLine($"✗ Skipping ...", ConsoleColor.Red);
//                    return;
//                }

//                var project = solution.Projects.FirstOrDefault(x => x.Name == options.Project);

//                if (project == null)
//                {
//                    ConsolePrinter.WriteLine($"✗ No project configuration found for project {projectConfiguration.Name}.", ConsoleColor.Red);
//                    ConsolePrinter.WriteLine($"✗ Skipping ...", ConsoleColor.Red);
//                    return;
//                }

//                UpdateProjectInfo(context, project, projectConfiguration);
//            }
//        }

//        void UpdateProjectInfo(Context context, IProjectLink projectLink, ProjectConfiguration projectConfiguration)
//        {
//            var project = Project.Open(projectLink.PathToProjectFile);

//            ConsolePrinter.WriteLine($"⟳ Updating project information for {project.Name} ...", ConsoleColor.Yellow);

//            #region Authors

//            var authors = context.Configuration.Authors;

//            if (string.IsNullOrWhiteSpace(authors))
//            {
//                authors = projectConfiguration.Authors;
//            }

//            if (!string.IsNullOrWhiteSpace(authors))
//            {
//                project.Authors = authors;
//            }

//            #endregion

//            #region Copyright

//            var copyright = context.Configuration.Copyright;

//            if (string.IsNullOrWhiteSpace(copyright))
//            {
//                copyright = projectConfiguration.Authors;
//            }

//            if (!string.IsNullOrWhiteSpace(copyright))
//            {
//                project.Copyright = copyright;
//            }

//            #endregion

//            #region PackageTags

//            var tags = $"{context.Configuration.Tags?.Trim(' ', ';') ?? ""}, {projectConfiguration.Tags?.Trim(' ', ';') ?? ""}".Trim(' ', ';');

//            if (!string.IsNullOrWhiteSpace(tags))
//            {
//                project.PackageTags = tags;
//            }

//            #endregion

//            #region PackageProjectUrl

//            var packageProjectUrl = context.Configuration.PackageProjectUrl;

//            if (string.IsNullOrWhiteSpace(packageProjectUrl))
//            {
//                packageProjectUrl = projectConfiguration.PackageProjectUrl;
//            }

//            if (!string.IsNullOrWhiteSpace(packageProjectUrl))
//            {
//                project.PackageProjectUrl = new Uri(packageProjectUrl);
//            }

//            #endregion

//            #region RepositoryUrl

//            var repositoryUrl = context.Configuration.RepositoryUrl;

//            if (string.IsNullOrWhiteSpace(repositoryUrl))
//            {
//                repositoryUrl = projectConfiguration.RepositoryUrl;
//            }

//            if (!string.IsNullOrWhiteSpace(repositoryUrl))
//            {
//                project.RepositoryUrl = new Uri(repositoryUrl);
//            }

//            #endregion

//            project.Save();

//            ConsolePrinter.WriteLine($"✓ Updated project information.", ConsoleColor.Green);
//        }
//    }
//}
