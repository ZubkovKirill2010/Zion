namespace Zion.STP
{
    public readonly struct NodeParsingContext<Node> where Node : STP.Node
    {
        internal readonly INodeReader<Node>[] ReadersArray;

        public required ICollection<INodeReader<Node>> Readers
        {
            init
            {
                ArgumentNullException.ThrowIfNull(value);
                ArgumentException.ThrowIf(value.Count == 0, "NodeReaders not transmitted (Count = 0)");
                ReadersArray = ZArray.FromCollection(value);
            }
        }

        public required INodeErrorHandler<Node> ErrorHandler
        {
            get;
            init => field = value.NotNull();
        }

        public int Capacity
        {
            get;
            init => field = Math.Max(5, value);
        } = 20;


        public NodeParsingContext() { }

        public NodeParsingContext(ParsingContext<Node> Context)
        {
            ReadersArray = Context.NodeReadersArray;
            ErrorHandler = Context.NodeErrorHandler;
            Capacity     = Context.NodesCapacity;
        }


        public INodeReader<Node>[] GetNodeReaders()
        {
            return ZArray.Clone(ReadersArray);
        }
    }
}