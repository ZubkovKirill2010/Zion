namespace Zion.STP
{
    public abstract class VerifableNode<S> : Node where S : ISemanticData
    {
        public abstract void Verificate(S Data);
    }
}