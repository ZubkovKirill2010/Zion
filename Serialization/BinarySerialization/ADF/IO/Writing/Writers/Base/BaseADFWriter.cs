using System.Numerics;
using System.Runtime.CompilerServices;
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