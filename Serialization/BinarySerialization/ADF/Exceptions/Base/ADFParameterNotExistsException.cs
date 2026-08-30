namespace Zion.Serialization.ADF
{
    public sealed class ADFParameterNotExistsException : ADFException
    {
        public ADFParameterNotExistsException(string ParameterName)
            : base($"Parameter '{ParameterName}' not exists.") { }
    }
}