using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using Zion.Vectors;
using Vector2 = Zion.Vectors.Vector2;
using Vector3 = Zion.Vectors.Vector3;

namespace Zion.Serialization.ADF
{
    public abstract class BaseADFWriter : IDisposable
    {
        #region Data
        private protected readonly ADFWritingContext Context;
        private protected readonly StreamGroup Data;

        #endregion

        #region Properties
        protected ADFWritingOptions Options => Context.Options;

        private protected WritableRegistries   Registries => Context.Registries;
        private protected TypeAssociation TypeAssociation => Context.TypeAssociation;
        private protected WriteStrategies WriteStrategies => Context.WriteStrategies;

        private protected ReferenceIdsRegistry References => Registries.References;
        private protected DataRegistry       DataRegistry => Registries.DataRegistry;
        private protected StringIdRegistry StringRegistry => Registries.StringRegistry;
        private protected FormatRegistry   FormatRegistry => Registries.FormatRegistry;

        public bool IsDisposed { get; private set; }

        public long TotalLength => Data.Length;

        public int CurrentPosition => Data.BaseStream.Position;

        #endregion

        #region Constructors
        internal BaseADFWriter(BaseADFWriter Base) : this(Base.Context) { }

        internal BaseADFWriter(BaseADFWriter Base, StreamGroup Target) : this(Base.Context, Target) { }

        internal BaseADFWriter(ADFWritingContext Context) : this(Context, new(Context?.Arena?.GetStream(1)!)) { }

        internal BaseADFWriter(ADFWritingContext Context, StreamGroup Target)
        {
            this.Context = Context.NotNull();
            this.Data = Target;
        }

        #endregion

        #region PublicMethods
        public void Flush(Stream Destination)
        {
            foreach (var Stream in Data)
            {
                Stream.CopyTo(Destination);
            }
        }

        #endregion

        #region ProtectedMethods
        protected ArenaStream GetNewStream()
        {
            return Context.Arena.GetStream(1);
        }

        protected ArenaStream GetBaseStream()
        {
            return Data.BaseStream;
        }

        protected uint GetOrAddDeferred(Type Type)
        {
            return TypeAssociation.GetOrAddDeferred(Type, FormatRegistry);
        }

