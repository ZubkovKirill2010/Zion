namespace Zion.STP.Dynamic
{
    public sealed class DynamicNodeParser<Node> where Node : STP.Node
    {
        private readonly INodeReader<Node>[] Readers;
        private readonly INodeErrorHandler<Node> ErrorHandler;
        private readonly int Capacity;

        public DynamicNodeParser(ParsingContext<Node> Context)
        {
            Readers = Context.NodeReadersArray;
            ErrorHandler = Context.NodeErrorHandler;
            Capacity = Context.NodesCapacity;
        }

        public DynamicNodeParser(NodeParsingContext<Node> Context)
        {
            Readers = Context.ReadersArray;
            ErrorHandler = Context.ErrorHandler;
            Capacity = Context.Capacity;
        }
    }
}