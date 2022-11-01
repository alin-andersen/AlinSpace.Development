namespace AlinSpace.Development
{
    /// <summary>
    /// Represents the project link interface.
    /// </summary>
    public interface IProjectLink
    {
        /// <summary>
        /// Gets the name of the project.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the absolute path to the project file.
        /// </summary>
        string PathToProjectFile { get; }
    }
}