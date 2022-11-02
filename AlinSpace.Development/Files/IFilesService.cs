namespace AlinSpace.Development.Files
{
    /// <summary>
    /// Represents the files service.
    /// </summary>
    public interface IFilesService
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

        /// <summary>
        /// Deletes the bin and obj folders recurively.
        /// </summary>
        /// <param name="path">Path.</param>
        void DeleteFolderBinAndObjRecursively(string path);
    }
}
