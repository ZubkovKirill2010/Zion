namespace Zion.STP
{
    public sealed class NewLineToken : WhiteSpaceToken { }

    public readonly struct NewLineTokenReader : ITokenReader
    {
        public bool Read(ref TextSource Source, out Token Token)
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