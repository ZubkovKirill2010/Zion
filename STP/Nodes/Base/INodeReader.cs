namespace Zion.STP
{
    public interface INodeReader<Node> where Node : INode
    {
        public bool Read(TokenSlice Tokens, out Node Node);
    }
}