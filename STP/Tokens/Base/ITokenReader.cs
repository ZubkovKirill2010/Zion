namespace Zion.STP
{
    public interface ITokenReader
    {
        bool Read(ITextSource Source, out IToken Token);
    }
}