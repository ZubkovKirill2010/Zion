namespace Zion.STP
{
    public readonly struct TokenMatcher<T> : ITokenMatcher where T : IToken
    {
        private readonly Func<T, bool> Predicate;

        public TokenMatcher(Func<T, bool>? Predicate = null)
        {
            this.Predicate = Predicate ?? (static _ => true);
        }

        public bool Match(IToken Token, out bool GoToNext)
        {
            GoToNext = true;
            return Token is T Target && Predicate(Target);
        }
    }
}