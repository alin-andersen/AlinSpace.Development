using Serilog;

namespace AlinSpace.Development.Build
{
    /// <summary>
    /// Represents the build graph.
    /// </summary>
    public class BuildGraph
    {
        private ILogger logger;
        private ISolution solution;

        private readonly IDictionary<string, BuildGraphNode> map = new Dictionary<string, BuildGraphNode>();

        /// <summary>
        /// Connstructor.
        /// </summary>
        private BuildGraph(ILogger logger, ISolution solution) 
        {
            this.logger = logger;
            this.solution = solution;
        }

        /// <summary>
        /// Creates new build graph asynchronously.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="solution">Solution.</param>
        /// <returns>Build graph.</returns>
        public static async Task<BuildGraph> NewAsync(ILogger logger, ISolution solution)
        {
            var buildGraph = new BuildGraph(logger, solution);

            await buildGraph.SetupAsync();

            return buildGraph;
        }

        Task SetupAsync()
        {
            var projects = solution.Projects.Select(x => Project.Open(x.PathToProjectFile)).ToList();

            var queue = new Queue<IProject>();

            foreach (var project in projects)
            {
                var buildGraphNode = BuildGraphNode.New(project);

                map.Add(project.Name, buildGraphNode);

                queue.Enqueue(project);
            }

            while(queue.Any())
            {
                var project = queue.Dequeue();
                var currentNode = map[project.Name];

                logger.Debug($"Current node: {{ProjectName}}", currentNode.Project.Name);

                foreach (var dependency in project.GetDependencies().Where(x => map.Values.Any(x2 => x2.Project.Name == x.Name)))
                {
                    if (!map.TryGetValue(dependency.Name, out var node))
                        continue;

                    logger.Debug($" - Link dependency: {{ProjectName}}", node.Project.Name);

                    currentNode.Dependencies.Add(node);
                }
            }

            return Task.CompletedTask;
        }

        public async Task BuildAsync(Func<IProject, IEnumerable<IProject>, Task> buildAction)
        {
            var queue = new Queue<BuildGraphNode>(map.Values);

            while (queue.Any())
            {
                var node = queue.Dequeue();

                logger.Debug($"Current item {{ProjectName}} ...", node.Project.Name);

                var unbuildDependencies = node.Dependencies
                    .Where(x => map.Values.Any(x2 => x2.Project.Name == x.Project.Name))
                    .Where(x => !x.Builded)
                    .ToList();

                if (unbuildDependencies.Any())
                {
                    logger.Debug($"Unbuild dependencies:");

                    foreach (var unbuildDependency in unbuildDependencies)
                    {
                        logger.Debug($" - {{ProjectName}} unbuild.", unbuildDependency.Project.Name);
                    }
                }

                if (unbuildDependencies.Empty())
                {
                    await buildAction(node.Project, node.Dependencies.Select(x => x.Project).ToList());
                    node.Builded = true;
                }
                else
                {
                    logger.Debug($"Requeue unbuild project {{ProjectName}} ...", node.Project.Name);
                    queue.Enqueue(node);
                }
            }
        }
    }
}
