using System.Numerics;
using Zion.Vectors;
using Vector2 = Zion.Vectors.Vector2;
using Vector3 = Zion.Vectors.Vector3;

namespace Zion.Serialization.TDC
{
    public abstract class BaseTDCWriter : IDisposable, IAsyncDisposable
    {
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

        protected void WriteContainer<T>(ITDCContainer<T> Container)
        {
            //TODO
        }


        protected void WriteSimplePrimitive(bool Value)
        {
            Write(SimplePrimitive.Boolean, Value, bool.Serializer);
        }

        protected void WriteSimplePrimitive(byte Value)
        {
            Write(SimplePrimitive.Byte, Value, byte.Serializer);
        }

        protected void WriteSimplePrimitive(sbyte Value)
        {
            Write(SimplePrimitive.SByte, Value, sbyte.Serializer);
        }

        protected void WriteSimplePrimitive(char Value)
        {
            Write(SimplePrimitive.Char, Value, char.Serializer);
        }

        protected void WriteSimplePrimitive(decimal Value)
        {
            Write(SimplePrimitive.Decimal, Value, decimal.Serializer);
        }

        protected void WriteSimplePrimitive(double Value)
        {
            Write(SimplePrimitive.Double, Value, double.Serializer);
        }

        protected void WriteSimplePrimitive(float Value)
        {
            Write(SimplePrimitive.Single, Value, float.Serializer);
        }

        protected void WriteSimplePrimitive(int Value)
        {
            Write(SimplePrimitive.Int32, Value, int.Serializer);
        }

        protected void WriteSimplePrimitive(uint Value)
        {
            Write(SimplePrimitive.UInt32, Value, uint.Serializer);
        }

        protected void WriteSimplePrimitive(long Value)
        {
            Write(SimplePrimitive.Int64, Value, long.Serializer);
        }

        protected void WriteSimplePrimitive(ulong Value)
        {
            Write(SimplePrimitive.UInt64, Value, ulong.Serializer);
        }

        protected void WriteSimplePrimitive(short Value)
        {
            Write(SimplePrimitive.Int16, Value, short.Serializer);
        }

        protected void WriteSimplePrimitive(ushort Value)
        {
            Write(SimplePrimitive.UInt16, Value, ushort.Serializer);
        }

        protected void WriteSimplePrimitive(string Value)
        {
            Write(SimplePrimitive.String, Value, string.Serializer);
        }


        protected void WriteSimplePrimitive(Half Value)
        {
            Write(SimplePrimitive.Half, Value, Half.Serializer);
        }

        protected void WriteSimplePrimitive(Index Value)
        {
            Write(SimplePrimitive.Index, Value, Index.Serializer);
        }

        protected void WriteSimplePrimitive(Range Value)
        {
            Write(SimplePrimitive.Range, Value, Range.Serializer);
        }

        protected void WriteSimplePrimitive(BigInteger Value)
        {
            Write(SimplePrimitive.BigInteger, Value, BigInteger.Serializer);
        }


        protected void WriteSimplePrimitive(RGBColor Value)
        {
            Write(SimplePrimitive.RGB, Value);
        }

        protected void WriteSimplePrimitive(RGBAColor Value)
        {
            Write(SimplePrimitive.RGBA, Value);
        }


        protected void WriteSimplePrimitive(Vector2 Value)
        {
            Write(SimplePrimitive.Vector2, Value);
        }

        protected void WriteSimplePrimitive(Vector2Int Value)
        {
            Write(SimplePrimitive.Vector2Int, Value);
        }

        protected void WriteSimplePrimitive(Vector3 Value)
        {
            Write(SimplePrimitive.Vector3, Value);
        }

        protected void WriteSimplePrimitive(Vector3Int Value)
        {
            Write(SimplePrimitive.Vector3Int, Value);
        }

        #endregion

        #region AbstractMethods
        protected virtual void OnDisposed() { }

        #endregion

        #region PrivateMethods
        private void Write<T>(SimplePrimitive Type, T Value, IBinaryWriter<T> Writer)
        {
            MemoryWriter.Write((ushort)Type);
            Writer.Write(MemoryWriter, Value);
        }

        private void Write<T>(SimplePrimitive Type, T Value) where T : IBinaryWritable
        {
            MemoryWriter.Write((ushort)Type);
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