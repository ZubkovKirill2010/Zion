namespace Zion.STP
{
    public abstract class GroupTemplate : Template
    {
        public abstract bool Read(StringView String, int Start, out Group Group);

        public sealed override bool Read(StringView String, int Start, out Block Block)
        {
            if (Read(String, Start, out Group Group))
            {
                Block = Group;
                return true;
            }

            Block = default!;
            return false;
        }
    }
}