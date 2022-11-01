using System.Collections.Generic;

namespace AlinSpace.Development
{
    /// <summary>
    /// Represents the solution interface.
    /// </summary>
    public interface ISolution
    {
        /// <summary>
        /// Gets the solution name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the projects.
        /// </summary>
        IEnumerable<IProjectLink> Projects { get; }
    }
}
