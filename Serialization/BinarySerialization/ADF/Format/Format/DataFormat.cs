using System.Collections;

namespace Zion.Serialization.ADF
{
    public readonly struct DataFormat : IEnumerable<Parameter>
    {
        public static readonly DataFormat Object = new DataFormat
        (
            [], FormatFlags.IsNullable | FormatFlags.IsAbstract | FormatFlags.IsReference
        );

        private readonly Parameter[] Parameters;
        private readonly FormatFlags Flags;

        public readonly uint BaseFormat;

        public bool IsDynamic  => Flags.HasFlag(FormatFlags.IsDynamic);
        public bool IsNullable => Flags.HasFlag(FormatFlags.IsNullable);
        public bool IsAbstract => Flags.HasFlag(FormatFlags.IsAbstract);
        public bool IsClass    => Flags.HasFlag(FormatFlags.IsReference);

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


        public int IndexOf(uint NameId, int Start)
        {
            var Span = Parameters.AsSpan();
            var Count = Span.Length;

            for (int i = Start; i < Count; i++)
            {
                if (Span[i].NameId == NameId)
                {
                    return i;
                }
            }

            return -1;
        }

        public int IndexOf(uint NameId)
        {
            return IndexOf(NameId, 0);
        }

        public bool Contains(uint NameId)
        {
            return IndexOf(NameId) != -1;
        }


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<Parameter> GetEnumerator()
        {
            return Parameters.Enumerate();
        }
    }
}