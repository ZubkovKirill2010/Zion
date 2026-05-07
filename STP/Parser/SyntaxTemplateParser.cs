namespace Zion.STP
{
    //Text -> Tokens -> Nodes -> Result
    public sealed class SyntaxTemplateParser<T, Node> where Node : INode
    {
        private readonly TextSource Source;
        private readonly ITokenReader[] TokenReaders;
        private readonly INodeReader<Node>[] NodeReaders;
        private readonly IParsingResult<T, Node> NodeConverter;

        public TokenRecoveryStrategy TokenRecoveryStrategy { private get; init; } = TokenRecoveryStrategy.Abort;
        public ITokenErrorHandler TokenErrorHandler
        {
            private get;
            init => field = value.NotNull();
        } = SkipCharTokenErrorHandler.Instance;


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
                                    IParsingResult<T, Node> Result)
        {
            ArgumentException.ThrowIf(TokenReaders.Count == 0, "TokenReaders not transmitted (Count = 0)");
            ArgumentException.ThrowIf(NodeReaders.Count == 0, "NodeReaders not transmitted (Count = 0)");

            this.Source = Source.NotNull();
            NodeConverter = Result.NotNull();
            this.TokenReaders = ZArray.FromCollection(TokenReaders);
            this.NodeReaders = ZArray.FromCollection(NodeReaders);
        }


        private bool TryParse(out ParsingResult<T> Result)
        {
            List<ErrorData> Errors = new List<ErrorData>();

            void AddError(ErrorData Error) => Errors.Add(Error);

            if (!ReadTokens(out List<IToken>? Tokens, AddError))
            {
                Result = default!;
                return false;
            }

            if (!ReadNodes(Tokens, out List<Node>? Nodes, AddError))
            {
                Result = default!;
                return false;
            }

            if (NodeConverter.GetResult(new NodeSource<Node>(Nodes), out T ParsingResult))
            {
                Result = new ParsingResult<T>(ParsingResult, Errors.ToArray());
                return true;
            }

            Result = new ParsingResult<T>(default!, Array.Empty<ErrorData>());
            return false;
        }


        public bool ReadTokens(out List<IToken> Tokens, Action<ErrorData> AddError)
        {
            Tokens = new List<IToken>(TokensCapacity);

            TextSource Source = this.Source.BeginNew();

            while (!Source.IsEnd)
            {
                bool TokenReaded = false;

                foreach (ITokenReader Reader in TokenReaders)
                {
                    TextSource ReaderSource = Source.BeginNew();

                    if (Reader.Read(ref ReaderSource, out IToken Token) && Token is not null)
                    {
                        ArgumentNullException.ThrowIfNull(ReaderSource);
                        CheckTokenLength(Token);

                        TokenReaded = true;
                        Tokens.Add(Token);

                        Source = ReaderSource;

                        break;
                    }
                }

                if (!TokenReaded)
                {
                    if (TokenRecoveryStrategy == TokenRecoveryStrategy.SkipToSync)
                    {
                        TokenErrorHandler.Handle(ref Source, out ErrorToken ErrorToken);

                        CheckTokenLength(ErrorToken);

                        if (Tokens.Count != 0 && Tokens[^1] is ErrorToken LastErrorToken)
                        {
                            Tokens[^1] = new ErrorToken()
                            {
                                Length = LastErrorToken.Length + ErrorToken.Length
                            };
                        }
                        else
                        {
                            Tokens.Add(ErrorToken);
                        }

                        continue;
                    }

                    return false;
                }
            }

            return true;
        }

        public bool ReadNodes(List<IToken> Tokens, out List<Node> Nodes, Action<ErrorData> AddError)
        {
            Nodes = new List<Node>(NodesCapacity);

            bool NodeReaded = false;
            int Start = 0;

            while (Start < Tokens.Count)
            {
                NodeReaded = false;

                foreach (INodeReader<Node> Reader in NodeReaders)
                {
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
                    //if (CalculateErrorPosition)
                    //{
                    //    ErrorPosition = GetTokenOffset(Tokens, Start);
                    //}
                    return false;
                }
            }

            return true;
        }


        private static int GetTokenOffset(List<IToken> Tokens, int TokenIndex)
        {
            int Offset = 0;

            for (int i = 0; i < TokenIndex && i < Tokens.Count; i++)
            {
                Offset += Tokens[i].Length;
            }

            return Offset;
        }

        private static void CheckTokenLength(IToken Token)
        {
            ArgumentOutOfRangeException.ThrowIf(Token.Length <= 0, $"Token.Length(={Token.Length}) <= 0");
        }
    }
}