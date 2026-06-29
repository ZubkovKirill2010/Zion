namespace Zion.STP
{
    public interface ITokenMatcher
    {
        public bool Match(Token Token, MatchingContext Context);
    }
}