namespace Zion.Serialization.ADF
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class ADFIgnoreAttribute : Attribute
    {

    }
}