        protected void ThrowIfDisposed()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(nameof(BaseADFWriter));
            }
        }

        #endregion

        #region Writing
        #region Primitives
        public void Write(string Name, bool Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Boolean, ADFPrimitives.WriteBoolean);
        }

        public void Write(string Name, byte Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Byte, ADFPrimitives.WriteByte);
        }

        public void Write(string Name, sbyte Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.SByte, ADFPrimitives.WriteSByte);
        }


        public void Write(string Name, short Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Int16, ADFPrimitives.WriteInt16);
        }

        public void Write(string Name, int Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Int32, ADFPrimitives.WriteInt32);
        }

        public void Write(string Name, long Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Int64, ADFPrimitives.WriteInt64);
        }

        public void Write(string Name, ushort Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.UInt16, ADFPrimitives.WriteUInt16);
        }

        public void Write(string Name, uint Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.UInt32, ADFPrimitives.WriteUInt32);
        }

        public void Write(string Name, ulong Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.UInt64, ADFPrimitives.WriteUInt64);
        }


        public void Write(string Name, char Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Char, ADFPrimitives.WriteChar);
        }

        public void Write(string Name, float Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Single, ADFPrimitives.WriteSingle);
        }

        public void Write(string Name, double Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Double, ADFPrimitives.WriteDouble);
        }

        public void Write(string Name, decimal Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Decimal, ADFPrimitives.WriteDecimal);
        }

        public void Write(string Name, string Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.String, ADFPrimitives.WriteString);
        }


        public void Write(string Name, Half Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Half, ADFPrimitives.WriteHalf);
        }

        public void Write(string Name, Index Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Index, ADFPrimitives.WriteIndex);
        }

        public void Write(string Name, Range Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Range, ADFPrimitives.WriteRange);
        }

        public void Write(string Name, BigInteger Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.BigInteger, ADFPrimitives.WriteBigInteger);
        }


        public void Write(string Name, RGBColor Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.RGB, ADFPrimitives.WriteRGB);
        }

        public void Write(string Name, RGBAColor Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.RGBA, ADFPrimitives.WriteRGBA);
        }


        public void Write(string Name, Vector2 Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Vector2, ADFPrimitives.WriteVector2);
        }

        public void Write(string Name, Vector2Int Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Vector2Int, ADFPrimitives.WriteVector2Int);
        }

        public void Write(string Name, Vector3 Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Vector3, ADFPrimitives.WriteVector3);
        }

        public void Write(string Name, Vector3Int Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Vector3Int, ADFPrimitives.WriteVector3Int);
        }


        private bool TryWritePrimitive<T>(string Name, T Value)
        {
            if (Value is null)
            {
                return false;
            }

            switch (Value)
            {
                case bool       V: Write(Name, V); return true;
                case byte       V: Write(Name, V); return true;
                case sbyte      V: Write(Name, V); return true;

                case short      V: Write(Name, V); return true;
                case ushort     V: Write(Name, V); return true;
                case int        V: Write(Name, V); return true;
                case uint       V: Write(Name, V); return true;
                case long       V: Write(Name, V); return true;
                case ulong      V: Write(Name, V); return true;

                case char       V: Write(Name, V); return true;
                case float      V: Write(Name, V); return true;
                case double     V: Write(Name, V); return true;
                case decimal    V: Write(Name, V); return true;
                case string     V: Write(Name, V); return true;

                case Half       V: Write(Name, V); return true;
                case Index      V: Write(Name, V); return true;
                case Range      V: Write(Name, V); return true;
                case BigInteger V: Write(Name, V); return true;

                case RGBColor   V: Write(Name, V); return true;
                case RGBAColor  V: Write(Name, V); return true;

                case Vector2    V: Write(Name, V); return true;
                case Vector3    V: Write(Name, V); return true;
                case Vector2Int V: Write(Name, V); return true;
                case Vector3Int V: Write(Name, V); return true;

                default: return false;
            }
        }

        private void WritePrimitive<T>(string Name, in T Value, uint FormatId, AutoWriter<T> Write)
        {
            ThrowIfDisposed();

            var NameId = StringRegistry.GetOrAdd(Name.NotNull());
            var Target = GetStreamGroup(Name, in NameId, in FormatId);

            Write(Context, Target, Value);
            OnWrited(Name, in NameId, in FormatId);
        }

        #endregion

        #region Objects
        public void Write<T>(string Name, T? Value)
        {
            ThrowIfDisposed();

            if (TryWritePrimitive(Name, Value))
            {
                return;
            }

            var NameId = StringRegistry.GetOrAdd(Name.NotNull());

            if (Value is null)
            {
                if (!Options.CanWriteNull)
                {
                    throw new ADFObjectIsNullException(Name);
                }

                var NullFormatId = GetOrAddDeferred(typeof(T));

                GetStreamGroup(Name, in NameId, NullFormatId).BaseStream.WriteCompressedZero(Context);
                OnWrited(Name, in NameId, NullFormatId);
                return;
            }

            var Type = Value.GetType();

            if (TryWriteEnum(Name, in NameId, Type, Value))
            {
                return;
            };
            
            WriteStrategies.GetEntry<T>(Context, Type).Deconstruct
            (
                out var FormatId,
                out var Strategy
            );

            var Target = GetStreamGroup(Name, in NameId, in FormatId);

            Strategy.Write(Context, Target, Value);
            OnWrited(Name, in NameId, in FormatId);
        }


        private bool TryWriteEnum<T>(string Name, in uint NameId, Type Type, T Value)
        {
            if (Type.IsEnum)
            {
                var FormatId = FormatRegistry.Add(DataFormat.GetEnumFormat<T>());
                var Stream = GetStreamGroup(Name, in NameId, in FormatId).BaseStream;

                WriteEnum(Stream, Value);
                OnWrited(Name, in NameId, in FormatId);

                return true;
            }
            return false;
        }

        private void WriteEnum<T>(ArenaStream Stream, T Value)
        {
            Stream.UseSpan
            (
                Unsafe.SizeOf<T>(),
                Span =>
                {
                    Unsafe.WriteUnaligned(ref Span[0], Value);
                }
            );
        }

        #endregion

        #region Sequences
        #region Array
        public void Write(string Name, bool[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, byte[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, sbyte[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, short[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, int[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, long[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, ushort[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, uint[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, ulong[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, char[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, float[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, double[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, decimal[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, string[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, Half[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, Index[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, Range[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, BigInteger[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, RGBColor[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, RGBAColor[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, Vector2[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, Vector2Int[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, Vector3[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, Vector3Int[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }


        public void Write(string Name, bool[] Array, int Count)
        {
            Write(Name, Array, Array.AsSpan(0, Count));
        }

        public void Write(string Name, byte[] Array, int Count)
        {
            Write(Name, Array, Array.AsSpan(0, Count));
        }

        public void Write(string Name, sbyte[] Array, int Count)
        {
            Write(Name, Array, Array.AsSpan(0, Count));
        }

        public void Write(string Name, short[] Array, int Count)
        {
            Write(Name, Array, Array.AsSpan(0, Count));
        }

        public void Write(string Name, int[] Array, int Count)
        {
            Write(Name, Array, Array.AsSpan(0, Count));
        }

        public void Write(string Name, long[] Array, int Count)
        {
            Write(Name, Array, Array.AsSpan(0, Count));
        }

        public void Write(string Name, ushort[] Array, int Count)
        {
            Write(Name, Array, Array.AsSpan(0, Count));
        }

        public void Write(string Name, uint[] Array, int Count)
        {
            Write(Name, Array, Array.AsSpan(0, Count));
        }

        public void Write(string Name, ulong[] Array, int Count)
        {
            Write(Name, Array, Array.AsSpan(0, Count));
        }

        public void Write(string Name, char[] Array, int Count)
        {
            Write(Name, Array, Array.AsSpan(0, Count));
        }

        public void Write(string Name, float[] Array, int Count)
        {
            Write(Name, Array, Array.AsSpan(0, Count));
        }

        public void Write(string Name, double[] Array, int Count)
        {
            Write(Name, Array, Array.AsSpan(0, Count));
        }

        public void Write(string Name, decimal[] Array, int Count)
        {
            Write(Name, Array, Array.AsSpan(0, Count));
        }

        public void Write(string Name, string[] Array, int Count)
        {
            Write(Name, Array, Array.AsSpan(0, Count));
        }

        public void Write(string Name, Half[] Array, int Count)
        {
            Write(Name, Array, Array.AsSpan(0, Count));
        }

        public void Write(string Name, Index[] Array, int Count)
        {
            Write(Name, Array, Array.AsSpan(0, Count));
        }

        public void Write(string Name, Range[] Array, int Count)
        {
            Write(Name, Array, Array.AsSpan(0, Count));
        }

        public void Write(string Name, BigInteger[] Array, int Count)
        {
            Write(Name, Array, Array.AsSpan(0, Count));
        }

        public void Write(string Name, RGBColor[] Array, int Count)
        {
            Write(Name, Array, Array.AsSpan(0, Count));
        }

        public void Write(string Name, RGBAColor[] Array, int Count)
        {
            Write(Name, Array, Array.AsSpan(0, Count));
        }

        public void Write(string Name, Vector2[] Array, int Count)
        {
            Write(Name, Array, Array.AsSpan(0, Count));
        }

        public void Write(string Name, Vector2Int[] Array, int Count)
        {
            Write(Name, Array, Array.AsSpan(0, Count));
        }

        public void Write(string Name, Vector3[] Array, int Count)
        {
            Write(Name, Array, Array.AsSpan(0, Count));
        }

        public void Write(string Name, Vector3Int[] Array, int Count)
        {
            Write(Name, Array, Array.AsSpan(0, Count));
        }


        public void Write(string Name, bool[] Array, int Start, int Count)
        {
            Write(Name, Array, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, byte[] Array, int Start, int Count)
        {
            Write(Name, Array, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, sbyte[] Array, int Start, int Count)
        {
            Write(Name, Array, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, short[] Array, int Start, int Count)
        {
            Write(Name, Array, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, int[] Array, int Start, int Count)
        {
            Write(Name, Array, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, long[] Array, int Start, int Count)
        {
            Write(Name, Array, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, ushort[] Array, int Start, int Count)
        {
            Write(Name, Array, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, uint[] Array, int Start, int Count)
        {
            Write(Name, Array, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, ulong[] Array, int Start, int Count)
        {
            Write(Name, Array, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, char[] Array, int Start, int Count)
        {
            Write(Name, Array, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, float[] Array, int Start, int Count)
        {
            Write(Name, Array, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, double[] Array, int Start, int Count)
        {
            Write(Name, Array, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, decimal[] Array, int Start, int Count)
        {
            Write(Name, Array, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, string[] Array, int Start, int Count)
        {
            Write(Name, Array, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, Half[] Array, int Start, int Count)
        {
            Write(Name, Array, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, Index[] Array, int Start, int Count)
        {
            Write(Name, Array, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, Range[] Array, int Start, int Count)
        {
            Write(Name, Array, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, BigInteger[] Array, int Start, int Count)
        {
            Write(Name, Array, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, RGBColor[] Array, int Start, int Count)
        {
            Write(Name, Array, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, RGBAColor[] Array, int Start, int Count)
        {
            Write(Name, Array, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, Vector2[] Array, int Start, int Count)
        {
            Write(Name, Array, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, Vector2Int[] Array, int Start, int Count)
        {
            Write(Name, Array, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, Vector3[] Array, int Start, int Count)
        {
            Write(Name, Array, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, Vector3Int[] Array, int Start, int Count)
        {
            Write(Name, Array, Array.AsSpan(Start, Count));
        }

        #endregion

        #region List
        public void Write(string Name, List<bool> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<byte> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<sbyte> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<short> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<int> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<long> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<ushort> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<uint> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<ulong> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<char> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<float> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<double> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<decimal> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<string> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<Half> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<Index> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<Range> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<BigInteger> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<RGBColor> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<RGBAColor> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<Vector2> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<Vector2Int> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<Vector3> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<Vector3Int> List)
        {
            Write(Name, List, List.AsSpan());
        }


        public void Write(string Name, List<bool> List, int Count)
        {
            Write(Name, List, List.AsSpan(0, Count));
        }

        public void Write(string Name, List<byte> List, int Count)
        {
            Write(Name, List, List.AsSpan(0, Count));
        }

        public void Write(string Name, List<sbyte> List, int Count)
        {
            Write(Name, List, List.AsSpan(0, Count));
        }

        public void Write(string Name, List<short> List, int Count)
        {
            Write(Name, List, List.AsSpan(0, Count));
        }

        public void Write(string Name, List<int> List, int Count)
        {
            Write(Name, List, List.AsSpan(0, Count));
        }

        public void Write(string Name, List<long> List, int Count)
        {
            Write(Name, List, List.AsSpan(0, Count));
        }

        public void Write(string Name, List<ushort> List, int Count)
        {
            Write(Name, List, List.AsSpan(0, Count));
        }

        public void Write(string Name, List<uint> List, int Count)
        {
            Write(Name, List, List.AsSpan(0, Count));
        }

        public void Write(string Name, List<ulong> List, int Count)
        {
            Write(Name, List, List.AsSpan(0, Count));
        }

        public void Write(string Name, List<char> List, int Count)
        {
            Write(Name, List, List.AsSpan(0, Count));
        }

        public void Write(string Name, List<float> List, int Count)
        {
            Write(Name, List, List.AsSpan(0, Count));
        }

        public void Write(string Name, List<double> List, int Count)
        {
            Write(Name, List, List.AsSpan(0, Count));
        }

        public void Write(string Name, List<decimal> List, int Count)
        {
            Write(Name, List, List.AsSpan(0, Count));
        }

        public void Write(string Name, List<string> List, int Count)
        {
            Write(Name, List, List.AsSpan(0, Count));
        }

        public void Write(string Name, List<Half> List, int Count)
        {
            Write(Name, List, List.AsSpan(0, Count));
        }

        public void Write(string Name, List<Index> List, int Count)
        {
            Write(Name, List, List.AsSpan(0, Count));
        }

        public void Write(string Name, List<Range> List, int Count)
        {
            Write(Name, List, List.AsSpan(0, Count));
        }

        public void Write(string Name, List<BigInteger> List, int Count)
        {
            Write(Name, List, List.AsSpan(0, Count));
        }

        public void Write(string Name, List<RGBColor> List, int Count)
        {
            Write(Name, List, List.AsSpan(0, Count));
        }

        public void Write(string Name, List<RGBAColor> List, int Count)
        {
            Write(Name, List, List.AsSpan(0, Count));
        }

        public void Write(string Name, List<Vector2> List, int Count)
        {
            Write(Name, List, List.AsSpan(0, Count));
        }

        public void Write(string Name, List<Vector2Int> List, int Count)
        {
            Write(Name, List, List.AsSpan(0, Count));
        }

        public void Write(string Name, List<Vector3> List, int Count)
        {
            Write(Name, List, List.AsSpan(0, Count));
        }

        public void Write(string Name, List<Vector3Int> List, int Count)
        {
            Write(Name, List, List.AsSpan(0, Count));
        }


        public void Write(string Name, List<bool> List, int Start, int Count)
        {
            Write(Name, List, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<byte> List, int Start, int Count)
        {
            Write(Name, List, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<sbyte> List, int Start, int Count)
        {
            Write(Name, List, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<short> List, int Start, int Count)
        {
            Write(Name, List, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<int> List, int Start, int Count)
        {
            Write(Name, List, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<long> List, int Start, int Count)
        {
            Write(Name, List, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<ushort> List, int Start, int Count)
        {
            Write(Name, List, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<uint> List, int Start, int Count)
        {
            Write(Name, List, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<ulong> List, int Start, int Count)
        {
            Write(Name, List, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<char> List, int Start, int Count)
        {
            Write(Name, List, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<float> List, int Start, int Count)
        {
            Write(Name, List, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<double> List, int Start, int Count)
        {
            Write(Name, List, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<decimal> List, int Start, int Count)
        {
            Write(Name, List, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<string> List, int Start, int Count)
        {
            Write(Name, List, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<Half> List, int Start, int Count)
        {
            Write(Name, List, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<Index> List, int Start, int Count)
        {
            Write(Name, List, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<Range> List, int Start, int Count)
        {
            Write(Name, List, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<BigInteger> List, int Start, int Count)
        {
            Write(Name, List, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<RGBColor> List, int Start, int Count)
        {
            Write(Name, List, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<RGBAColor> List, int Start, int Count)
        {
            Write(Name, List, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<Vector2> List, int Start, int Count)
        {
            Write(Name, List, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<Vector2Int> List, int Start, int Count)
        {
            Write(Name, List, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<Vector3> List, int Start, int Count)
        {
            Write(Name, List, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<Vector3Int> List, int Start, int Count)
        {
            Write(Name, List, List.AsSpan(Start, Count));
        }

        #endregion

        #region Span
        public void Write(string Name, ReadOnlySpan<bool> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<byte> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<sbyte> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<short> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<int> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<long> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<ushort> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<uint> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<ulong> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<char> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<float> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<double> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<decimal> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<string> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<Half> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<Index> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<Range> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<BigInteger> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<RGBColor> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<RGBAColor> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<Vector2> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<Vector2Int> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<Vector3> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<Vector3Int> Span)
        {
            Write(Name, null, Span);
        }


        private void Write(string Name, object? Collection, ReadOnlySpan<bool> Span)
        {
            WritePrimitives<bool>
            (
                new
                (
                    Name, ADFPrimitives.Boolean,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {

                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<byte> Span)
        {
            WritePrimitives<byte>
            (
                new
                (
                    Name, ADFPrimitives.Byte,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {

                        }
                        else
                        {

                        }
                        //TODO: Write Span<byte>
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<sbyte> Span)
        {
            WritePrimitives<sbyte>
            (
                new
                (
                    Name, ADFPrimitives.SByte,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {

                        }
                        else
                        {

                        }
                        //TODO: Write Span<sbyte>
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<short> Span)
        {
            WritePrimitives<short>
            (
                new
                (
                    Name, ADFPrimitives.Int16,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {

                        }
                        else
                        {

                        }
                        //TODO: Write Span<short>
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<int> Span)
        {
            WritePrimitives<int>
            (
                new
                (
                    Name, ADFPrimitives.Int32,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {

                        }
                        else
                        {

                        }
                        //TODO: Write Span<int>
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<long> Span)
        {
            WritePrimitives<long>
            (
                new
                (
                    Name, ADFPrimitives.Int64,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {

                        }
                        else
                        {

                        }
                        //TODO: Write Span<long>
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<ushort> Span)
        {
            WritePrimitives<ushort>
            (
                new
                (
                    Name, ADFPrimitives.UInt16,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {

                        }
                        else
                        {

                        }
                        //TODO: Write Span<ushort>
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<uint> Span)
        {
            WritePrimitives<uint>
            (
                new
                (
                    Name, ADFPrimitives.UInt32,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {

                        }
                        else
                        {

                        }
                        //TODO: Write Span<uint>
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<ulong> Span)
        {
            WritePrimitives<ulong>
            (
                new
                (
                    Name, ADFPrimitives.UInt64,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {

                        }
                        else
                        {

                        }
                        //TODO: Write Span<ulong>
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<char> Span)
        {
            WritePrimitives<char>
            (
                new
                (
                    Name, ADFPrimitives.Char,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {

                        }
                        else
                        {

                        }
                        //TODO: Write Span<char>
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<float> Span)
        {
            WritePrimitives<float>
            (
                new
                (
                    Name, ADFPrimitives.Single,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {

                        }
                        else
                        {

                        }
                        //TODO: Write Span<float>
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<double> Span)
        {
            WritePrimitives<double>
            (
                new
                (
                    Name, ADFPrimitives.Double,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {

                        }
                        else
                        {

                        }
                        //TODO: Write Span<double>
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<decimal> Span)
        {
            WritePrimitives<decimal>
            (
                new
                (
                    Name, ADFPrimitives.Decimal,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {

                        }
                        else
                        {

                        }
                        //TODO: Write Span<decimal>
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<string> Span)
        {
            WritePrimitives<string>
            (
                new
                (
                    Name, ADFPrimitives.String,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {

                        }
                        else
                        {

                        }
                        //TODO: Write Span<string>
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<Half> Span)
        {
            WritePrimitives<Half>
            (
                new
                (
                    Name, ADFPrimitives.Half,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {

                        }
                        else
                        {

                        }
                        //TODO: Write Span<Half>
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<Index> Span)
        {
            WritePrimitives<Index>
            (
                new
                (
                    Name, ADFPrimitives.Index,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {

                        }
                        else
                        {

                        }
                        //TODO: Write Span<Index>
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<Range> Span)
        {
            WritePrimitives<Range>
            (
                new
                (
                    Name, ADFPrimitives.Range,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {

                        }
                        else
                        {

                        }
                        //TODO: Write Span<Range>
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<BigInteger> Span)
        {
            WritePrimitives<BigInteger>
            (
                new
                (
                    Name, ADFPrimitives.BigInteger,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {

                        }
                        else
                        {

                        }
                        //TODO: Write Span<BigInteger>
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<RGBColor> Span)
        {
            WritePrimitives<RGBColor>
            (
                new
                (
                    Name, ADFPrimitives.RGB,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {

                        }
                        else
                        {

                        }
                        //TODO: Write Span<RGBColor>
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<RGBAColor> Span)
        {
            WritePrimitives<RGBAColor>
            (
                new
                (
                    Name, ADFPrimitives.RGBA,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {

                        }
                        else
                        {

                        }
                        //TODO: Write Span<RGBAColor>
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<Vector2> Span)
        {
            WritePrimitives<Vector2>
            (
                new
                (
                    Name, ADFPrimitives.Vector2,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {

                        }
                        else
                        {

                        }
                        //TODO: Write Span<Vector2>
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<Vector2Int> Span)
        {
            WritePrimitives<Vector2Int>
            (
                new
                (
                    Name, ADFPrimitives.Vector2Int,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {

                        }
                        else
                        {

                        }
                        //TODO: Write Span<Vector2Int>
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<Vector3> Span)
        {
            WritePrimitives<Vector3>
            (
                new
                (
                    Name, ADFPrimitives.Vector3,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {

                        }
                        else
                        {

                        }
                        //TODO: Write Span<Vector3>
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<Vector3Int> Span)
        {
            WritePrimitives<Vector3Int>
            (
                new
                (
                    Name, ADFPrimitives.Vector3Int,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {

                        }
                        else
                        {

                        }
                        //TODO: Write Span<Vector3Int>
                    }
                )
            );
        }


        private void WritePrimitives<T>(CollectionWriteContext<T> WriteContext)
        {
            ThrowIfDisposed();

            var NameId = StringRegistry.GetOrAdd(WriteContext.Name);
            var Target = GetStreamGroup(WriteContext.Name, in NameId, in WriteContext.FormatId);

            if (WriteContext.Collection is not null
                && References.TryGetReference(WriteContext.Collection, out var Reference))
            {
                Target.BaseStream.WriteCompressed(Context, Reference.Id);
            }
            else
            {
                WriteContext.Write(Context, Target);
            }

            OnWrited(WriteContext.Name, in NameId, in WriteContext.FormatId);
        }

        #endregion

        #region IEnumerable

        #endregion

        #endregion

        #endregion

        #region AbstractMethods
        protected virtual StreamGroup GetStreamGroup(string Name, in uint NameId, in uint FormatId) => Data;

        protected virtual void OnWrited(string Name, in uint NameId, in uint FormatId) { }

        protected virtual void OnDisposed() { }

        #endregion

        #region IDisposable
        public void Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                Context.CurrentPosition += Data.Length;
                OnDisposed();
            }
        }

        #endregion

        #region PrivateMethods
        private static bool IsRootType(Type Type)
        {
            if (Type.IsValueType)
            {
                return true;
            }
            var BaseType = Type.BaseType;
            return BaseType is null || BaseType == typeof(object);
        }

        private static IEnumerable<Type> EnumerateHierarchy(Type Type)
        {
            if (!IsRootType(Type))
            {
                foreach (var Hierarchy in EnumerateHierarchy(Type.BaseType!))
                {
                    yield return Hierarchy;
                }
            }

            yield return Type;
        }

        #endregion
    }
}