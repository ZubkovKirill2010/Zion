namespace Zion.STP
{
    public interface INodeReader<N, SemanticData> where N : Node where SemanticData : class
    {
        public bool Read(TokenSlice Tokens, SemanticData SemanticData, out N Node);
    }
}