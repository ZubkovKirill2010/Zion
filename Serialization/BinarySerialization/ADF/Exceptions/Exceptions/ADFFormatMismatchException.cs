namespace Zion.Serialization.ADF
{
    public sealed class ADFFormatMismatchException : ADFMismatchException
    {
        public ADFFormatMismatchException(uint Value, uint Target)
            : base($"Format mismatch: Value: '{Value}', Target: {Target}") { }
    }
}