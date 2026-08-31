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
        private readonly ArenaStream Stream;
        private readonly List<ADFObjectWriter> References;

        protected readonly ADFWritingOptions Options;
        protected readonly WritableRegistries Registries;

        protected DataRegistry     DataRegistry   => Registries.DataRegistry;
        protected StringIdRegistry StringRegistry => Registries.StringRegistry;
        protected FormatIdRegistry FormatRegistry => Registries.FormatRegistry;

        #endregion

        #region Properties
        public long TotalLength
        {
            get;
            private set
            {
                ArgumentOutOfRangeException.ThrowIfNegative(value);
                field = value;
            }
        } = -1L;

        public bool IsDisposed => TotalLength < 0;

        public int CurrentPosition => Stream.Position;

        #endregion

        #region Constructors
        internal BaseADFWriter(BaseADFWriter Base) : this(Base.Arena)
        {
            Registries = Base.Registries;
            Options = Base.Options;
        }

        internal BaseADFWriter(Arena<byte> Arenas)
        {
            Arena = Arenas.NotNull();
            Stream = Arena.GetStream(64);
            References = new(0);
            Registries = new();
            Options = ADFWritingOptions.Default;
        }

        internal BaseADFWriter(Arena<byte> Arenas, ADFWritingOptions? Options)
            : this(Arenas)
        {
            this.Options = Options ?? ADFWritingOptions.Default;
        }

        #endregion

        #region PublicMethods
        public void Flush(Stream Destination)
        {
            Stream.CopyTo(Destination);
            foreach (ADFObjectWriter Reference in References)
            {
                Reference.Flush(Destination);
            }
        }

        #endregion

        #region Writing
        #region Primitives
        public void Write(string Name, bool Value)
        {
            Write
            (
                Name, ADFPrimitives.Boolean, in Value,
                static (S, V) => S.Write(V)
            );
        }

        public void Write(string Name, byte Value)
        {
            Write
            (
                Name, ADFPrimitives.Byte, in Value,
                static (S, V) => S.Write(V)
            );
        }

        public void Write(string Name, sbyte Value)
        {
            Write
            (
                Name, ADFPrimitives.SByte, in Value,
                static (S, V) => S.Write(V)
            );
        }


        public void Write(string Name, short Value)
        {
            Write
            (
                Name, ADFPrimitives.Int16, in Value,
                static (S, V) => S.Write(V)
            );
        }

        public void Write(string Name, int Value)
        {
            Write
            (
                Name, ADFPrimitives.Int32, in Value,
                static (S, V) => S.Write(V),
                static (S, V) => S.Write7BitEncodedInt(V)
            );
        }

        public void Write(string Name, long Value)
        {
            Write
            (
                Name, ADFPrimitives.Int64, in Value,
                static (S, V) => S.Write(V),
                static (S, V) => S.Write7BitEncodedInt64(V)
            );
        }

        public void Write(string Name, ushort Value)
        {
            Write
            (
                Name, ADFPrimitives.UInt16, in Value,
                static (S, V) => S.Write(V)        
            );
        }

        public void Write(string Name, uint Value)
        {
            Write
            (
                Name, ADFPrimitives.UInt32, in Value,
                static (S, V) => S.Write(V),
                static (S, V) => S.Write7BitEncodedUInt(V)        
            );
        }

        public void Write(string Name, ulong Value)
        {
            Write
            (
                Name, ADFPrimitives.UInt64, in Value,
                static (S, V) => S.Write(V),
                static (S, V) => S.Write7BitEncodedUInt64(V)       
            );
        }


        public void Write(string Name, char Value)
        {
            Write
            (
                Name, ADFPrimitives.Char, in Value,
                static (S, V) => S.Write(V)        
            );
        }

        public void Write(string Name, float Value)
        {
            Write
            (
                Name, ADFPrimitives.Single, in Value,
                static (S, V) => S.Write(V)    
            );
        }

        public void Write(string Name, double Value)
        {
            Write
            (
                Name, ADFPrimitives.Double, in Value,
                static (S, V) => S.Write(V)
                //Update: WriteConcise double
            );
        }

        public void Write(string Name, decimal Value)
        {
            Write
            (
                Name, ADFPrimitives.Decimal, in Value,
                static (S, V) => S.Write(V)
                //Update: WriteConcise double
            );
        }

        public void Write(string Name, string Value)
        {
            Write
            (
                Name, ADFPrimitives.String, in Value,
                (S, V) => S.Write(StringRegistry.GetOrAdd(V)),
                (S, V) => S.Write7BitEncodedUInt(StringRegistry.GetOrAdd(V))
            );
        }


        public void Write(string Name, Half Value)
        {
            Write
            (
                Name, ADFPrimitives.Half, in Value,
                static (S, V) => S.Write(V)
            );
        }

        public void Write(string Name, Index Value)
        {
            Write
            (
                Name, ADFPrimitives.Index, in Value,
                static (S, V) => S.Write(V),
                static (S, V) => S.Write7BitEncodedIndex(V)
            );
        }

        public void Write(string Name, Range Value)
        {
            Write
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
            //TODO: Write BigInteger
            Write
            (
                Name, ADFPrimitives.Range, in Value,
                static (S, V) => throw new NotImplementedException()
            );
        }


        public void Write(string Name, RGBColor Value)
        {
            Write
            (
                Name, ADFPrimitives.RGB, in Value,
                static (S, V) => S.Write(V)
            );
        }

        public void Write(string Name, RGBAColor Value)
        {
            Write
            (
                Name, ADFPrimitives.RGBA, in Value,
                static (S, V) => S.Write(V)
            );
        }


        public void Write(string Name, Vector2 Value)
        {
            Write
            (
                Name, ADFPrimitives.Vector2, in Value,
                static (S, V) => S.Write(V)
                //Update: WriteConcise Vector2
            );
        }

        public void Write(string Name, Vector2Int Value)
        {
            Write
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
            Write
            (
                Name, ADFPrimitives.Vector3, in Value,
                static (S, V) => S.Write(V)
                //Update: WriteConcise Vector3        
            );
        }

        public void Write(string Name, Vector3Int Value)
        {
            Write
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


        private void Write<T>(string Name, uint FormatId, in T Value, WriteAction<T> Write)
        {
            Write<T>(Name, FormatId, in Value, Write, Write);
        }

        private void Write<T>(string Name, uint FormatId, in T Value, WriteAction<T> WriteFull, WriteAction<T> WriteConcise)
        {
            ThrowIfDisposed();
            //TODO
        }

        #endregion

        #endregion

        #region AbstractMethods
        protected virtual ArenaStream GetStream(in uint NameId) => Stream;

        protected abstract void OnWrited(string Name, in uint NameId, in uint FormatId);
        
        protected abstract void OnDisposed();

        #endregion

        #region IDisposable
        public void Dispose()
        {
            long Length = Stream.Length;

            foreach (var Reference in References)
            {
                Length += Reference.TotalLength;
            }

            TotalLength = Length;

            OnDisposed();
        }

        #endregion

        #region PrivateMethods
        private void ThrowIfDisposed()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(nameof(BaseADFWriter));
            }
        }

        #endregion
    }
}