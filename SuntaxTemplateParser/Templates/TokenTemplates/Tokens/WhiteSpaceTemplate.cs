namespace Zion.STP
{
    public sealed class WhiteSpaceTemplate : TokenTemplate
    {
        public override bool Read(StringView String, int Start, out Token Token)
        {
            int Index = Start;

            while (Index < String.Length && char.IsWhiteSpace(String[Index]))
            {
                Index++;
            }

            return Accessor.Try
            (
                Index == Start,
                () => new WhiteSpaceToken(Index - Start),
                out Token
            );
        }
    }
}