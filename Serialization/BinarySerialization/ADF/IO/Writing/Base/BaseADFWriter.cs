using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Zion.Vectors;
using Vector2 = Zion.Vectors.Vector2;
using Vector3 = Zion.Vectors.Vector3;

namespace Zion.Serialization.ADF
{
    public abstract class BaseADFWriter : IDisposable
    {
        #region Delegates
        protected delegate void WriteAction<T>(ArenaStream Stream, T Value);

        #endregion

        #region Data
        protected readonly ADFWritingContext Context;

        private StreamGroup Data;

        private int ChildPosition = 0;

        #endregion

        #region Properties

        protected ADFWritingOptions     Options => Context.Options;
        protected WritableRegistries Registries => Context.Registries;

        protected TypeAssociation TypeAssociation => Registries.TypeAssociation;
        protected ReferenceIdsRegistry References => Registries.References;
        protected DataRegistry       DataRegistry => Registries.DataRegistry;
        protected StringIdRegistry StringRegistry => Registries.StringRegistry;
        protected FormatIdRegistry FormatRegistry => Registries.FormatRegistry;

        public bool IsDisposed { get; private set; }

        public long TotalLength => Data.Length;

        public int CurrentPosition => Data.BaseStream.Position;

        #endregion

        #region Constructors
        internal BaseADFWriter(BaseADFWriter Base) : this(Base.Context) { }

        internal BaseADFWriter(BaseADFWriter Base, ArenaStream BaseStream) : this(Base.Context, BaseStream) { }

        internal BaseADFWriter(ADFWritingContext Context) : this(Context, Context?.Arena?.GetStream(64)!) { }

