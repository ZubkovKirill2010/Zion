namespace Zion.STP
{
    public enum TokenStatus : byte
    {
        Valid,
        HasErrors
    }

    public interface IToken
    {
        public int Length { get; init; }
        public TokenStatus Status { get; init; }
    }
}