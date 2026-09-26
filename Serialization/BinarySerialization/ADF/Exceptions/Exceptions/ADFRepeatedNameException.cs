namespace Zion.Serialization.ADF
{
    public sealed class ADFRepeatedNameException : ADFException
    {
        public readonly string ParameterName;

        public ADFRepeatedNameException(string ParameterName)
            : base($"Parameter '{ParameterName}' already exists")
        {
            this.ParameterName = ParameterName;
        }
    }
}