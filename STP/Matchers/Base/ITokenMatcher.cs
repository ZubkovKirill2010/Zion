namespace Zion.STP
{
    public interface ITokenMatcher
    {
        public bool Match(Token Token, out bool GoToNext);
    }
}