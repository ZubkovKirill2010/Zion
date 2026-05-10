namespace Zion.STP
{
    public readonly record struct ParsingResult<T>
    (
        T Result,
        TokenSlice Tokens,
        ParsingErrors Errors
    );

    public readonly record struct ParsingErrors
    (
        TokenErrors TokenErrors,
        NodeErrors  NodeErrors
    );

    public readonly record struct TokenErrors
    (
        int[] InvalidTokens,
        int[] ErrorTokens
    );

    public readonly record struct NodeErrors
    (
        int[] InvalidNodes,
        int[] ErrorNodes
    );
}