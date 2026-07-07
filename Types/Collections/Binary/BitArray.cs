using System.Collections;
using Zion.Serialization;
using Zion.Types.Collections.Binary;

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

        #region PublicMethods

        public BitArray Clone()
        {
            return new BitArray(Data, Length);
        }

        public BitList ToBitList()
        {
            return new BitList(Data, Length);
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
        private static int GetByteCount(int Count)
        {
            return (Count + 7) >> 3;
        }

        #endregion
    }
}