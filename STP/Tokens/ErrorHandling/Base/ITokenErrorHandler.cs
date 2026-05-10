namespace Zion.STP
{
    public interface ITokenErrorHandler
    {
        public void Handle(ref TextSource Source, out ErrorToken Token);
    }
}