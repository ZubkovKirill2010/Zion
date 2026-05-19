namespace Zion.STP
{
    public readonly struct SkipToBreakToken<N, SemanticData> : INodeErrorHandler<N, SemanticData> where N : Node, new() where SemanticData : class
    {
        private readonly HashSet<Type> BreakTokens;

        public SkipToBreakToken(IEnumerable<Type> BreakTokens)
        {
            this.BreakTokens = BreakTokens.ToHashSet();
        }

        public N Handle(TokenSlice Tokens, SemanticData SemanticData)
        {
            for (int i = 0; i < Tokens.Count; i++)
            {
                if (BreakTokens.Contains(Tokens[i].GetType()))
                {
                    return new N() { TokensCount = i };
                }
            }

            return new N() { TokensCount = Tokens.Count - 1 };
        }
    }
}