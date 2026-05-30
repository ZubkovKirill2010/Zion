namespace Zion.STP
{
    public readonly record struct NodeParsingResult<Node>
    (
        List<Node> Nodes,
        SemanticData SemanticData,
        NodeErrors Errors
    ) where Node : STP.Node;
}