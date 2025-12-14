namespace Zion.STP
{
    public sealed class EmptyTemplate : Template
    {
        public override bool Read(StringView String, int Start, out Block Block)
        {
            Block = new EmptyToken();
            return true;
        }
    }
}