namespace Zion.STP
{
    public interface ITokenMatcher
    {
        public bool Match(IToken Token, out bool GoToNext);
    }
}