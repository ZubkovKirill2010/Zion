namespace Zion.STP
{
    public abstract class Template
    {
        public abstract bool Read(StringView String, int Start, out Block Block);
    }
}