using System.Numerics;
using Zion.Vectors;
using Vector2 = Zion.Vectors.Vector2;
using Vector3 = Zion.Vectors.Vector3;

namespace Zion.Serialization.TDC
{
    public abstract class PrimitiveTDCWriter : BaseTDCWriter
    {
        #region Constructors
        public PrimitiveTDCWriter(BaseTDCWriter Base) : base(Base) { }

        #endregion

        #region PublicMethods
        #region All
        public void Write<T>(T Value)
        {
            if (Value is ITDCPrimitive<T> Primitive)
            {
                WritePrimitive(Primitive, out ushort TypeId);
                OnWrited(TypeId);
            }
            else if (Value is ITDCContainer<T> Container)
            {
                WriteContainer(Container);
            }
            else if (SimplePrimitive.Contains<T>(out SimplePrimitive Type))
            {
                WriteSimplePrimitive(Value, Type);
            }
            else
            {
                throw new InvalidOperationException($"Value ({typeof(T)}) is not ITDCPrimitive, ITDCContainer or simple primitive");
            }
        }

        private void WriteSimplePrimitive<T>(T Value, SimplePrimitive Type)
        {
            Write(Type, Value, BinarySerializer.GetWriter<T>().NotNull());
        }

        #endregion

        #region SimplePrimitives
        public void Write(bool Value)
        {
            WriteSimplePrimitive(Value);
        }

        public void Write(byte Value)
        {
            Write(SimplePrimitive.Byte, Value, byte.Serializer);
        }

        public void Write(sbyte Value)
        {
            Write(SimplePrimitive.SByte, Value, sbyte.Serializer);
        }

        public void Write(char Value)
        {
            Write(SimplePrimitive.Char, Value, char.Serializer);
        }

        public void Write(decimal Value)
        {
            Write(SimplePrimitive.Decimal, Value, decimal.Serializer);
        }

        public void Write(double Value)
        {
            Write(SimplePrimitive.Double, Value, double.Serializer);
        }

        public void Write(float Value)
        {
            Write(SimplePrimitive.Single, Value, float.Serializer);
        }

        public void Write(int Value)
        {
            Write(SimplePrimitive.Int32, Value, int.Serializer);
        }

        public void Write(uint Value)
        {
            Write(SimplePrimitive.UInt32, Value, uint.Serializer);
        }

        public void Write(long Value)
        {
            Write(SimplePrimitive.Int64, Value, long.Serializer);
        }

        public void Write(ulong Value)
        {
            Write(SimplePrimitive.UInt64, Value, ulong.Serializer);
        }

        public void Write(short Value)
        {
            Write(SimplePrimitive.Int16, Value, short.Serializer);
        }

        public void Write(ushort Value)
        {
            Write(SimplePrimitive.UInt16, Value, ushort.Serializer);
        }

        public void Write(string Value)
        {
            Write(SimplePrimitive.String, Value, string.Serializer);
        }


        public void Write(Half Value)
        {
            Write(SimplePrimitive.Half, Value, Half.Serializer);
        }

        public void Write(Index Value)
        {
            Write(SimplePrimitive.Index, Value, Index.Serializer);
        }

        public void Write(Range Value)
        {
            Write(SimplePrimitive.Range, Value, Range.Serializer);
        }

        public void Write(BigInteger Value)
        {
            Write(SimplePrimitive.BigInteger, Value, BigInteger.Serializer);
        }


        public void Write(RGBColor Value)
        {
            Write(SimplePrimitive.RGB, Value);
        }

        public void Write(RGBAColor Value)
        {
            Write(SimplePrimitive.RGBA, Value);
        }


        public void Write(Vector2 Value)
        {
            Write(SimplePrimitive.Vector2, Value);
        }

        public void Write(Vector2Int Value)
        {
            Write(SimplePrimitive.Vector2Int, Value);
        }

        public void Write(Vector3 Value)
        {
            Write(SimplePrimitive.Vector3, Value);
        }

        public void Write(Vector3Int Value)
        {
            Write(SimplePrimitive.Vector3Int, Value);
        }

        #endregion

        #endregion

        #region AbstractMethods
        protected abstract void OnWrited(ushort TypeId);

        #endregion

        #region PrivateMethods
        private void Write<T>(SimplePrimitive Type, T Value, IBinaryWriter<T> Writer)
        {
            ushort TypeId = (ushort)Type;
            OnWrited(TypeId);
            Writer.Write(MemoryWriter, Value);
        }

        private void Write<T>(SimplePrimitive Type, T Value) where T : IBinaryWritable
        {
            ushort TypeId = (ushort)Type;
            OnWrited(TypeId);
            MemoryWriter.Write(Value);
        }

        #endregion
    }
}