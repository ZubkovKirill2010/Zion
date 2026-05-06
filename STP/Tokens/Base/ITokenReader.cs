namespace Zion.STP
{
    public interface ITokenReader
    {
        public bool Read(ref ITextSource Source, out IToken Token);
    }
}