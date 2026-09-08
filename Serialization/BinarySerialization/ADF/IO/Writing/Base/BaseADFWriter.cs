using System.Numerics;
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
        private readonly Arena<byte> Arena;

        protected readonly ADFWritingOptions  Options;
        protected readonly WritableRegistries Registries;

        private StreamGroup Data;

        private int ChildPosition = 0;

        #endregion

        #region Properties
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
        internal BaseADFWriter(Arena<byte> Arenas, ADFWritingOptions? WriteOptions)
        {
            Options    = WriteOptions ?? ADFWritingOptions.Default;
            Arena      = Arenas.NotNull();
            Registries = new();
            Data       = new(Arena.GetStream(64));
        }

        internal BaseADFWriter(BaseADFWriter Base)
            : this(Base, Base.Data.BaseStream) { { } }

        internal BaseADFWriter(BaseADFWriter Base, ArenaStream BaseStream)
        {
            if (!BaseStream.NotNull().IsFrom(Base.Arena))
            {
                throw new ArgumentException("The stream must be from the transferred Arena");
            }

            Options    = Base.Options;
            Arena      = Base.Arena;
            Registries = Base.Registries;
            Data       = new(BaseStream);
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
            return Arena.GetStream(Size);
        }

        protected ArenaStream GetBaseStream()
        {
            return Data.BaseStream;
        }

        protected void AddChild(ArenaStream Stream)
        {
            Data = Data.Add(new(Stream));
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
            var Type   = typeof(T);
            
            if (TypeAssociation.TryGetFormatId(Type, out uint FormatId))
            {
                var Format = FormatRegistry[FormatId];
                var Stream = GetStream(Name, in NameId, in FormatId);
                var IsNull = false;
                
                void WriteNullReference()
                {
                    if (Options.Compression)
                    {
                        Stream.Write((byte)0);
                    }
                    else
                    {
                        Stream.Write(0u);
                    }
                }

                if (Value is null)
                {
                    IsNull = true;
                    if (!Format.IsNullable)
                    {
                        throw new ADFObjectIsNullException(Name);
                    }
                }

                if (Format.IsReference)
                {
                    if (IsNull)
                    {
                        WriteNullReference();
                        OnWrited(Name, in NameId, in FormatId);
                        return;
                    }
                    WriteExistingClass(Name, NameId, Format, Stream, Value);
                    OnWrited(Name, in NameId, in FormatId);
                }
                else
                {
                    if (IsNull)
                    {
                        Stream.Write(false);
                        OnWrited(Name, in NameId, in FormatId);
                        return;
                    }
                    if (Format.IsNullable)
                    {
                        Stream.Write(true);
                    }
                    WriteExistingStruct(Name, NameId, FormatId, Format, Stream, Value);
                    OnWrited(Name, in NameId, in FormatId);
                }
            }
            else
            {
                if (Value is null && !CanWriteNull())
                {
                    throw new ADFObjectIsNullException(Name);
                }

                var Stream = GetStream(Name, in NameId, in FormatId);

                if (Type.IsValueType)
                {
                    WriteNewStruct(Name, NameId, Stream, Value);
                }
                else
                {
                    WriteNewClass(Name, NameId, Stream, Value);
                }
            }
        }


        private void WriteExistingStruct<T>(string Name, uint NameId, uint FormatId, DataFormat Format, ArenaStream Stream, T Value)
        {
            //TODO: WriteExistingStruct
        }

        private void WriteExistingClass<T>(string Name, uint NameId, DataFormat? Format, ArenaStream Stream, T Value)
        {
            //TODO: WriteExistingClass
        }

        private void WriteNewStruct<T>(string Name, uint NameId, ArenaStream Stream, T Value)
        {
            //TODO: WriteNewStruct
        }

        private void WriteNewClass<T>(string Name, uint NameId, ArenaStream Stream, T Value)
        {
            //TODO: WriteNewClass
        }

        #endregion

        #region Sequences

        #endregion

        #endregion

        #region AbstractMethods
        protected virtual ArenaStream GetStream(string Name, in uint NameId, in uint FormatId) => Data.BaseStream;

        protected virtual void OnDisposed() { }

        protected abstract bool CanWriteNull();

        protected abstract void OnWrited(string Name, in uint NameId, in uint FormatId);
        
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
            var Stream = Arena.GetStream(0);            
            Stream.Write(Value);
            AddChild(Stream);
        }

        #endregion
    }
}