using Serilog;

namespace AlinSpace.Development.Files
{
    public class FilesService : IFilesService
    {
        private readonly ILogger logger;

        public FilesService(ILogger logger)
        {
            this.logger = logger;
        }

        public void DeleteFolderNamesRecursively(string path, IEnumerable<string> folderNames)
        {
            var folders = GetFoldersToDelete(path, folderNames);

            foreach (var folder in folders)
            {
                try
                {
                    Directory.Delete(folder, true);
                }
                catch
                {
                    // ignore
                }
            }
        }

        public void DeleteFolderNamesRecursively(string path, params string[] folderNames)
        {
            DeleteFolderNamesRecursively(path, folderNames);
        }

        IEnumerable<string> GetFoldersToDelete(string path, IEnumerable<string> folderNames)
        {
            path = PathHelper.MakeRoot(path);

            if (!Directory.Exists(path))
                return Enumerable.Empty<string>();

            var foldersToDelete = new List<string>();

            var folderNamesSet = new HashSet<string>(folderNames);

            var foldersToProcess = new Queue<string>(Directory.GetDirectories(path));

            // Continue until no more folders to process are available.
            while(foldersToProcess.Any())
            {
                var folderToProcess = foldersToProcess.Dequeue();

                var folderName = Path.GetDirectoryName(folderToProcess);

                if (string.IsNullOrWhiteSpace(folderName))
                    continue;

                // If the folder name is not in the set,
                // then add all its sub folders to be processed.

                if (!folderNamesSet.Contains(folderName))
                {
                    foreach(var subFolder in Directory.GetDirectories(folderToProcess))
                    {
                        foldersToProcess.Enqueue(subFolder);
                    }

                    continue;
                }
             
                foldersToDelete.Add(folderToProcess);
            }

            return foldersToDelete;
        }

        public void DeleteFolderBinAndObjRecursively(string path)
        {
            DeleteFolderNamesRecursively(path, "bin", "obj");
        }
    }
}
