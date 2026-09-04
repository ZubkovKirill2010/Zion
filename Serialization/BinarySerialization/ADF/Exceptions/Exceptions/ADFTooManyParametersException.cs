namespace Zion.Serialization.ADF
{
    public sealed class ADFTooManyParametersException : ADFMismatchException
    {
        public ADFTooManyParametersException(int Count, int TargetCount)
            : base($"Too many parameters: expected {Count}, but received {TargetCount}") { }
    }
}