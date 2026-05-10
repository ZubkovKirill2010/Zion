namespace Zion.STP
{
    public class WhiteSpaceToken : Token { }

    public readonly struct WhiteSpaceTokenReader : ITokenReader
    {
        public bool Read(ref TextSource Source, out Token Token)
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