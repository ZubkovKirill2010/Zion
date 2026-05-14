namespace Zion.STP
{
    public abstract class VerifableNode<SemanticData> : Node where SemanticData : class
    {
        public abstract void Verificate(SemanticData Data);
    }
}