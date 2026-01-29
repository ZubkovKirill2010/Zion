namespace Zion.STP
{
    public abstract class GroupTemplate<T> : GroupTemplate, ITemplate<T>
    {
        public abstract bool Read(StringView String, int Start, out Group<T> Group);

        public bool ReadTyped(StringView String, int Start, out IBlock<T> Block)
        {
            if (Read(String, Start, out Group<T> TypedGroup))
            {
                Block = TypedGroup;
                return true;
            }

            Block = default!;
            return false;
        }

        public sealed override bool Read(StringView String, int Start, out Group Group)
        {
            if (Read(String, Start, out Group<T> TypedGroup))
            {
                Group = TypedGroup;
                return true;
            }

            Group = default!;
            return false;
        }
    }
}