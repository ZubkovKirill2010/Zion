namespace Zion.STP
{
    public readonly struct ParsingContext<Node> where Node : STP.Node
    {
        internal readonly ITokenReader[] TokenReadersArray;
        internal readonly INodeReader<Node>[] NodeReadersArray;

        public required ICollection<ITokenReader> TokenReaders
        {
            init
            {
                ArgumentNullException.ThrowIfNull(value);
                ArgumentException.ThrowIf(value.Count == 0, "TokenReaders not transmitted (Count = 0)");
                TokenReadersArray = ZArray.FromCollection(value);
            }
        }
        public required ICollection<INodeReader<Node>> NodeReaders
        {
            init
            {
                ArgumentNullException.ThrowIfNull(value);
                ArgumentException.ThrowIf(value.Count == 0, "NodeReaders not transmitted (Count = 0)");
                NodeReadersArray = ZArray.FromCollection(value);
            }
        }

        public required ITokenErrorHandler TokenErrorHandler
        {
            get;
            init => field = value.NotNull();
        } = SkipCharTokenErrorHandler.Instance;
        public required INodeErrorHandler<Node> NodeErrorHandler
        {
            get;
            init => field = value.NotNull();
        }

        public int TokensCapacity
        {
            get;
            init => field = Math.Max(5, value);
        } = 40;
        public int NodesCapacity
        {
            get;
            init => field = Math.Max(5, value);
        } = 20;

        public Func<Symbol>? RootSymbol { get; init; }

        public ParsingContext() { }


        public ITokenReader[] GetTokenReaders()
        {
            return ZArray.Clone(TokenReadersArray);
        }

        public INodeReader<Node>[] GetNodeReaders()
        {
            return ZArray.Clone(NodeReadersArray);
        }
    }
}