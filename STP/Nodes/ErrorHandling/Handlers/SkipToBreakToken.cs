namespace Zion.STP
{
    public readonly struct SkipToBreakToken : INodeErrorHandler<Node>
    {
        private readonly HashSet<Token> BreakTokens; 

        public SkipToBreakToken(IEnumerable<Token> BreakTokens)
        {
            this.BreakTokens = BreakTokens.ToHashSet();
        }

        public Node Handle(TokenSlice Tokens)
        {

        }
    }
}