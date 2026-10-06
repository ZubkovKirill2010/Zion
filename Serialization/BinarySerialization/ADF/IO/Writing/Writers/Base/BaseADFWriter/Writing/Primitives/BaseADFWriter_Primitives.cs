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
                case bool V: Write(Name, V); return true;
                case byte V: Write(Name, V); return true;
                case sbyte V: Write(Name, V); return true;

                case short V: Write(Name, V); return true;
                case ushort V: Write(Name, V); return true;
                case int V: Write(Name, V); return true;
                case uint V: Write(Name, V); return true;
                case long V: Write(Name, V); return true;
                case ulong V: Write(Name, V); return true;

                case char V: Write(Name, V); return true;
                case float V: Write(Name, V); return true;
                case double V: Write(Name, V); return true;
                case decimal V: Write(Name, V); return true;
                case string V: Write(Name, V); return true;

                case Half V: Write(Name, V); return true;
                case Index V: Write(Name, V); return true;
                case Range V: Write(Name, V); return true;
                case BigInteger V: Write(Name, V); return true;

                case RGBColor V: Write(Name, V); return true;
                case RGBAColor V: Write(Name, V); return true;

                case Vector2 V: Write(Name, V); return true;
                case Vector3 V: Write(Name, V); return true;
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
    }
}