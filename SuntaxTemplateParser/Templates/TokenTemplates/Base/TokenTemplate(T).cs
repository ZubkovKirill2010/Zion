namespace Zion.STP
{
    public abstract class TokenTemplate<T> : TokenTemplate, ITemplate<T>
    {
        public abstract bool Read(StringView String, int Start, out Token<T> Token);

        public bool ReadTyped(StringView String, int Start, out IBlock<T> Block)
        {
            if (Read(String, Start, out Token<T> TypedToken))
            {
                Block = TypedToken;
                return true;
            }

            Block = default!;
            return false;
        }

        public sealed override bool Read(StringView String, int Start, out Token Token)
        {
            if (Read(String, Start, out Token<T> TypedToken))
            {
                Token = TypedToken;
                return true;
            }

            Token = default!;
            return false;
        }
    }
}