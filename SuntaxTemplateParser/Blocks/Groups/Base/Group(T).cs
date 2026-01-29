namespace Zion.STP
{
    public abstract class Group<T> : Group, IBlock<T>
    {
        public abstract T GetValue(StringView String, int Start);
    }
}