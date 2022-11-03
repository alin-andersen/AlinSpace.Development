namespace AlinSpace.Development.Build
{
    /// <summary>
    /// Represents the build graph node.
    /// </summary>
    public class BuildGraphNode
    {
        /// <summary>
        /// Gets the dependencies.
        /// </summary>
        public IList<BuildGraphNode> Dependencies { get; } = new List<BuildGraphNode>();

        /// <summary>
        /// Gets the project.
        /// </summary>
        public IProject Project { get; }

        /// <summary>
        /// Gets or sets the flag indicating whether or not the project has been build.
        /// </summary>
        public bool Builded { get; set; }

        private BuildGraphNode(IProject project)
        {
            Project = project;
        }

        /// <summary>
        /// Creates new build graph node.
        /// </summary>
        /// <param name="project">Project.</param>
        /// <returns>Build graph node.</returns>
        public static BuildGraphNode New(IProject project)
        {
            return new BuildGraphNode(project);
        }
    }
}