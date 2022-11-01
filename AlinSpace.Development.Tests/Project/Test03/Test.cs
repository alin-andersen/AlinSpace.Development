namespace AlinSpace.Development.Tests.Project.Tests.Test03
{
    /// <summary>
    /// Remove dependency from project.
    /// </summary>
    public class Test
    {
        [Fact]
        public void Perform()
        {
            var project = Development.Project.Open("Project/Test03/Input.txt");
            var dependencies = project.GetDependencies();
            dependencies.First().Remove();
            project.Save();

            Assert.Equal(
                expected: File.ReadAllText("Project/Test03/Expected.txt"),
                actual: File.ReadAllText("Project/Test03/Input.txt"));
        }
    }
}