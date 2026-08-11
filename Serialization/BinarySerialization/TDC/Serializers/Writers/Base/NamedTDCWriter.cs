using System.Numerics;
using Zion.Vectors;
using Vector2 = Zion.Vectors.Vector2;
using Vector3 = Zion.Vectors.Vector3;

namespace Zion.Serialization.TDC
{
    public abstract class NamedTDCWriter : BaseTDCWriter
    {
        #region Data
        protected readonly DataRegistry DataRegistry;
        protected readonly MemoryStream Memory;
        protected readonly BinaryWriter MemoryWriter;

        #endregion

        #region Constructors
        public NamedTDCWriter(Stream Stream, TypeIdRegistry TypeRegistry)
            : base(Stream, TypeRegistry)
        {
            DataRegistry = new();
            Memory = new();
            MemoryWriter = new BinaryWriter(Memory);
        }

        #endregion

        #region PublicMethods
        public bool Contains(string Key)
        {
            return DataRegistry.Contains(Key);
        }


        public void Write(string Key, bool Value)
        {
            Write(Key, SimplePrimitive.Boolean, Value, bool.Serializer);
        }

        public void Write(string Key, byte Value)
        {
            Write(Key, SimplePrimitive.Byte, Value, byte.Serializer);
        }

        public void Write(string Key, sbyte Value)
        {
            Write(Key, SimplePrimitive.SByte, Value, sbyte.Serializer);
        }

        public void Write(string Key, char Value)
        {
            Write(Key, SimplePrimitive.Char, Value, char.Serializer);
        }

        public void Write(string Key, decimal Value)
        {
            Write(Key, SimplePrimitive.Decimal, Value, decimal.Serializer);
        }

        public void Write(string Key, double Value)
        {
            Write(Key, SimplePrimitive.Double, Value, double.Serializer);
        }

        public void Write(string Key, float Value)
        {
            Write(Key, SimplePrimitive.Single, Value, float.Serializer);
        }

        public void Write(string Key, int Value)
        {
            Write(Key, SimplePrimitive.Int32, Value, int.Serializer);
        }

        public void Write(string Key, uint Value)
        {
            Write(Key, SimplePrimitive.UInt32, Value, uint.Serializer);
        }

        public void Write(string Key, long Value)
        {
            Write(Key, SimplePrimitive.Int64, Value, long.Serializer);
        }

        public void Write(string Key, ulong Value)
        {
            Write(Key, SimplePrimitive.UInt64, Value, ulong.Serializer);
        }

        public void Write(string Key, short Value)
        {
            Write(Key, SimplePrimitive.Int16, Value, short.Serializer);
        }

        public void Write(string Key, ushort Value)
        {
            Write(Key, SimplePrimitive.UInt16, Value, ushort.Serializer);
        }

        public void Write(string Key, string Value)
        {
            Write(Key, SimplePrimitive.String, Value, string.Serializer);
        }


        public void Write(string Key, Half Value)
        {
            Write(Key, SimplePrimitive.Half, Value, Half.Serializer);
        }


        public void Write(string Key, Index Value)
        {
            Write(Key, SimplePrimitive.Index, Value, Index.Serializer);
        }

        public void Write(string Key, Range Value)
        {
            Write(Key, SimplePrimitive.Range, Value, Range.Serializer);
        }


        public void Write(string Key, BigInteger Value)
        {
            Write(Key, SimplePrimitive.BigInteger, Value, BigInteger.Serializer);
        }


        public void Write(string Key, RGBColor Value)
        {
            Write(Key, SimplePrimitive.RGB, Value);
        }

        public void Write(string Key, RGBAColor Value)
        {
            Write(Key, SimplePrimitive.RGBA, Value);
        }


        public void Write(string Key, Vector2 Value)
        {
            Write(Key, SimplePrimitive.Vector2, Value);
        }

        public void Write(string Key, Vector2Int Value)
        {
            Write(Key, SimplePrimitive.Vector2Int, Value);
        }

        public void Write(string Key, Vector3 Value)
        {
            Write(Key, SimplePrimitive.Vector3, Value);
        }

        public void Write(string Key, Vector3Int Value)
        {
            Write(Key, SimplePrimitive.Vector3Int, Value);
        }



        public void Write<T>(string Key, ITDCPrimitive<T> Value) where T : ITDCPrimitive<T> { }

        public void Write<T>(string Key, ITDCContainer<T> Value) where T : ITDCContainer<T> { }

        #endregion

        #region AbstractMethods
        protected abstract void WriteSimplePrimitive<T>(string Key, T Value, Action<BinaryWriter> Write);

        #endregion

        #region PrivateMethods
        private void Write<T>(string Key, SimplePrimitive Type, T Value, IBinaryWriter<T> Writer)
        {
            CheckKey(Key);

            long Start = Memory.Position;
            MemoryWriter.Write((ushort)Type);
            Writer.Write(MemoryWriter, Value);
            long Length = Memory.Position - Start;

            DataRegistry.Add(Key, new(Start, Length));
        }

        private void Write<T>(string Key, SimplePrimitive Type, T Value) where T : IBinaryWritable
        {
            CheckKey(Key);

            long Start = Memory.Position;
            MemoryWriter.Write((ushort)Type);
            MemoryWriter.Write(Value);
            long Length = Memory.Position - Start;

            DataRegistry.Add(Key, new(Start, Length));
        }


        private void CheckKey(string Key)
        {
            if (Contains(Key))
            {
                throw new ArgumentException($"Recording with a key '{Key}' already exists");
            }
        }

        #endregion
    }
}