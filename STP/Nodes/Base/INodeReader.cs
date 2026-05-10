namespace Zion.STP
{
    public interface INodeReader<N> where N : Node
    {
        public bool Read(TokenSlice Tokens, out N Node);
    }
}