namespace Zion.STP
{
    public interface ITemplate<T>
    {
        public bool ReadTyped(StringView String, int Start, out IBlock<T> Block);
    }
}