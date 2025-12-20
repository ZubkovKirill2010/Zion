namespace Zion.STP
{
    public abstract class GroupTemplate : Template
    {
        public override sealed bool Read(StringView String, int Start, out Block Block)
        {
            bool Result = Read(String: String, Start: Start, Group: out Group Group);
            Block = Group;
            return Result;
        }

        public abstract bool Read(StringView String, int Start, out Group Group);
    }
}