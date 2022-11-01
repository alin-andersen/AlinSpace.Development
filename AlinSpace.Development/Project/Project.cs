namespace AlinSpace.Development
{
    /// <summary>
    /// Represents the project.
    /// </summary>
    public static class Project
    {
        /// <summary>
        /// Open the project file.
        /// </summary>
        /// <param name="pathToProjectFile">Path to the project file.</param>
        /// <returns>Project.</returns>
        public static IProject Open(string pathToProjectFile)
        {
            return ProjectInternal.Open(pathToProjectFile);
        }
    }
}
