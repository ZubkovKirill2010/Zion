using System.Diagnostics;
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
        private readonly List<ADFObjectWriter> Childs;

        protected readonly ADFWritingOptions Options;
        protected readonly WritableRegistries Registries;

        protected TypeAssociation TypeAssociation => Registries.TypeAssociation;
        protected ReferenceIdsRegistry References => Registries.References;
        protected DataRegistry       DataRegistry => Registries.DataRegistry;
        protected StringIdRegistry StringRegistry => Registries.StringRegistry;
        protected FormatIdRegistry FormatRegistry => Registries.FormatRegistry;

        private int ChildPosition = 0;

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
            Childs = new(0);
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
            foreach (ADFObjectWriter Reference in Childs)
            {
                Reference.Flush(Destination);
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
            //TODO: WritePrimitive BigInteger
            WritePrimitive
            (
                Name, ADFPrimitives.Range, in Value,
                static (S, V) => throw new NotImplementedException()
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
            var Stream = GetStream(in NameId);

            var WriteAction = Options.Compression ? WriteConcise : WriteFull;

            WriteAction(Stream, Value);

            OnWrited(Name, in NameId, in FormatId);
        }

        #endregion

        #region Objects
        public void Write<T>(string Name, T? Value)
        {
            ThrowIfDisposed();

            uint NameId = StringRegistry.GetOrAdd(Name.NotNull());

            if (Value is null)
            {
                var Stream = GetStream(in NameId);
                if (Options.Compression)
                {
                    Stream.Write((byte)0);
                }
                else
                {
                    Stream.Write(0u);
                }
                OnWrited(Name, in NameId, 0u);
                return;
            }

            if (TryWritePrimitive(Name, Value))
            {
                return;
            }

            WriteComplex(Name, in NameId, Value);
        }


        private void WriteComplex<T>(string Name, in uint NameId, T Value)
        {
            //TODO: Записывать в формат typeof(T), писать T (для сохранения абстракции)
            var Type = Value.GetType();
            var Stream = GetStream(in NameId);

            if (Type.IsClass)
            {
                if (References.TryGetReference(Value, out var Reference))
                {
                    Stream.Write(Reference.Id);
                    OnWrited(Name, in NameId, in Reference.Definition.FormatId);
                }
                else
                {
                    //TODO:
                    //Пишем ссылку на новый объект (относительную позицию от конца этой структуры (ChildPosition)
                    //После записи получаем FormatId и вызываем OnWrited
                    //Добавляем ссылку в References
                    //Пишем по приоритетам:
                    //IADFWritable
                    //IADFWriter
                    //TypeSchema
                    //Reflection
                }
                return;
            }
            else
            {
                //TODO:
                //Value - структура.
                //Узнаём как сериализовать структуру и записываем её в тот же поток
                //После записи получаем FormatId и вызываем OnWrited
                //Пишем по приоритетам:
                //IADFWritable
                //IADFWriter
                //TypeSchema
                //Reflection

                Action<ADFObjectWriter>? WriteAction = Value is IADFWritable Writable
                    ? Writable.Write
                    :
                    (
                        ADFSerializer.TryGetWriter(Value, out var Serializer)
                        ? WriteAction = Writer => Serializer.Write(Writer, Value)
                        : null
                    );

                void WriteAuto(ADFObjectWriter Writer)
                {
                    var Serializer = AutoADFSerializer.GetWriter<T>(Type);
                    Serializer.Write(Writer, Value);
                }

                if (TypeAssociation.TryGetFormatId(Type, out uint FormatId))
                {
                    //TODO
                    var CheckingWriter = new ADFCheckingObjectWriter
                    (
                        Stream, // Нужно: писать в тот же поток
                        FormatId, // Нужно: ожидаемый формат
                        Options
                    );

                    if (WriteAction is not null)
                    {
                        WriteAction(CheckingWriter);
                    }
                    else
                    {
                        WriteAuto(CheckingWriter);
                    }

                    OnWrited(Name, NameId, FormatId);
                }
                else
                {
                    //TODO
                    var RecordWriter = new ADFRecordObjectWriter
                    (
                        Stream, // Нужно: писать в тот же поток
                        FormatRegistry, // Нужно: реестр форматов для создания
                        Type, // Нужно: тип для создания формата
                        Options
                    );

                    if (WriteAction is not null)
                    {
                        WriteAction(RecordWriter);
                    }
                    else
                    {
                        WriteAuto(RecordWriter);
                    }

                    var CreatedFormat = RecordWriter.BuildFormat();
                    var CreatedFormatId = FormatRegistry.Add(CreatedFormat);

                    TypeAssociation.Add(Type, CreatedFormatId);

                    OnWrited(Name, NameId, CreatedFormatId);
                }
            }
        }

        #endregion

        #region Sequences

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

            foreach (var Reference in Childs)
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