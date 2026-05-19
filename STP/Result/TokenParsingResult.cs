namespace Zion.STP
{
    public readonly record struct TokenParsingResult
    (
        List<Token> Tokens,
        TokenErrors Errors
    );
}
