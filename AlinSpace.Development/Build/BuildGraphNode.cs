namespace AlinSpace.Development.Build
{
    public class BuildGraphNode
    {
        public IList<BuildGraphNode> Dependencies { get; set; } = new List<BuildGraphNode>();

        public IProject? Project { get; set; }

        public bool Builded { get; set; }
    }
}