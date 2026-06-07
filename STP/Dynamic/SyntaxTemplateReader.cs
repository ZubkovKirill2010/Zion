namespace Zion.STP.Dynamic
{
    public sealed class SyntaxTemplateReader<Node, TPointer> where Node : STP.Node where TPointer : TextPointer<TPointer>
    {
        private readonly TextDocument<TPointer> Document;

        private readonly ITokenReader[] TokenReaders;
        private readonly ITokenErrorHandler TokenErrorHandler;

        private readonly DynamicNodeReader<Node> NodeReader;

        private readonly int TokensCapacity;

        private readonly Func<Symbol>? RootSymbol;

        
        public SyntaxTemplateReader(TextDocument<TPointer> Document, ParsingContext<Node> Context)
        {
            this.Document = Document.NotNull();

            TokenReaders = Context.TokenReadersArray;
            TokenErrorHandler = Context.TokenErrorHandler;

            NodeReader = new DynamicNodeReader<Node>(Context);

            TokensCapacity = Context.TokensCapacity;

            RootSymbol = Context.RootSymbol;
        }
    }
}