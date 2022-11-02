using DryIoc;

namespace AlinSpace.Development
{
    public static class RootContainer
    {
        public static IContainer Instance { get; } = new Container();
    }
}
