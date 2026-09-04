namespace Zion.Serialization.ADF
{
    public sealed class ADFNameMismatchException : ADFMismatchException
    {
        public ADFNameMismatchException(string Name, string TargetName)
            : base($"Name mismatch: Name: '{Name}', Target: '{TargetName}'") { }
    }
}