namespace AlinSpace.Development
{
    /// <summary>
    /// Represents the implementation of <see cref="IProjectLink"/>.
    /// </summary>
    internal class ProjectLinkInternal : IProjectLink
    {
        public string Name { get; }

        public string PathToProjectFile { get; }

        public ProjectLinkInternal(
            string name,
            string pathToProjectFile)
        {
            Name = name;
            PathToProjectFile = pathToProjectFile;
        }
    }
}
