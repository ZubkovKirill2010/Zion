namespace Zion.STP
{
    public interface ITokenMatcher
    {
        bool Match(Token Token, out bool GoToNext);
    }
}