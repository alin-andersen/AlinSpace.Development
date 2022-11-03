using DryIoc;

namespace AlinSpace.Development
{
    /// <summary>
    /// Represents the root container.
    /// </summary>
    public static class RootContainer
    {
        /// <summary>
        /// Gets the container instance;
        /// </summary>
        public static IContainer Instance { get; } = new Container();
    }
}
