namespace AlinSpace.Development
{
    public class Nuget : INuget
    {
        public Nuget Instance { get; } = new Nuget();

        public Task CleanCacheAsync()
        {
            throw new NotImplementedException();
        }
    }
}
