using System.Collections;

namespace Zion.Serialization.ADF
{
    public readonly struct DataFormat : IEnumerable<Parameter>
    {
        public static readonly DataFormat Object = new DataFormat
        (
            [], FormatFlags.IsNullable | FormatFlags.IsAbstract | FormatFlags.IsClass
        );

        private readonly Parameter[] Parameters;
        private readonly FormatFlags Flags;

        public readonly uint BaseFormat;

        public bool IsDynamic  => Flags.HasFlag(FormatFlags.IsDynamic);
        public bool IsNullable => Flags.HasFlag(FormatFlags.IsNullable);
        public bool IsAbstract => Flags.HasFlag(FormatFlags.IsAbstract);
        public bool IsClass    => Flags.HasFlag(FormatFlags.IsClass);

        public int ParametersCount => Parameters.Length;


        public DataFormat(IEnumerable<Parameter> Parameters, FormatFlags Flags)
        {
            this.Parameters = Parameters.NotNull().ToArray();
            this.Flags = Flags;
        }

        public DataFormat(IEnumerable<Parameter> Parameters, FormatFlags Flags, uint BaseFormat)
        {
            this.Parameters = Parameters.NotNull().ToArray();
            this.Flags = Flags;
            this.BaseFormat = BaseFormat;
        }


        public Parameter this[int   Index] => Parameters[Index];
        
        public Parameter this[Index Index] => Parameters[Index];


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<Parameter> GetEnumerator()
        {
            return Parameters.Enumerate();
        }
    }
}