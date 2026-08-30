namespace Zion.Serialization.ADF
{
    public sealed class ADFRepeatedNameException : ADFException
    {
        public ADFRepeatedNameException(string ParameterName)
            : base($"Parameter '{ParameterName}' already exists") { }
    }
}