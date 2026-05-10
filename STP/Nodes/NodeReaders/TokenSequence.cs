namespace Zion.STP
{
    public readonly struct TokenSequence<N> : INodeReader<N> where N : Node
    {
        private readonly ITokenMatcher[] Sequence;
        private readonly Func<TokenSlice, N> GetNode;

        public TokenSequence(ICollection<ITokenMatcher> Sequence, Func<TokenSlice, N> GetNode)
        {
            this.Sequence = ZArray.FromCollection(Sequence);
            this.GetNode = GetNode.NotNull();
        }

        public bool Read(TokenSlice Tokens, out N Node)
        {
            Node = default!;

            if (Tokens.Count < Sequence.Length)
            {
                return false;
            }

            int TokenIndex = 0;

            foreach (ITokenMatcher Matcher in Sequence)
            {
                if (!Matcher.Match(Tokens[TokenIndex], out bool GoToNext))
                {
                    return false;
                }
                if (GoToNext)
                {
                    TokenIndex++;
                }
            }

            Node = GetNode(Tokens);
            return true;
        }
    }
}