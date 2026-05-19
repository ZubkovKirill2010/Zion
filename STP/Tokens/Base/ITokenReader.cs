namespace Zion.STP
{
    public interface ITokenReader
    {
        bool Read(ref TextSource Source, out Token Token);
    }
}