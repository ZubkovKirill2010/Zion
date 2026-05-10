namespace Zion.STP
{
    public interface INodeErrorHandler<N> where N : Node
    {
        public N Handle(TokenSlice Tokens);
    }
}