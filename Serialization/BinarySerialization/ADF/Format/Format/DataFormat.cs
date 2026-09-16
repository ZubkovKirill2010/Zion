using System.Collections;
using System.Runtime.CompilerServices;

namespace Zion.Serialization.ADF
{
    public readonly struct DataFormat : IEnumerable<Parameter>
    {
        #region Data
        private readonly Parameter[] Parameters;
        private readonly uint[] Generics = [];

        public readonly FormatFlags Flags;
        public readonly uint BaseFormat;

        #endregion

        #region Properties
        public bool IsArray     => Flags.HasFlag(FormatFlags.IsArray);
        public bool IsReference => Flags.HasFlag(FormatFlags.IsReference);
        public bool IsAbstract  => Flags.HasFlag(FormatFlags.IsAbstract);
        public bool IsNullable  => Flags.HasFlag(FormatFlags.IsNullable);
        public bool IsEnum      => Flags.HasFlag(FormatFlags.IsEnum);
        public bool IsGenerated => Flags.HasFlag(FormatFlags.IsGenerated);

        public bool IsGeneric   => Generics.Length > 0;

        public int ParametersCount => Parameters.Length;
        public int GenericsCount   => Generics.Length;

        #endregion

        #region Constructors
        public DataFormat(IEnumerable<Parameter> Parameters, FormatFlags Flags)
        {
            this.Parameters = Parameters.NotNull().ToArray();
            this.Flags = Flags;
        }

        public DataFormat(IEnumerable<Parameter> Parameters, FormatFlags Flags, uint BaseFormat)
            : this(Parameters, Flags)
        {
            this.BaseFormat = BaseFormat;
        }

        public DataFormat(IEnumerable<Parameter> Parameters, IEnumerable<uint> Generics, FormatFlags Flags, uint BaseFormat)
            : this(Parameters, Flags, BaseFormat)
        {
            this.Generics = Generics.NotNull().ToArray();
        }

        #endregion

        #region Indexers
        public Parameter this[int   Index] => Parameters[Index];
        
        public Parameter this[Index Index] => Parameters[Index];

        #endregion

        #region PublicMethods
        public static DataFormat GetEnumFormat<T>()
        {
            if (!typeof(T).IsEnum)
            {
                throw new InvalidCastException("T is not Enum");
            }

            return new DataFormat
            (
                [],
                Unsafe.SizeOf<T>() switch
                {
                    1 => FormatFlags.IsEnum8,
                    2 => FormatFlags.IsEnum16,
                    4 => FormatFlags.IsEnum32,
                    8 => FormatFlags.IsEnum64,
                    _ => throw new Exception()
                }
            );
        }

        public static int GetEnumSize(FormatFlags Flags)
        {
            return ((int)Flags & 0b11000000) << 3;
        }


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