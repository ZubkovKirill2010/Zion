namespace Zion.Serialization.ADF
{
    public readonly struct DataFormat
    {
        private readonly Parameter[] Parameters;
        private readonly FormatFlags Flags;

        public bool IsDynamic  => Flags.HasFlag(FormatFlags.IsDynamic);
        public bool IsNullable => Flags.HasFlag(FormatFlags.IsNullable);
        public bool IsAbstract => Flags.HasFlag(FormatFlags.IsAbstract);
        public bool HasParent  => Flags.HasFlag(FormatFlags.HasParent);


        public DataFormat(IEnumerable<Parameter> Parameters, FormatFlags Flags)
        {
            this.Parameters = Parameters.NotNull().ToArray();
            this.Flags = Flags;
        }
    }
}