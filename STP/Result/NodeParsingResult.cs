namespace Zion.STP
{
    public readonly record struct NodeParsingResult<Node>
    (
        List<Node> Nodes,
        SemanticData SemanticData,
        List<Verification> PendingVerifications,
        NodeErrors Errors
    ) where Node : STP.Node;
}