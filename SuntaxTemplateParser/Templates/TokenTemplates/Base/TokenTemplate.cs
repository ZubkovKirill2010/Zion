namespace Zion.STP
{
    public abstract class TokenTemplate : Template
    {
        public abstract bool Read(StringView String, int Start, out Token Token);

        public sealed override bool Read(StringView String, int Start, out Block Block)
        {
            if (Read(String, Start, out Token Token))
            {
                Block = Token;
                return true;
            }

            Block = default!;
            return false;
        }
    }
}