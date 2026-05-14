namespace Zion.STP
{
    public interface INodeErrorHandler<N, SemanticData> where N : Node where SemanticData : class
    {
        public N Handle(TokenSlice Tokens, SemanticData SemanticData);
    }
}