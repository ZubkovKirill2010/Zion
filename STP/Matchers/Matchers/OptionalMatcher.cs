namespace Zion.STP
{
    public sealed class OptionalMatcher : ITokenMatcher
    {
        private readonly ITokenMatcher Validator;

        public OptionalMatcher(ITokenMatcher Validator)
        {
            this.Validator = Validator.NotNull();
        }

        public bool Match(IToken Token, out bool GoToNext)
        {
            if (!Validator.Match(Token, out GoToNext))
            {
                GoToNext = false;
            }
            return true;
        }
    }
}