namespace Zion.STP
{
    public class WordToken : IValueToken<string>
    {
        public int Length { get; init; }
        public string Value { get; init; }
        public TokenStatus Status { get; init; }

        public override string ToString() => $"[{Value}]";
    }
}