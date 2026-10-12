using System.Numerics;
using Zion.Vectors;
using Vector2 = Zion.Vectors.Vector2;
using Vector3 = Zion.Vectors.Vector3;

namespace Zion.Serialization.ADF
{
    //_Primitives
    public abstract partial class BaseADFWriter
    {
        public void Write(string Name, bool Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Boolean, BooleanWriteStrategy.Instance);
        }

        public void Write(string Name, byte Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Byte, ByteWriteStrategy.Instance);
        }

        public void Write(string Name, sbyte Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.SByte, SByteWriteStrategy.Instance);
        }


        public void Write(string Name, short Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Int16, Int16WriteStrategy.Instance);
        }

        public void Write(string Name, int Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Int32, Int32WriteStrategy.Instance);
        }

        public void Write(string Name, long Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Int64, Int64WriteStrategy.Instance);
        }

        public void Write(string Name, ushort Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.UInt16, UInt16WriteStrategy.Instance);
        }

        public void Write(string Name, uint Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.UInt32, UInt32WriteStrategy.Instance);
        }

        public void Write(string Name, ulong Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.UInt64, UInt64WriteStrategy.Instance);
        }


        public void Write(string Name, char Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Char, CharWriteStrategy.Instance);
        }

        public void Write(string Name, float Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Single, SingleWriteStrategy.Instance);
        }

        public void Write(string Name, double Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Double, DoubleWriteStrategy.Instance);
        }

        public void Write(string Name, decimal Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Decimal, DecimalWriteStrategy.Instance);
        }

        public void Write(string Name, string Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.String, StringWriteStrategy.Instance);
        }


        public void Write(string Name, Half Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Half, HalfWriteStrategy.Instance);
        }

        public void Write(string Name, Index Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Index, IndexWriteStrategy.Instance);
        }

        public void Write(string Name, Range Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Range, RangeWriteStrategy.Instance);
        }

        public void Write(string Name, BigInteger Value)
        {
            WritePrimitive<BigInteger>(Name, in Value, ADFPrimitives.BigInteger, BigIntegerWriteStrategy.Instance);
        }


        public void Write(string Name, RGBColor Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.RGB, RGBColorWriteStrategy.Instance);
        }

        public void Write(string Name, RGBAColor Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.RGBA, RGBAColorWriteStrategy.Instance);
        }


        public void Write(string Name, Vector2 Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Vector2, Vector2WriteStrategy.Instance);
        }

        public void Write(string Name, Vector2Int Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Vector2Int, Vector2IntWriteStrategy.Instance);
        }

        public void Write(string Name, Vector3 Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Vector3, Vector3WriteStrategy.Instance);
        }

        public void Write(string Name, Vector3Int Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Vector3Int, Vector3IntWriteStrategy.Instance);
        }


        private void WritePrimitive<T>(string Name, in T Value, uint FormatId, IWriteStrategy<T> Strategy)
        {
            ThrowIfDisposed();

            var NameId = StringRegistry.GetOrAdd(Name.NotNull());
            var Target = GetStreamGroup(Name, in NameId, in FormatId);

            Strategy.Write(Context, Target, Value);
            OnWrited(Name, in NameId, in FormatId);
        }
    }
}