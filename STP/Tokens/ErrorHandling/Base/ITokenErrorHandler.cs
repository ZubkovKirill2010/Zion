namespace Zion.STP
{
    public interface ITokenErrorHandler
    {
        void Handle(ref TextSource Source, out ErrorToken Token);
    }
}