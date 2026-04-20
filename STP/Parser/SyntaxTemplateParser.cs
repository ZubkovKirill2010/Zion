namespace Zion.STP
{
    public sealed class SyntaxTemplateParser<T, Node> where Node : INode
    {
        private readonly ITextSource Source;
        private readonly ITokenReader[] TokenReaders;
        private readonly INodeReader<Node>[] NodeReaders;
        private readonly IParsingResult<T, Node> NodeConverter;

        public int TokenCapacity
        {
            get; 
            private init => field = Math.Max(10, value);
        }


        public SyntaxTemplateParser(ITextSource Source,
                                    ICollection<ITokenReader> TokenReaders,
                                    ICollection<INodeReader<Node>> NodeReaders,
                                    IParsingResult<T, Node> Result)
        {
            this.Source        = Source.NotNull();
            this.NodeConverter = Result.NotNull();
            this.TokenReaders  = ZArray.FromCollection(TokenReaders);
            this.NodeReaders   = ZArray.FromCollection(NodeReaders);
        }


        public bool TryParse(out T Result)
        {
            return NodeConverter.GetResult(new NodeSource<Node>(ReadNodes(ReadTokens())), out Result);
        }

        private List<IToken> ReadTokens()
        {
            ITextSource  Source = this.Source.BeginNew();
            List<IToken> Tokens = new List<IToken>(20);
            
            bool TokenReaded = false;

            while (!Source.IsEnd)
            {
                TokenReaded = false;

                foreach (ITokenReader Reader in TokenReaders)
                {
                    Source.Reset();

                    if (Reader.Read(Source, out IToken Token) && Token is not null)
                    {
                        if (Token.Length <= 0)
                        {
                            throw new ArgumentOutOfRangeException($"Token.Length(={Token.Length}) <= 0");
                        }

                        Source = Source.BeginNew();

                        TokenReaded = true;
                        Tokens.Add(Token);
                        break;
                    }
                }

                if (!TokenReaded)
                {
                    throw new Exception("!Temp! Not a single token came up !Temp!");
                }
            }

            return Tokens;
        }

        private List<Node> ReadNodes(List<IToken> Tokens)
        {
            List<Node> Nodes = new List<Node>(20);

            bool NodeReaded = false;
            int Start = 0;

            while (Start < Tokens.Count)
            {
                NodeReaded = false;

                foreach (INodeReader<Node> Reader in NodeReaders)
                {
                    Source.Reset();

                    if (Reader.Read(new TokenSlice(Tokens, Start), out Node Node) && Node is not null)
                    {
                        if (Node.TokensCount <= 0)
                        {
                            throw new ArgumentOutOfRangeException($"Node.TokensCount(={Node.TokensCount}) <= 0");
                        }

                        Start += Node.TokensCount;

                        NodeReaded = true;
                        Nodes.Add(Node);

                        break;
                    }
                }

                if (!NodeReaded)
                {
                    throw new Exception("!Temp! Not a single token came up !Temp!");
                }
            }

            return Nodes;
        }
    }
}