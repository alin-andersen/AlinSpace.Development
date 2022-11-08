namespace AlinSpace.Development.Tests.Project.Tests.Test01
{
    /// <summary>
    /// Sets version of project.
    /// </summary>
    public class Test
    {
        [Fact]
        public void Perform()
        {
            var project = Development.Project.Open("Project/Test01/Input.txt");
            project.Version = new Version(1, 2, 3);
            project.Save();

            Assert.Equal(
                expected: File.ReadAllText("Project/Test01/Expected.txt"),
                actual: File.ReadAllText("Project/Test01/Input.txt"));
        }
    }
}