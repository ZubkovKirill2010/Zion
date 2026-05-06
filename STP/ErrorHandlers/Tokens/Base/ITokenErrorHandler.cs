namespace Zion.STP
{
    public enum TokenRecoveryStrategy
    {
        Abort, SkipToSync
    }

    public interface ITokenErrorHandler
    {
        public void Handle(ref ITextSource Source, out ErrorToken Token);
    }
}