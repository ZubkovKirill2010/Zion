namespace Zion.STP
{
    public readonly record struct NodeParsingResult<Node>
    (
        List<Node>    Nodes,
        SemanticData  SemanticData,
        int ErrorCount
    ) where Node : STP.Node
    {
        public bool ContainsErrors => ErrorCount > 0;
    }
}