namespace Zion.STP
{
    public delegate void Verification(SemanticData Semantic);

    public abstract class VerifiableNode : Node
    {
        public abstract void Verificate(SemanticData Semantic);
    }
}