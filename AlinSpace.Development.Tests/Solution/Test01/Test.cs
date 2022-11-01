namespace AlinSpace.Development.Tests.Solution.Tests.Test01
{
    /// <summary>
    /// Enumerates through Project of solution. 
    /// </summary>
    public class Test
    {
        [Fact]
        public void Perform()
        {
            var solution = Development.Solution.Read("Solution/Test01/Input.txt");

            Assert.Equal(
                expected: 3,
                actual: solution.Projects.Count());

            Assert.Equal(
                expected: "AlinSpace.ProjectManipulator",
                actual: solution.Projects.First().Name);

            Assert.Equal(
                expected: "TestProject",
                actual: solution.Projects.Skip(1).First().Name);

            Assert.Equal(
                expected: "AlinSpace.Development.Tests.Tests",
                actual: solution.Projects.Skip(2).First().Name);
        }
    }
}