namespace Zion.STP
{
    public interface IBlock<T>
    {
        public abstract T GetValue(StringView String, int Start);
    }
}