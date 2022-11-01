namespace AlinSpace.Development
{
    /// <summary>
    /// Represents the bin-and-obj folder deletion.
    /// </summary>
    public interface IFilesystemCleanup
    {
        /// <summary>
        /// Deletes folder names recursively.
        /// </summary>
        /// <param name="path">Path.</param>
        /// <param name="folderNames">Folder names to delete.</param>
        void DeleteFolderNamesRecursively(string path, IEnumerable<string> folderNames);

        /// <summary>
        /// Deletes folder names recursively.
        /// </summary>
        /// <param name="path">Path.</param>
        /// <param name="folderNames">Folder names to delete.</param>
        void DeleteFolderNamesRecursively(string path, params string[] folderNames);
    }
}
