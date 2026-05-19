namespace Zion.STP
{
    //Text -> Tokens -> Nodes -> Semantic verification -> Result
    public sealed class SyntaxTemplateParser<Result, Node> where Node : STP.Node
    {
        private readonly TextSource Source;
        private readonly ITokenReader[] TokenReaders;
        private readonly INodeReader<Node>[] NodeReaders;
        private readonly IParsingResult<Result, Node> ResultConverter;

        public RecoveryStrategy TokenRecoveryStrategy { get; init; } = RecoveryStrategy.Abort;
        public RecoveryStrategy NodeRecoveryStrategy { get; init; } = RecoveryStrategy.Abort;

        public ITokenErrorHandler TokenErrorHandler
        {
            private get;
            init => field = value.NotNull();
        } = SkipCharTokenErrorHandler.Instance;
        public INodeErrorHandler<Node, SemanticData>? NodeErrorHandler
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



        public SyntaxTemplateParser(TextSource Source,
                                    ICollection<ITokenReader> TokenReaders,
                                    ICollection<INodeReader<Node>> NodeReaders,
                                    IParsingResult<Result, Node> Result)
        {
            ArgumentException.ThrowIf(TokenReaders.Count == 0, "TokenReaders not transmitted (Count = 0)");
            ArgumentException.ThrowIf(NodeReaders.Count == 0, "NodeReaders not transmitted (Count = 0)");

            ResultConverter = Result.NotNull();
            this.Source = Source.NotNull();
            this.TokenReaders = ZArray.FromCollection(TokenReaders);
            this.NodeReaders = ZArray.FromCollection(NodeReaders);
        }


        public bool Parse(out ParsingResult<Result> Result)
        {
            if (!ReadTokens(out List<Token>? Tokens, out TokenErrors TokenErrors))
            {
                Result = default!;
                return false;
            }

            if (!ReadNodes(Tokens, out List<Node> Nodes, out SemanticData SemanticData, out NodeErrors NodeErrors))
            {
                Result = default!;
                return false;
            }

            if (!HandleSemantic(Nodes, SemanticData))
            {
                Result = default!;
                return false;
            }

            if (ResultConverter.GetResult(new NodeSource<Node>(Nodes), out Result ParsingResult))
            {
                Result = new ParsingResult<Result>
                (
                    ParsingResult,
                    new TokenSlice(Tokens, 0),
                    new ParsingErrors(TokenErrors, NodeErrors)
                );
                return true;
            }

            Result = new ParsingResult<Result>();
            return false;
        }


        public bool ReadTokens(out List<Token> Tokens, out TokenErrors Errors)
        {
            TextSource Source = this.Source.BeginNew();

            List<Token> TokenList = new List<Token>(TokensCapacity);

            List<int> InvalidTokens = new List<int>(5);
            List<int> ErrorTokens = new List<int>(5);

            void AddToken(Token Token)
            {
                if (Token.Status == Validation.Invalid)
                {
                    InvalidTokens.Add(TokenList.Count);
                }
                TokenList.Add(Token);
            }

            void AddErrorToken(ErrorToken Token)
            {
                ErrorTokens.Add(TokenList.Count);
                TokenList.Add(Token);
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
                    if (TokenRecoveryStrategy == RecoveryStrategy.Synchronize)
                    {
                        TokenErrorHandler.Handle(ref Source, out ErrorToken ErrorToken);

                        if (TokenList.Count != 0 && TokenList[^1] is ErrorToken LastErrorToken)
                        {
                            TokenList[^1] = new ErrorToken()
                            {
                                Length = LastErrorToken.Length + ErrorToken.Length
                            };
                        }
                        else
                        {
                            AddErrorToken(ErrorToken);
                        }

                        continue;
                    }

                    Tokens = default!;
                    Errors = new TokenErrors(InvalidTokens.ToArray(), ErrorTokens.ToArray());
                    return false;
                }
            }

            Tokens = TokenList;
            Errors = new TokenErrors(InvalidTokens.ToArray(), ErrorTokens.ToArray());

            return true;
        }

        public bool ReadNodes(List<Token> Tokens, out List<Node> Nodes, out SemanticData SemanticData, out NodeErrors Errors)
        {
            Nodes = new List<Node>(NodesCapacity);
            SemanticData = new SemanticData();

            List<int> InvalidNodes = new List<int>(5);
            List<int> ErrorNodes = new List<int>(5);

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
                        //if (Node is VerifableNode VerifableNode)
                        //{
                            //Verification += VerifableNode.Verificate;
                        //}

                        Start += Node.TokensCount;
                        NodeReaded = true;
                        Nodes.Add(Node);
                        break;
                    }
                }

                if (!NodeReaded)
                {
                    if (NodeRecoveryStrategy == RecoveryStrategy.Synchronize && NodeErrorHandler is not null)
                    {
                        Node ErrorNode = NodeErrorHandler.Handle(GetTokenSlice(), SemanticData);

                        ErrorNodes.Add(Nodes.Count);
                        Nodes.Add(ErrorNode);

                        continue;
                    }

                    Errors = new NodeErrors(InvalidNodes.ToArray(), [Nodes.Count]);
                    return false;
                }
            }

            Errors = new NodeErrors();
            return true;
        }

        public bool HandleSemantic(List<Node> Nodes, SemanticData SemanticData)
        {

        }
    }
}