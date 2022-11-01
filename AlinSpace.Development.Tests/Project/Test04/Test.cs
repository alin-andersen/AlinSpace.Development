namespace AlinSpace.Development.Tests.Project.Tests.Test04
{
    /// <summary>
    /// Add dependency to project.
    /// </summary>
    public class Test
    {
        [Fact]
        public void Perform()
        {
            var project = Development.Project.Open("Project/Test04/Input.txt");
            project.AddOrUpdateDependency("AutoMapper", new Version(3, 4, 5));
            project.Save();

            Assert.Equal(
                expected: File.ReadAllText("Project/Test04/Expected.txt"),
                actual: File.ReadAllText("Project/Test04/Input.txt"));
        }
    }
}