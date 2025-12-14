namespace Zion.STP
{
    public sealed class EmptyToken : Token
    {
        public override bool Check(StringView String, int Start)
        {
            return true;
        }
    }
}