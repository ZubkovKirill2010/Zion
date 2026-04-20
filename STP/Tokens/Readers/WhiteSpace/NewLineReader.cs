namespace Zion.STP
{
    public readonly struct NewLineReader : ITokenReader
    {
        public bool Read(ITextSource Source, out IToken Token)
        {
            if (Source.Begins("\n"))
            {
                Token = new NewLineToken() { Length = 1 };
                return true;
            }
            else if (Source.Begins("\r\n"))
            {
                Token = new NewLineToken() { Length = 1 };
                return true;
            }

            Token = null!;
            return false;
        }
    }
}