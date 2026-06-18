namespace Zion.STP
{
    public sealed class OptionalMatcher : ITokenMatcher
    {
        private readonly ITokenMatcher Validator;

        public OptionalMatcher(ITokenMatcher Validator)
        {
            this.Validator = Validator.NotNull();
        }

        public bool Match(Token Token, MatchingContext Context)
        {
            if (!Validator.Match(Token, Context))
            {
                Context.GoToNext = false;
            }
            return true;
        }
    }
}