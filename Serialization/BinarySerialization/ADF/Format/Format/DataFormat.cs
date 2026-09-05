using System.Collections;

namespace Zion.Serialization.ADF
{
    public readonly struct DataFormat : IEnumerable<Parameter>
    {
        #region Data
        private readonly Parameter[] Parameters;
        
        public readonly FormatFlags Flags;
        public readonly uint BaseFormat;

        #endregion

        #region Properties
        public bool IsArray     => Flags.HasFlag(FormatFlags.IsArray);
        public bool IsReference => Flags.HasFlag(FormatFlags.IsReference);
        public bool IsAbstract  => Flags.HasFlag(FormatFlags.IsAbstract);
        public bool IsNullable  => Flags.HasFlag(FormatFlags.IsNullable);
        public bool IsEnum      => Flags.HasFlag(FormatFlags.IsEnum);

        public int ParametersCount => Parameters.Length;

        #endregion

        #region Constructors
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

        #endregion

        #region Indexers
        public Parameter this[int   Index] => Parameters[Index];
        
        public Parameter this[Index Index] => Parameters[Index];

        #endregion

        #region PublicMethods
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

        #endregion

        #region IEnumerable
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<Parameter> GetEnumerator()
        {
            return Parameters.Enumerate();
        }

        #endregion
    }
}