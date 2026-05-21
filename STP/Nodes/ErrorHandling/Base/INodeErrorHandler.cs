namespace Zion.STP
{
    public interface INodeErrorHandler<Node> where Node : STP.Node
    {
        public Node Handle(TokenSlice Tokens);
    }
}