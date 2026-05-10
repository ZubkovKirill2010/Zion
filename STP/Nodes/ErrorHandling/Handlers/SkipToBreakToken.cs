namespace Zion.STP
{
    public readonly struct SkipToBreakToken<N> : INodeErrorHandler<N> where N : Node, new()
    {
        private readonly HashSet<Type> BreakTokens;

        public SkipToBreakToken(IEnumerable<Type> BreakTokens)
        {
            this.BreakTokens = BreakTokens.ToHashSet();
        }

        public N Handle(TokenSlice Tokens)
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