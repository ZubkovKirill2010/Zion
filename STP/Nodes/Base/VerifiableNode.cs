namespace Zion.STP
{
    public delegate bool Verification(SemanticData Semantic);

    public abstract class VerifiableNode : Node
    {
        public abstract void Verificate(SemanticData Semantic);
    }
}