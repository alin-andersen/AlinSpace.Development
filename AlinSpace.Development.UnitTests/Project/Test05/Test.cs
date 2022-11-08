namespace AlinSpace.Development.Tests.Project.Tests.Test05
{
    /// <summary>
    /// Update dependency version of project.
    /// </summary>
    public class Test
    {
        [Fact]
        public void Perform()
        {
            var project = Development.Project.Open("Project/Test05/Input.txt");
            project.AddOrUpdateDependency("AutoMapper", new Version(7, 8, 9));
            project.Save();

            Assert.Equal(
                expected: File.ReadAllText("Project/Test05/Expected.txt"),
                actual: File.ReadAllText("Project/Test05/Input.txt"));
        }
    }
}