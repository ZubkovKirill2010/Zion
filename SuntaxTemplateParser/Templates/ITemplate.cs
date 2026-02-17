namespace Zion.STP
{
    public interface ITemplate<T>
    {
        bool ReadTyped(StringView String, int Start, out IBlock<T> Block);
    }
}