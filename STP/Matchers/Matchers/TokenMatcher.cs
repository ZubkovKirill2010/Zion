namespace Zion.STP
{
    public readonly struct TokenMatcher<T> : ITokenMatcher where T : Token
    {
        private readonly Func<T, bool> Predicate;

        public TokenMatcher(Func<T, bool>? Predicate = null)
        {
            this.Predicate = Predicate ?? (static _ => true);
        }

        public bool Match(Token Token, MatchingContext Context)
        {
            return Token is T Target && Predicate(Target);
        }
    }
}