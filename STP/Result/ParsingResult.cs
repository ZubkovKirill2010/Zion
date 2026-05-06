namespace Zion.STP
{
    public readonly record struct ParsingResult<T>
    (
        T Result,
        ErrorData[] Errors
    );

    public readonly record struct ErrorData
    (
        int Position
    );
}