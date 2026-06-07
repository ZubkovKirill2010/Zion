namespace Zion.STP.Dynamic
{
    public sealed class DynamicNodeReader<Node> where Node : STP.Node
    {
        private readonly INodeReader<Node>[] Readers;
        private readonly INodeErrorHandler<Node> ErrorHandler;
        private readonly int Capacity;

        public DynamicNodeReader(ParsingContext<Node> Context)
        {
            Readers = Context.NodeReadersArray;
            ErrorHandler = Context.NodeErrorHandler;
            Capacity = Context.NodesCapacity;
        }

        public DynamicNodeReader(NodeParsingContext<Node> Context)
        {
            Readers = Context.ReadersArray;
            ErrorHandler = Context.ErrorHandler;
            Capacity = Context.Capacity;
        }
    }
}