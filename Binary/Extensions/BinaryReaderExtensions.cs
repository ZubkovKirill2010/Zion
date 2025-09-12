namespace Zion
{
    public static class BinaryReaderExtensions
    {
        public static T Read<T>(this BinaryReader Reader) where T : IBinaryObject<T>
        {
            return T.Read(Reader);
        }

        public static T Read<T, I>(this BinaryReader Reader, Func<BinaryReader, I> Read) where T : IBinaryGeneric<T, I>
        {
            return T.Read(Reader, Read);
        }


        public static T[] ReadArray<T>(this BinaryReader Reader) where T : IBinaryObject<T>
        {
            return ReadArray(Reader, T.Read, Reader.ReadInt32());
        }
        public static T[] ReadArray<T>(this BinaryReader Reader, Func<BinaryReader, T> Read)
        {
            return ReadArray<T>(Reader, Read, Reader.ReadInt32());
        }
        public static T[] ReadArray<T>(this BinaryReader Reader, Func<BinaryReader, T> Read, int Count)
        {
            T[] Result = new T[Count];
            for (int i = 0; i < Count; i++)
            {
                Result[i] = Read(Reader);
            }
            return Result;
        }

        public static List<T> ReadList<T>(this BinaryReader Reader, int AdditionalCapacity = 0) where T : IBinaryObject<T>
        {
            return ReadList(Reader, T.Read, Reader.ReadInt32(), AdditionalCapacity);
        }
        public static List<T> ReadList<T>(this BinaryReader Reader, Func<BinaryReader, T> Read, int AdditionalCapacity = 0)
        {
            return ReadList<T>(Reader, Read, Reader.ReadInt32(), AdditionalCapacity);
        }
        public static List<T> ReadList<T>(this BinaryReader Reader, Func<BinaryReader, T> Read, int Count, int AdditionalCapacity = 0)
        {
            List<T> Result = new List<T>(Count + AdditionalCapacity);
            for (int i = 0; i < Count; i++)
            {
                Result.Add(Read(Reader));
            }
            return Result;
        }


        public static string[] ReadStringArray(this BinaryReader Reader)
            => ReadArray(Reader, Reader => Reader.ReadString());
        public static bool[] ReadBooleanArray(this BinaryReader Reader)
            => ReadArray(Reader, Reader => Reader.ReadBoolean());
        public static byte[] ReadByteArray(this BinaryReader Reader)
            => ReadArray(Reader, Reader => Reader.ReadByte());
        public static char[] ReadCharArray(this BinaryReader Reader)
            => ReadArray(Reader, Reader => Reader.ReadChar());
        public static int[] ReadInt32Array(this BinaryReader Reader)
            => ReadArray(Reader, Reader => Reader.ReadInt32());
        public static float[] ReadSingleArray(this BinaryReader Reader)
            => ReadArray(Reader, Reader => Reader.ReadSingle());
        public static double[] ReadDoubleArray(this BinaryReader Reader)
            => ReadArray(Reader, Reader => Reader.ReadDouble());
        public static decimal[] ReadDecimalArray(this BinaryReader Reader)
            => ReadArray(Reader, Reader => Reader.ReadDecimal());
        public static long[] ReadInt64Array(this BinaryReader Reader)
            => ReadArray(Reader, Reader => Reader.ReadInt64());
        public static sbyte[] ReadSByteArray(this BinaryReader Reader)
            => ReadArray(Reader, Reader => Reader.ReadSByte());
        public static ushort[] ReadUInt16Array(this BinaryReader Reader)
            => ReadArray(Reader, Reader => Reader.ReadUInt16());
        public static uint[] ReadUInt32Array(this BinaryReader Reader)
            => ReadArray(Reader, Reader => Reader.ReadUInt32());
        public static ulong[] ReadUInt64Array(this BinaryReader Reader)
            => ReadArray(Reader, Reader => Reader.ReadUInt64());


        public static List<string> ReadStringList(this BinaryReader Reader)
            => ReadList(Reader, Reader => Reader.ReadString());
        public static List<bool> ReadBooleanList(this BinaryReader Reader)
            => ReadList(Reader, Reader => Reader.ReadBoolean());
        public static List<byte> ReadByteList(this BinaryReader Reader)
            => ReadList(Reader, Reader => Reader.ReadByte());
        public static List<char> ReadCharList(this BinaryReader Reader)
            => ReadList(Reader, Reader => Reader.ReadChar());
        public static List<int> ReadInt32List(this BinaryReader Reader)
            => ReadList(Reader, Reader => Reader.ReadInt32());
        public static List<float> ReadSingleList(this BinaryReader Reader)
            => ReadList(Reader, Reader => Reader.ReadSingle());
        public static List<double> ReadDoubleList(this BinaryReader Reader)
            => ReadList(Reader, Reader => Reader.ReadDouble());
        public static List<decimal> ReadDecimalList(this BinaryReader Reader)
            => ReadList(Reader, Reader => Reader.ReadDecimal());
        public static List<long> ReadInt64List(this BinaryReader Reader)
            => ReadList(Reader, Reader => Reader.ReadInt64());
        public static List<sbyte> ReadSByteList(this BinaryReader Reader)
            => ReadList(Reader, Reader => Reader.ReadSByte());
        public static List<ushort> ReadUInt16List(this BinaryReader Reader)
            => ReadList(Reader, Reader => Reader.ReadUInt16());
        public static List<uint> ReadUInt32List(this BinaryReader Reader)
            => ReadList(Reader, Reader => Reader.ReadUInt32());
        public static List<ulong> ReadUInt64List(this BinaryReader Reader)
            => ReadList(Reader, Reader => Reader.ReadUInt64());
    }
}