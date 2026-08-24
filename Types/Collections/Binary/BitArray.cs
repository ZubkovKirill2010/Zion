using System.Collections;
using Zion.Serialization;

namespace Zion
{
    public class BitArray : IBinarySerializable<BitArray>, IEnumerable<bool>
    {
        #region Constants
        private const int Filter = 0b111;

        #endregion

        #region Data
        private readonly byte[] Data;
        
        public readonly int Length;

        #endregion

        #region Constructors
        public BitArray(int Length)
        {
            this.Data = new byte[GetByteCount(Length)];
            this.Length = Length;
        }

        public BitArray(byte[] Data, int Length)
        {
            ArgumentNullException.ThrowIfNull(Data);
            ArgumentOutOfRangeException.ThrowIfWithout(Length >> 3, Data.Length);

            this.Data   = ZArray.Clone(Data);
            this.Length = Length;
        }

        private BitArray(int Length, byte[] Data)
        {
            this.Length = Length;
            this.Data = Data;
        }

        #endregion

        #region Indexers
        public bool this[int Index]
        {
            get
            {
                if (Index < 0 || Index >= Length)
                {
                    throw new ArgumentOutOfRangeException($"Index(={Index}) out of range [0..{Length})");
                }
                return Data[Index >> 3].GetBit(Index & Filter);
            }
            set
            {
                if (Index < 0 || Index >= Length)
                {
                    throw new ArgumentOutOfRangeException($"Index(={Index}) out of range [0..{Length})");
                }
                int ByteIndex = Index >> 3;
                Data[ByteIndex] = Data[ByteIndex].SetBit(Index & Filter, value);
            }
        }

        public bool this[Index Index]
        {
            get => this[Index.GetOffset(Length)];
            set => this[Index.GetOffset(Length)] = value;
        }

        #endregion

        #region Operators
        public static bool operator ==(BitArray A, BitArray B)
        {
            if (object.CompareReferences(A, B, out bool ReferenceComprasion))
            {
                return ReferenceComprasion;
            }
            if (A.Length != B.Length)
            {
                return false;
            }

            int Count = A.Length & 0b111;
            byte[] ABuffer = A.Data;
            byte[] BBuffer = B.Data;

            for (int i = 0; i < Count; i++)
            {
                if (ABuffer[i] != BBuffer[i])
                {
                    return false;
                }
            }

            int LastByteOffset = 8 - A.GetLastByteLength();
            return ABuffer[^1] >> LastByteOffset == ABuffer[^1] >> LastByteOffset;
        }

        public static bool operator !=(BitArray A, BitArray B)
        {
            return !(A == B);
        }


        public static BitArray operator &(BitArray A, BitArray B)
        {
            return ConvertBits(A, B, static (A, B) => (byte)(A & B));
        }

        public static BitArray operator |(BitArray A, BitArray B)
        {
            return ConvertBits(A, B, static (A, B) => (byte)(A | B));
        }

        public static BitArray operator ^(BitArray A, BitArray B)
        {
            return ConvertBits(A, B, static (A, B) => (byte)(A ^ B));

        }

        public static BitArray operator ~(BitArray Value)
        {
            ArgumentNullException.ThrowIfNull(Value);

            byte[] Source = Value.Data;
            byte[] Result = new byte[Source.Length];
            int Length = Value.Length;

            for (int i = 0; i < Length; i++)
            {
                Result[i] = (byte)~Source[i];
            }

            return new BitArray(Length, Result);
        }

        #endregion

        #region PublicMethods
        public BitArray Clone()
        {
            return new BitArray(Data, Length);
        }

        public BitList ToBitList()
        {
            return new BitList(Data, Length);
        }


        public Span<byte> AsSpan()
        {
            return Data.AsSpan();
        }

        public byte[] ToByteArray()
        {
            return ZArray.Clone(Data);
        }

        public bool[] ToBooleanArray()
        {
            bool[] Result = new bool[Length];
            int Index = 0;

            foreach (bool Bit in this)
            {
                if (Bit)
                {
                    Result[Index] = true;
                }
                Index++;
            }

            return Result;
        }

        #endregion

        #region IBinarySerializable
        public void Write(BinaryWriter Writer)
        {
            Writer.Write(Length);
            Writer.Write(Data, 0, GetByteCount(Length));
        }

        public static BitArray Read(BinaryReader Reader)
        {
            int Count = Reader.ReadInt32();
            byte[] Data = Reader.ReadBytes(GetByteCount(Count));

            return new BitArray(Count, Data);
        }

        #endregion

        #region IEnumerable
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<bool> GetEnumerator()
        {
            int FullBytes = Length >> 3;

            for (int i = 0; i < FullBytes; i++)
            {
                byte Current = Data[i];
                yield return (Current & 0b0000_0001) != 0;
                yield return (Current & 0b0000_0010) != 0;
                yield return (Current & 0b0000_0100) != 0;
                yield return (Current & 0b0000_1000) != 0;
                yield return (Current & 0b0001_0000) != 0;
                yield return (Current & 0b0010_0000) != 0;
                yield return (Current & 0b0100_0000) != 0;
                yield return (Current & 0b1000_0000) != 0;
            }

            int LastByteLength = Length & Filter;
            if (LastByteLength != 0)
            {
                byte Current = Data[FullBytes];
                int BitIndex = 0;
                if (++BitIndex > LastByteLength) { yield break; }
                yield return (Current & 0b0000_0001) != 0;
                if (++BitIndex > LastByteLength) { yield break; }
                yield return (Current & 0b0000_0010) != 0;
                if (++BitIndex > LastByteLength) { yield break; }
                yield return (Current & 0b0000_0100) != 0;
                if (++BitIndex > LastByteLength) { yield break; }
                yield return (Current & 0b0000_1000) != 0;
                if (++BitIndex > LastByteLength) { yield break; }
                yield return (Current & 0b0001_0000) != 0;
                if (++BitIndex > LastByteLength) { yield break; }
                yield return (Current & 0b0010_0000) != 0;
                if (++BitIndex > LastByteLength) { yield break; }
                yield return (Current & 0b0100_0000) != 0;
                if (++BitIndex > LastByteLength) { yield break; }
                yield return (Current & 0b1000_0000) != 0;
            }
        }

        #endregion

        #region PrivateMethods
        private int GetLastByteLength()
        {
            return Data.Length - Length;
        }

        private static int GetByteCount(int Count)
        {
            return (Count + 7) >> 3;
        }

        private static BitArray ConvertBits(BitArray A, BitArray B, Func<byte, byte, byte> Convert)
        {
            ArgumentNullException.ThrowIfNull(A);
            ArgumentNullException.ThrowIfNull(B);

            int MinLength = Math.Min(A.Data.Length, B.Data.Length);
            int MaxLength = Math.Max(A.Data.Length, B.Data.Length);
            byte[] Result = new byte[MaxLength];
            Span<byte> ASpan = A.AsSpan();
            Span<byte> BSpan = B.AsSpan();

            for (int i = 0; i < MinLength; i++)
            {
                Result[i] = Convert(ASpan[i], BSpan[i]);
            }

            return new BitArray(MaxLength, Result);
        }

        #endregion
    }
}