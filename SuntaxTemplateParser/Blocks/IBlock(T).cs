namespace Zion.STP
{
    public interface IBlock<T>
    {
        abstract T GetValue(StringView String, int Start);
    }
}