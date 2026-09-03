namespace Zion.Serialization.ADF
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class ADFNameAttribute : Attribute
    {
        public readonly string Name;

        public ADFNameAttribute(string Name)
        {
            this.Name = Name.NotNull();
        }
    }
}