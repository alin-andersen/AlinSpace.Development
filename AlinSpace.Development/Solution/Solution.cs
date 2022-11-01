namespace AlinSpace.Development
{
    /// <summary>
    /// Represents the solution.
    /// </summary>
    public static class Solution
    {
        /// <summary>
        /// Read solution file.
        /// </summary>
        /// <param name="pathToFile">Path to solution file.</param>
        /// <returns>Solution.</returns>
        public static ISolution Read(string pathToFile)
        {
            return SolutionInternal.Read(pathToFile);
        }
    }
}
