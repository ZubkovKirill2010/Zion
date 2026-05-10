namespace Zion.STP
{
    public interface ITokenReader
    {
        public bool Read(ref TextSource Source, out Token Token);
    }
}