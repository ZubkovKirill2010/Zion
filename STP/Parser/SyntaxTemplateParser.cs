namespace Zion.STP
{
    //Text -> Tokens -> Nodes -> Semantic verification -> Result
    public sealed class SyntaxTemplateParser<Node> where Node : STP.Node
    {
        private readonly TextSource Source;
        private readonly ITokenReader[] TokenReaders;
        private readonly INodeReader<Node>[] NodeReaders;

        public required ITokenErrorHandler TokenErrorHandler
        {
            private get;
            init => field = value.NotNull();
        } = SkipCharTokenErrorHandler.Instance;
        public required INodeErrorHandler<Node> NodeErrorHandler
        {
            private get;
            init => field = value.NotNull();
        }

        public int TokensCapacity
        {
            get;
            private init => field = Math.Max(5, value);
        } = 20;
        public int NodesCapacity
        {
            get;
            private init => field = Math.Max(5, value);
        } = 10;


        public Func<Symbol>? RootSymbol { get; init; }


        public SyntaxTemplateParser(TextSource Source,
                                    ICollection<ITokenReader> TokenReaders,
                                    ICollection<INodeReader<Node>> NodeReaders)
        {
            ArgumentException.ThrowIf(TokenReaders.Count == 0, "TokenReaders not transmitted (Count = 0)");
            ArgumentException.ThrowIf(NodeReaders.Count == 0, "NodeReaders not transmitted (Count = 0)");

            this.Source = Source.NotNull();
            this.TokenReaders = ZArray.FromCollection(TokenReaders);
            this.NodeReaders = ZArray.FromCollection(NodeReaders);
        }


        public ParsingResult<Node> Parse()
        {
            TokenParsingResult Tokens = ReadTokens();
            NodeParsingResult<Node> Nodes = ReadNodes(Tokens.Tokens);

            HandleSemantic(Nodes);

            return new ParsingResult<Node>
            (
                new ListView<Token>(Tokens.Tokens),
                new ListView<Node>(Nodes.Nodes),
                new ParsingErrors(Tokens.Errors, Nodes.Errors)
            );
        }


        public TokenParsingResult ReadTokens()
        {
            TextSource Source = this.Source.BeginNew();

            List<Token> Tokens = new List<Token>(TokensCapacity);

            List<int> InvalidTokens = new List<int>(5);
            List<int> ErrorTokens = new List<int>(5);

            void AddToken(Token Token)
            {
                if (Token.Status == Validation.Invalid)
                {
                    InvalidTokens.Add(Tokens.Count);
                }
                Tokens.Add(Token);
            }

            void AddErrorToken(ErrorToken Token)
            {
                ErrorTokens.Add(Tokens.Count);
                Tokens.Add(Token);
            }

            while (!Source.IsEnd)
            {
                bool TokenReaded = false;

                foreach (ITokenReader Reader in TokenReaders)
                {
                    TextSource ReaderSource = Source.BeginNew();

                    if (Reader.Read(ref ReaderSource, out Token Token) && Token is not null)
                    {
                        ArgumentNullException.ThrowIfNull(ReaderSource);

                        TokenReaded = true;
                        AddToken(Token);

                        Source = ReaderSource;

                        break;
                    }
                }

                if (!TokenReaded)
                {
                    TokenErrorHandler.Handle(ref Source, out ErrorToken ErrorToken);

                    if (Tokens.Count != 0 && Tokens[^1] is ErrorToken LastErrorToken)
                    {
                        Tokens[^1] = new ErrorToken()
                        {
                            Length = LastErrorToken.Length + ErrorToken.Length
                        };
                    }
                    else
                    {
                        AddErrorToken(ErrorToken);
                    }
                }
            }

            return new TokenParsingResult(Tokens, new TokenErrors(InvalidTokens.ToArray(), ErrorTokens.ToArray()));
        }

        public NodeParsingResult<Node> ReadNodes(List<Token> Tokens)
        {
            List<Node> Nodes = new List<Node>(NodesCapacity);
            List<Symbol> SemanticTree = new List<Symbol>(10);

            List<int> InvalidNodes = new List<int>(5);

            List<Verification> PendingVerifications = new List<Verification>();

            bool NodeReaded = false;
            int Start = 0;

            TokenSlice GetTokenSlice() => new TokenSlice(Tokens, Start);

            while (Start < Tokens.Count)
            {
                NodeReaded = false;

                foreach (INodeReader<Node> Reader in NodeReaders)
                {
                    if (Reader.Read(GetTokenSlice(), out Node Node) && Node is not null)
                    {
                        SemanticTree.AddIfNotNull(Node.GetSymbol());

                        if (Node is VerifiableNode Verifiable)
                        {
                            PendingVerifications.Add(Verifiable.Verificate);
                        }

                        Start += Node.TokensCount;
                        NodeReaded = true;
                        Nodes.Add(Node);
                        break;
                    }
                }

                if (!NodeReaded)
                {
                    Node ErrorNode = NodeErrorHandler.Handle(GetTokenSlice());
                    Nodes.Add(ErrorNode);
                }
            }

            return new NodeParsingResult<Node>
            (
                Nodes,
                new SemanticData(SemanticTree),
                PendingVerifications,
                new NodeErrors()
            );
        }

        public void HandleSemantic(NodeParsingResult<Node> Nodes)
        {
            SemanticData Semantic = Nodes.SemanticData;

            foreach (Verification Verification in Nodes.PendingVerifications)
            {
                Verification.Invoke(Semantic);
            }
        }
    }
}