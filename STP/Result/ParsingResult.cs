namespace Zion.STP
{
    public readonly record struct ParsingResult<Node>
    (
        TokenParsingResult Tokens,
        NodeParsingResult<Node> Nodes
    ) where Node : STP.Node;
}