namespace AlinSpace.Development
{
    /// <summary>
    /// Represents the project interface.
    /// </summary>
    public interface IProject
    {
        /// <summary>
        /// Gets the absolute path to the project file.
        /// </summary>
        string PathToProjectFile { get; }

        /// <summary>
        /// Gets the name of the project.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the project type.
        /// </summary>
        ProjectType Type { get; }

        /// <summary>
        /// Gets or sets the flag indicating whether or not
        /// to generate a package on build.
        /// </summary>
        bool? GeneratePackageOnBuild { get; set; }

        #region Version

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        Version Version { get; set; }

        /// <summary>
        /// Increment the major number.
        /// </summary>
        /// <returns>Project.</returns>
        IProject VersionIncrementMajor();

        /// <summary>
        /// Increment the minor number.
        /// </summary>
        /// <returns>Project.</returns>
        IProject VersionIncrementMinor();

        /// <summary>
        /// Increment the build number.
        /// </summary>
        /// <returns>Project.</returns>
        IProject VersionIncrementBuild();

        #endregion

        #region Dependencies

        /// <summary>
        /// Add or update dependency.
        /// </summary>
        /// <param name="name">Name of the dependency.</param>
        /// <param name="version">Version.</param>
        /// <returns>Project.</returns>
        IProject AddOrUpdateDependency(string name, Version version);

        /// <summary>
        /// Get dependencies of the project.
        /// </summary>
        /// <returns>Enumerable of dependencies.</returns>
        IEnumerable<IDependency> GetDependencies();

        #endregion

        #region Authors

        /// <summary>
        /// Gets or sets the authors.
        /// </summary>
        string Authors { get; set; }

        #endregion

        #region Copyright

        /// <summary>
        /// Gets or sets the copyright.
        /// </summary>
        string Copyright { get; set; }

        #endregion

        #region PackageProjectUrl

        /// <summary>
        /// Gets or sets the package project URL.
        /// </summary>
        Uri PackageProjectUrl { get; set; }

        #endregion

        #region RepositoryUrl

        /// <summary>
        /// Gets or sets the repository URL.
        /// </summary>
        Uri RepositoryUrl { get; set; }

        #endregion

        #region PackageTags

        /// <summary>
        /// Gets or sets the package tags.
        /// </summary>
        string PackageTags { get; set; }

        #endregion

        /// <summary>
        /// Save the project.
        /// </summary>
        void Save();
    }
}