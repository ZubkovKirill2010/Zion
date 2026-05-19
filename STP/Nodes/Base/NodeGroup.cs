namespace Zion.STP
{
    public abstract class NodeGroup : VerifableNode
    {
        private readonly List<Node> Childs;

        public NodeGroup()
        {
            Childs = new List<Node>();
        }
        public NodeGroup(int ChildsCapacity)
        {
            Childs = new List<Node>(Math.Max(1, ChildsCapacity));
        }

        protected abstract SemanticData GetSemanticData();

        public sealed override void Verificate(SemanticData Data)
        {
            SemanticCollecter.Collect(Childs, Data);
        }


        public void AddChild(Node Child)
        {
            Childs.Add();
        }
    }
}