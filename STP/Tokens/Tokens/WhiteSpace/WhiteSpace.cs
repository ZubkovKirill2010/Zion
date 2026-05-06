namespace Zion.STP
{
    public readonly struct WhiteSpaceToken : IToken
    {
        public int Length { get; init; }
    }

    public readonly struct WhiteSpaceReader : ITokenReader
    {
        public bool Read(ref ITextSource Source, out IToken Token)
        {
            int Length = 0;

            foreach (char Char in Source)
            {
                if (char.IsWhiteSpace(Char))
                {
                    Length++;
                }
                else
                {
                    break;
                }
            }

            if (Length != 0)
            {
                Token = new WhiteSpaceToken() { Length = Length };
                return true;
            }

            Token = null!;
            return false;
        }
    }
}