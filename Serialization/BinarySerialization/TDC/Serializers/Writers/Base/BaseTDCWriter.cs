using System.Numerics;
using Zion.Vectors;
using Vector2 = Zion.Vectors.Vector2;
using Vector3 = Zion.Vectors.Vector3;

namespace Zion.Serialization.TDC
{
    public abstract class BaseTDCWriter : IDisposable, IAsyncDisposable
    {
        #region Types
        protected delegate void WriteAction<T>(T Value, out ushort TypeId);

        #endregion

        #region Data
        protected readonly TypeIdRegistry TypeRegistry;
        protected readonly MemoryStream Memory;
        protected readonly BinaryWriter MemoryWriter;

        #endregion

        #region Constructors
        public BaseTDCWriter()
        {
            TypeRegistry = new();
            Memory = new();
            MemoryWriter = new(Memory);
        }

        public BaseTDCWriter(BaseTDCWriter Base)
        {
            TypeRegistry = Base.TypeRegistry;
            Memory       = Base.Memory;
            MemoryWriter = Base.MemoryWriter;
        }

        #endregion

        #region ProtectedMethods
        protected void WritePrimitive<T>(T Primitive, out ushort TypeId) where T : ITDCPrimitive<T>
        {
            Type Type = Primitive.GetType();

            if (TypeRegistry.TryGetInfo(Type, out TypeInfo Info)
                && Info.Format is PrimitiveFormat Format)
            {
                CheckingPrimitiveTDCWriter PrimitiveWriter = new(this, Format);
                Primitive.Write(PrimitiveWriter);
            }
            else
            {
                RecordPrimitiveTDCWriter PrimitiveWriter = new(this);
                Primitive.Write(PrimitiveWriter);
                Info = TypeRegistry.Add(Type, PrimitiveWriter.GetFormat());
            }
            TypeId = Info.TypeId;
        }

        protected void WriteContainer<T>(ITDCContainer<T> Container, out ushort TypeId)
        {
            var Writer = new ContainerTDCWriter(this);
            Container.Write(Writer);
            //TODO
            //Получение DataRegistry из Writer,
            //вычисление новых параметров,
            //сохранение их в TypeRegistry
            TypeId = 0;
        }


        protected void WriteSimplePrimitive(bool Value, out ushort TypeId)
        {
            Write(SimplePrimitive.Boolean, Value, bool.Serializer, out TypeId);
        }

        protected void WriteSimplePrimitive(byte Value, out ushort TypeId)
        {
            Write(SimplePrimitive.Byte, Value, byte.Serializer, out TypeId);
        }

        protected void WriteSimplePrimitive(sbyte Value, out ushort TypeId)
        {
            Write(SimplePrimitive.SByte, Value, sbyte.Serializer, out TypeId);
        }

        protected void WriteSimplePrimitive(char Value, out ushort TypeId)
        {
            Write(SimplePrimitive.Char, Value, char.Serializer, out TypeId);
        }

        protected void WriteSimplePrimitive(decimal Value, out ushort TypeId)
        {
            Write(SimplePrimitive.Decimal, Value, decimal.Serializer, out TypeId);
        }

        protected void WriteSimplePrimitive(double Value, out ushort TypeId)
        {
            Write(SimplePrimitive.Double, Value, double.Serializer, out TypeId);
        }

        protected void WriteSimplePrimitive(float Value, out ushort TypeId)
        {
            Write(SimplePrimitive.Single, Value, float.Serializer, out TypeId);
        }

        protected void WriteSimplePrimitive(int Value, out ushort TypeId)
        {
            Write(SimplePrimitive.Int32, Value, int.Serializer, out TypeId);
        }

        protected void WriteSimplePrimitive(uint Value, out ushort TypeId)
        {
            Write(SimplePrimitive.UInt32, Value, uint.Serializer, out TypeId);
        }

        protected void WriteSimplePrimitive(long Value, out ushort TypeId)
        {
            Write(SimplePrimitive.Int64, Value, long.Serializer, out TypeId);
        }

        protected void WriteSimplePrimitive(ulong Value, out ushort TypeId)
        {
            Write(SimplePrimitive.UInt64, Value, ulong.Serializer, out TypeId);
        }

        protected void WriteSimplePrimitive(short Value, out ushort TypeId)
        {
            Write(SimplePrimitive.Int16, Value, short.Serializer, out TypeId);
        }

        protected void WriteSimplePrimitive(ushort Value, out ushort TypeId)
        {
            Write(SimplePrimitive.UInt16, Value, ushort.Serializer, out TypeId);
        }

        protected void WriteSimplePrimitive(string Value, out ushort TypeId)
        {
            Write(SimplePrimitive.String, Value, string.Serializer, out TypeId);
        }


        protected void WriteSimplePrimitive(Half Value, out ushort TypeId)
        {
            Write(SimplePrimitive.Half, Value, Half.Serializer, out TypeId);
        }

        protected void WriteSimplePrimitive(Index Value, out ushort TypeId)
        {
            Write(SimplePrimitive.Index, Value, Index.Serializer, out TypeId);
        }

        protected void WriteSimplePrimitive(Range Value, out ushort TypeId)
        {
            Write(SimplePrimitive.Range, Value, Range.Serializer, out TypeId);
        }

        protected void WriteSimplePrimitive(BigInteger Value, out ushort TypeId)
        {
            Write(SimplePrimitive.BigInteger, Value, BigInteger.Serializer, out TypeId);
        }


        protected void WriteSimplePrimitive(RGBColor Value, out ushort TypeId)
        {
            Write(SimplePrimitive.RGB, Value, out TypeId);
        }

        protected void WriteSimplePrimitive(RGBAColor Value, out ushort TypeId)
        {
            Write(SimplePrimitive.RGBA, Value, out TypeId);
        }


        protected void WriteSimplePrimitive(Vector2 Value, out ushort TypeId)
        {
            Write(SimplePrimitive.Vector2, Value, out TypeId);
        }

        protected void WriteSimplePrimitive(Vector2Int Value, out ushort TypeId)
        {
            Write(SimplePrimitive.Vector2Int, Value, out TypeId);
        }

        protected void WriteSimplePrimitive(Vector3 Value, out ushort TypeId)
        {
            Write(SimplePrimitive.Vector3, Value, out TypeId);
        }

        protected void WriteSimplePrimitive(Vector3Int Value, out ushort TypeId)
        {
            Write(SimplePrimitive.Vector3Int, Value, out TypeId);
        }

        #endregion

        #region AbstractMethods
        protected virtual void OnDisposed() { }

        #endregion

        #region PrivateMethods
        private void Write<T>(SimplePrimitive Type, T Value, IBinaryWriter<T> Writer, out ushort TypeId)
        {
            TypeId = (ushort)Type;
            MemoryWriter.Write(TypeId);
            Writer.Write(MemoryWriter, Value);
        }

        private void Write<T>(SimplePrimitive Type, T Value, out ushort TypeId) where T : IBinaryWritable
        {
            TypeId = (ushort)Type;
            MemoryWriter.Write(TypeId);
            MemoryWriter.Write(Value);
        }

        #endregion

        #region IDisposable
        public void Dispose()
        {
            OnDisposed();
            Memory.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            OnDisposed();
            await Memory.DisposeAsync();
        }

        #endregion
    }
}