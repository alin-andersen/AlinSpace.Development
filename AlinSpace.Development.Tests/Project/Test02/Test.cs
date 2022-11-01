namespace AlinSpace.Development.Tests.Project.Tests.Test02
{
    /// <summary>
    /// Sets version of dependency in project.
    /// </summary>
    public class Test
    {
        [Fact]
        public void Perform()
        {
            var project = Development.Project.Open("Project/Test02/Input.txt");
            var dependencies = project.GetDependencies();
            dependencies.First().Version = new Version(5, 6, 7);
            project.Save();

            Assert.Equal(
                expected: File.ReadAllText("Project/Test02/Expected.txt"),
                actual: File.ReadAllText("Project/Test02/Input.txt"));
        }
    }
}