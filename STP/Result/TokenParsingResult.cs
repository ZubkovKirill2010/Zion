namespace Zion.STP
{
    public readonly record struct TokenParsingResult
    (
        ListView<Token> Tokens,
        ListView<int>   Errors
    );
}