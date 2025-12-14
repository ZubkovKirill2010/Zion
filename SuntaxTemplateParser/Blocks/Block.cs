namespace Zion.STP
{
    public abstract class Block
    {
        public event Action Changed;
        public event Action<int> LengthChanged;

        public abstract int Length { get; }

        public abstract bool Check(StringView String, int Start, out int Length);
    }
}