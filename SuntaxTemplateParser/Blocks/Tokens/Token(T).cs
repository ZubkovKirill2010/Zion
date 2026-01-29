namespace Zion.STP
{
    public abstract class Token<T> : Token, IBlock<T>
    {
        public abstract T GetValue(StringView String, int Start);
    }
}