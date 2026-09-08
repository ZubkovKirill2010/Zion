namespace Zion.Serialization.ADF
{
    public sealed class ADFObjectIsNullException : ADFException
    {
        public ADFObjectIsNullException(string ParameterName)
            : base($"{ParameterName} cannot be null") { }
    }
}