        internal BaseADFWriter(ADFWritingContext Context, ArenaStream BaseStream)
        {
            this.Context = Context.NotNull();
            this.Data = new(BaseStream.NotNull());
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
        protected ArenaStream GetNewStream(int Size)
        {
            return Context.Arena.GetStream(Size);
        }

        protected ArenaStream GetBaseStream()
        {
            return Data.BaseStream;
        }

        protected void AddChild(ArenaStream Stream)
        {
            Data = Data.Add(new(Stream));
            ChildPosition += Stream.Length;
        }

        protected void AddChild(StreamGroup Group)
        {
            Data = Data.Add(Group);
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
            WritePrimitive
            (
                Name, ADFPrimitives.Boolean, in Value,
                static (S, V) => S.Write(V)
            );
        }

        public void Write(string Name, byte Value)
        {
            WritePrimitive
            (
                Name, ADFPrimitives.Byte, in Value,
                static (S, V) => S.Write(V)
            );
        }

        public void Write(string Name, sbyte Value)
        {
            WritePrimitive
            (
                Name, ADFPrimitives.SByte, in Value,
                static (S, V) => S.Write(V)
            );
        }


        public void Write(string Name, short Value)
        {
            WritePrimitive
            (
                Name, ADFPrimitives.Int16, in Value,
                static (S, V) => S.Write(V)
            );
        }

        public void Write(string Name, int Value)
        {
            WritePrimitive
            (
                Name, ADFPrimitives.Int32, in Value,
                static (S, V) => S.Write(V),
                static (S, V) => S.Write7BitEncodedInt(V)
            );
        }

        public void Write(string Name, long Value)
        {
            WritePrimitive
            (
                Name, ADFPrimitives.Int64, in Value,
                static (S, V) => S.Write(V),
                static (S, V) => S.Write7BitEncodedInt64(V)
            );
        }

        public void Write(string Name, ushort Value)
        {
            WritePrimitive
            (
                Name, ADFPrimitives.UInt16, in Value,
                static (S, V) => S.Write(V)        
            );
        }

        public void Write(string Name, uint Value)
        {
            WritePrimitive
            (
                Name, ADFPrimitives.UInt32, in Value,
                static (S, V) => S.Write(V),
                static (S, V) => S.Write7BitEncodedUInt(V)        
            );
        }

        public void Write(string Name, ulong Value)
        {
            WritePrimitive
            (
                Name, ADFPrimitives.UInt64, in Value,
                static (S, V) => S.Write(V),
                static (S, V) => S.Write7BitEncodedUInt64(V)       
            );
        }


        public void Write(string Name, char Value)
        {
            WritePrimitive
            (
                Name, ADFPrimitives.Char, in Value,
                static (S, V) => S.Write(V)        
            );
        }

        public void Write(string Name, float Value)
        {
            WritePrimitive
            (
                Name, ADFPrimitives.Single, in Value,
                static (S, V) => S.Write(V)    
            );
        }

        public void Write(string Name, double Value)
        {
            WritePrimitive
            (
                Name, ADFPrimitives.Double, in Value,
                static (S, V) => S.Write(V)
                //Update: WriteConcise double
            );
        }

        public void Write(string Name, decimal Value)
        {
            WritePrimitive
            (
                Name, ADFPrimitives.Decimal, in Value,
                static (S, V) => S.Write(V)
                //Update: WriteConcise double
            );
        }

        public void Write(string Name, string Value)
        {
            WritePrimitive
            (
                Name, ADFPrimitives.String, in Value,
                (S, V) => S.Write(StringRegistry.GetOrAdd(V)),
                (S, V) => S.Write7BitEncodedUInt(StringRegistry.GetOrAdd(V))
            );
        }


        public void Write(string Name, Half Value)
        {
            WritePrimitive
            (
                Name, ADFPrimitives.Half, in Value,
                static (S, V) => S.Write(V)
            );
        }

        public void Write(string Name, Index Value)
        {
            WritePrimitive
            (
                Name, ADFPrimitives.Index, in Value,
                static (S, V) => S.Write(V),
                static (S, V) => S.Write7BitEncodedIndex(V)
            );
        }

        public void Write(string Name, Range Value)
        {
            WritePrimitive
            (
                Name, ADFPrimitives.Range, in Value,
                static (S, V) => S.Write(V),
                static (S, V) =>
                {
                    S.Write7BitEncodedIndex(V.Start);
                    S.Write7BitEncodedIndex(V.End);
                }
            );
        }

        public void Write(string Name, BigInteger Value)
        {
            WritePrimitive
            (
                Name, ADFPrimitives.BigInteger, in Value,
                (S, V) =>
                {
                    S.Write(ChildPosition);
                    WriteBigIntegerValue(Value);
                },
                (S, V) =>
                {
                    S.Write7BitEncodedInt(ChildPosition);
                    WriteBigIntegerValue(Value);
                }
            );
        }


        public void Write(string Name, RGBColor Value)
        {
            WritePrimitive
            (
                Name, ADFPrimitives.RGB, in Value,
                static (S, V) => S.Write(V)
            );
        }

        public void Write(string Name, RGBAColor Value)
        {
            WritePrimitive
            (
                Name, ADFPrimitives.RGBA, in Value,
                static (S, V) => S.Write(V)
            );
        }


        public void Write(string Name, Vector2 Value)
        {
            WritePrimitive
            (
                Name, ADFPrimitives.Vector2, in Value,
                static (S, V) => S.Write(V)
                //Update: WriteConcise Vector2
            );
        }

        public void Write(string Name, Vector2Int Value)
        {
            WritePrimitive
            (
                Name, ADFPrimitives.Vector2Int, in Value,
                static (S, V) => S.Write(V),
                static (S, V) =>
                {
                    S.Write7BitEncodedInt(V.X);
                    S.Write7BitEncodedInt(V.Y);
                }        
            );
        }

        public void Write(string Name, Vector3 Value)
        {
            WritePrimitive
            (
                Name, ADFPrimitives.Vector3, in Value,
                static (S, V) => S.Write(V)
                //Update: WriteConcise Vector3        
            );
        }

        public void Write(string Name, Vector3Int Value)
        {
            WritePrimitive
            (
                Name, ADFPrimitives.Vector3Int, in Value,
                static (S, V) => S.Write(V),
                static (S, V) =>
                {
                    S.Write7BitEncodedInt(V.X);
                    S.Write7BitEncodedInt(V.Y);
                    S.Write7BitEncodedInt(V.Z);
                }
            );
        }


        private bool TryWritePrimitive<T>(string Name, T Value)
        {
            if (Value is null) { return false; }

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

        private void WritePrimitive<T>(string Name, uint FormatId, in T Value, WriteAction<T> Write)
        {
            WritePrimitive(Name, FormatId, in Value, Write, Write);
        }

        private void WritePrimitive<T>(string Name, in uint FormatId, in T Value, WriteAction<T> WriteFull, WriteAction<T> WriteConcise)
        {
            ThrowIfDisposed();

            var NameId = StringRegistry.GetOrAdd(Name.NotNull());
            var WriteAction = Options.Compression ? WriteConcise : WriteFull;
            var Stream = GetStream(Name, in NameId, in FormatId);

            WriteAction(Stream, Value);
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
                if (!Options.CanHasNull)
                {
                    throw new ADFObjectIsNullException(Name);
                }

                var Stream = GetStreamForNull(Name, in NameId);
                if (Options.Compression)
                {
                    Stream.Write((byte)0);
                }
                else
                {
                    Stream.Write(0u);
                }
                OnNullWrited(Name, in NameId);
            }

            var Type = Value!.GetType();

            if (Type.IsEnum)
            {
                WriteEnum(Name, in NameId, Type, Value);
                return;
            }

            if (!TryWriteExistingObject(Name, in NameId, Type, in Value))
            {
                WriteNewObject(Name, in NameId, Value);
            }
        }


        private bool TryWriteExistingObject<T>(string Name, in uint NameId, Type Type, in T Value)
        {
            if (TypeAssociation.TryGetFormatId(Type, out uint FormatId))
            {
                var Format = FormatRegistry[FormatId];

                if (Format.IsDeferred)
                {
                    return false;
                }

                var Stream = GetStream(Name, in NameId, in FormatId);
                var ParameterType = typeof(T);
                
                if (ParameterType != Type)
                {
                    WriteExistingObject(in FormatId, in Format, Stream, Value);
                    OnWrited(Name, in NameId, in FormatId);
                }
                else
                {
                    var ParameterFormatId = TypeAssociation[ParameterType];

                    WriteCompressedUInt(Stream, FormatId);
                    WriteExistingObject(in FormatId, in Format, Stream, Value);
                    OnWrited(Name, in NameId, in ParameterFormatId);
                }

                return true;
            }
            return false;
        }


        private void WriteEnum<T>(string Name, in uint NameId, Type Type, T Value)
        {
            if (!TypeAssociation.TryGetFormatId(Type, out uint FormatId))
            {
                FormatId = FormatRegistry.Add(DataFormat.GetEnumFormat<T>());
                TypeAssociation.Add(Type, FormatId);
            }

            var Stream = GetStream(Name, in NameId, in FormatId);

            Stream.UseSpan
            (
                Unsafe.SizeOf<T>(),
                Span =>
                {
                    Unsafe.WriteUnaligned(ref Span[0], Value);
                }
            );
        }

        private void WriteExistingObject<T>(in uint FormatId, in DataFormat Format, ArenaStream Stream, T Value)
        {
            var Type = Value!.GetType();

            WriteGenerics(Stream, Type);

            if (IsRootType(Type))
            {
                if (Value is IADFWritable Writable)
                {
                    using var Writer = new ADFCheckingObjectWriter(this, Stream, Format);
                    Writable.Write(Writer);
                }
                else if (ADFSerializer.TryGetSerializer<T>(Value, out var Serializer))
                {
                    using var Writer = new ADFCheckingObjectWriter(this, Stream, Format);
                    Serializer.Write(Writer, Value);
                }
                else
                {
                    var AutoSerializer = AutoADFSerializer.GetWriter<T>(Type);
                    AutoSerializer.Write(Stream, Value);
                }
            }
            else
            {
                foreach (var LayerFormat in FormatRegistry.EnumerateHierarchy(Format))
                {
                    //TODO
                }
            }
        }

        private void WriteNewObject<T>(string Name, in uint NameId, T Value)
        {
            var Type = Value!.GetType();

            if (IsRootType(Type))
            {
                
            }
            else
            {
                //TODO
            }
        }


        private void WriteGenerics(ArenaStream Stream, Type Type)
        {
            if (Type.IsGenericType)
            {
                bool Compression = Options.Compression;

                void Write(uint Id)
                {
                    if (Compression)
                    {
                        Stream.Write7BitEncodedUInt(Id);
                    }
                    else
                    {
                        Stream.Write(Id);
                    }
                }

                foreach (var Generic in Type.GetGenericArguments())
                {
                    if (!TypeAssociation.TryGetFormatId(Generic, out uint GenericFormatId))
                    {
                        GenericFormatId = FormatRegistry.AddDeferred();
                    }
                    Write(GenericFormatId);
                }
            }
        }

        #endregion

        #region Sequences

        #endregion

        #endregion

        #region AbstractMethods
        protected virtual ArenaStream GetStream(string Name, in uint NameId, in uint FormatId) => Data.BaseStream;

        protected virtual ArenaStream GetStreamForNull(string Name, in uint NameId) => Data.BaseStream;

        protected virtual void OnWrited(string Name, in uint NameId, in uint FormatId) { }

        protected virtual void OnNullWrited(string Name, in uint NameId) { }

        protected virtual void OnDisposed() { }

        #endregion

        #region IDisposable
        public void Dispose()
        {
            IsDisposed = true;
            OnDisposed();
        }

        #endregion

        #region PrivateMethods
        private void WriteBigIntegerValue(BigInteger Value)
        {
            var Stream = Context.Arena.GetStream(0);            
            Stream.Write(Value);
            AddChild(Stream);
        }

        private void WriteCompressedUInt(ArenaStream Stream, uint Value)
        {
            if (Options.Compression)
            {
                Stream.Write7BitEncodedUInt(Value);
            }
            else
            {
                Stream.Write(Value);
            }
        }

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
                foreach (var Hierarchy in EnumerateHierarchy(Type.BaseType))
                {
                    yield return Hierarchy;
                }
            }

            yield return Type;
        }

        #endregion
    }
}