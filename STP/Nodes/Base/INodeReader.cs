namespace Zion.STP
{
    public interface INodeReader<Node> where Node : STP.Node
    {
        public bool Read(TokenSlice Tokens, out Node Node);
    }
}