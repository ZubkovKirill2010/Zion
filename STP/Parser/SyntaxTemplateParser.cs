namespace Zion.STP
{
    //Text -> Tokens -> Nodes -> Semantic verification -> Result
    public sealed class SyntaxTemplateParser<Node> where Node : STP.Node
    {
        private readonly TextSource Source;

        private readonly ITokenReader[] TokenReaders;
        private readonly ITokenErrorHandler TokenErrorHandler;

        private readonly NodeParser<Node> NodeParser;
       
        private readonly int TokensCapacity; 

        private readonly Func<Symbol>? RootSymbol;


        public SyntaxTemplateParser(TextSource Source, ParsingContext<Node> Context)
        {
            this.Source = Source.NotNull();

            TokenReaders = Context.TokenReadersArray;
            TokenErrorHandler = Context.TokenErrorHandler;
            TokensCapacity = Context.TokensCapacity;

            NodeParser = new NodeParser<Node>(Context);

            RootSymbol = Context.RootSymbol;
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
            return NodeParser.Parse(Tokens);
        }

        public void HandleSemantic(NodeParsingResult<Node> Nodes)
        {
            SemanticData Semantic = Nodes.SemanticData;

            foreach (Node Node in Nodes.Nodes)
            {
                Node.Verificate(Semantic);
            }
        }
    }
}