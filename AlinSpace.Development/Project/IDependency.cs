namespace AlinSpace.Development
{
    /// <summary>
    /// Represents the project dependency interface.
    /// </summary>
    public interface IDependency
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public Version Version { get; set; }

        /// <summary>
        /// Removes the dependency.
        /// </summary>
        void Remove();
    }
}
