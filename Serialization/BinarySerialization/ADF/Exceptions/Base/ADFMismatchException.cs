namespace Zion.Serialization.ADF
{
    public abstract class ADFMismatchException : ADFException
    {
        public ADFMismatchException(string Message)
            : base(Message) { }
    }
}