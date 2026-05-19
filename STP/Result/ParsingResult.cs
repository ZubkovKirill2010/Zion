namespace Zion.STP
{
    public readonly record struct ParsingResult<Node>
    (
        ListView<Token> Tokens,
        ListView<Node> Nodes,
        ParsingErrors Errors
    ) where Node : STP.Node;

    public readonly record struct ParsingErrors
    (
        TokenErrors TokenErrors,
        NodeErrors NodeErrors
    );

    public readonly record struct TokenErrors
    (
        int[] InvalidTokens, //Токены с внутренней ошибкой.
        int[] ErrorTokens   //Токены которых не существует
    );

    public readonly record struct NodeErrors
    (
        int[] InvalidNodes,//Ноды содержащие InvalidTokens / токены не прошедшие семантическую проверку
        int[] ErrorNodes  // Ноды содержащие ErrorTokens
    );
}