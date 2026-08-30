namespace Zion.Serialization.ADF
{
    public abstract class ADFException : Exception
    {
        public ADFException(string Message)
            : base(Message) { }
    }
}