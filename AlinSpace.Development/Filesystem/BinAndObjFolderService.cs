namespace AlinSpace.Development.Filesystem
{
    public class BinAndObjFolderService : IBinAndObjFolderService
    {
        public static BinAndObjFolderService Instance { get; } = new BinAndObjFolderService();

        public void DeleteRecursive(string path)
        {
            var directories = GetRecursiveDirectories(path, "bin", "obj");

            foreach (var directory in directories)
            {
                var projectFiles = Directory.GetFiles(directory, "*.csproj");

                if (projectFiles.Length == 0)
                    continue;

                var binFolder = Path.Combine(directory, "bin");

                if (Directory.Exists(binFolder))
                {
                    try
                    {
                        Directory.Delete(binFolder, true);
                    }
                    catch
                    {
                        // ignore
                    }
                }

                var objFolder = Path.Combine(directory, "obj");

                if (Directory.Exists(objFolder))
                {
                    try
                    {
                        Directory.Delete(objFolder, true);
                    }
                    catch
                    {
                        // ignore
                    }
                }
            }
        }

        IEnumerable<string> GetRecursiveDirectories(string path, params string[] folderNameToStop)
        {
            path = PathHelper.MakeRoot(path);

            var store = new List<string>();

            if (!Directory.Exists(path))
                return store;

            store.Add(path);

            GetRecursiveDirectories(store, path, folderNameToStop);

            return store;
        }

        void GetRecursiveDirectories(IList<string> store, string pathToSearch, string[] folderNameToStop)
        {
            var paths = Directory.GetDirectories(pathToSearch);

            foreach(var path in paths)
            {
                store.Add(path);

                var folderName = Path.GetDirectoryName(path);

                if (folderNameToStop.Any(x => x == folderName))
                    continue;

                GetRecursiveDirectories(paths, path, folderNameToStop);
            }
        }
    }
}
