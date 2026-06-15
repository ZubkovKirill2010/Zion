namespace Zion.STP
{
    public readonly record struct NodeParsingResult<Node>
    (
        List<Node>    Nodes,
        SemanticData  SemanticData,
        ListView<int> Errors
    ) where Node : STP.Node;
}