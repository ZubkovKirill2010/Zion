namespace Zion.STP
{
    public sealed class ValueMatcher<T> : ITokenMatcher
    {
        private readonly T? Target;

        public Func<T?, T?, bool> Comparer { get; init; } = static (A, B) => A?.Equals(B) ?? false;

        public ValueMatcher(T? Target)
        {
            this.Target = Target;
        }

        public bool Match(Token Token, MatchingContext Context)
        {
            return Token is ValueToken<T> ValueToken && Comparer(ValueToken.Value, Target);
        }
    }
}