namespace Zion.STP
{
    public class WordToken : ValueToken<string>
    {
        public override string ToString()
        {
            return $"[Word:\"{Value}\"]";
        }
    }
}