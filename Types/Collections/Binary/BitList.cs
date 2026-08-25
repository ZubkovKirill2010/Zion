using System.Collections;
using Zion.Serialization;

namespace Zion
{
    public class BitList : IList<bool>, IBinarySerializable<BitList>
    {
        #region Constants
        private const int Filter = 0b111;

        #endregion

        #region Data
        private byte[] Data;

        #endregion

        #region Properties
        public int Count { get; private set; }

        public int Capacity => Data.Length << 3;

        public bool IsReadOnly => false;

        #endregion

        #region Constructors
        public BitList() : this(24) { }

        public BitList(int Capacity)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Capacity);
            Data = new byte[GetByteCount(Capacity)];
        }

        public BitList(byte[] Data, int Count)
        {
            ArgumentNullException.ThrowIfNull(Data);
            ArgumentOutOfRangeException.ThrowIfWithout(Count >> 3, Data.Length);

            this.Data = ZArray.Clone(Data);
            this.Count = Count;
        }

        private BitList(int Count, byte[] Data)
        {
            this.Data = Data.NotNull();
            this.Count = Count;
        }

        #endregion

        #region Indexers
        public bool this[int Index]
        {
            get
            {
                if (Index < 0 || Index >= Count)
                {
                    throw new ArgumentOutOfRangeException($"Index(={Index}) out of range [0..{Count})");
                }
                return Data[Index >> 3].GetBit(Index & Filter);
            }
            set
            {
                if (Index < 0 || Index >= Count)
                {
                    throw new ArgumentOutOfRangeException($"Index(={Index}) out of range [0..{Count})");
                }
                int ByteIndex = Index >> 3;
                Data[ByteIndex] = Data[ByteIndex].SetBit(Index & Filter, value);
            }
        }

        public bool this[Index Index]
        {
            get => this[Index.GetOffset(Count)];
            set => this[Index.GetOffset(Count)] = value;
        }

        #endregion

        #region OverrideMethods
        public override string ToString()
        {
            int BitCount = Count;

            if (BitCount <= 0)
            {
                return "[]";
            }

            int ByteCount = (BitCount + 7) >> 3;
            int StringLength = 1 + BitCount + (ByteCount - 1) + 1;

            return string.Create
            (
                StringLength,
                (Data, Count),
                static (Span, State) =>
                {
                    var Data = State.Data;
                    int TotalBits = State.Count;

                    Span[0] = '[';
                    Span[^1] = ']';

                    int DestinationIndex = 1;
                    int FullBytes = TotalBits >> 3;

                    for (int ByteIndex = 0; ByteIndex < FullBytes; ByteIndex++)
                    {
                        if (ByteIndex > 0)
                        {
                            Span[DestinationIndex++] = '_';
                        }

                        byte B = Data[ByteIndex];
                        Span[DestinationIndex++] = (char)('0' + ((B >> 7) & 1));
                        Span[DestinationIndex++] = (char)('0' + ((B >> 6) & 1));
                        Span[DestinationIndex++] = (char)('0' + ((B >> 5) & 1));
                        Span[DestinationIndex++] = (char)('0' + ((B >> 4) & 1));
                        Span[DestinationIndex++] = (char)('0' + ((B >> 3) & 1));
                        Span[DestinationIndex++] = (char)('0' + ((B >> 2) & 1));
                        Span[DestinationIndex++] = (char)('0' + ((B >> 1) & 1));
                        Span[DestinationIndex++] = (char)('0' + (B & 1));
                    }

                    int RemainingBits = TotalBits & 7;
                    if (RemainingBits > 0)
                    {
                        if (FullBytes > 0)
                        {
                            Span[DestinationIndex++] = '_';
                        }

                        byte Bit = Data[FullBytes];
                        for (int BitIndex = 0; BitIndex < RemainingBits; BitIndex++)
                        {
                            int Shift = 7 - BitIndex;
                            Span[DestinationIndex++] = (char)('0' + ((Bit >> Shift) & 1));
                        }
                    }
                }
            );
        }

        public override bool Equals(object? Object)
        {
            return Object is BitList BitList && this == BitList;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Data, Count);
        }

        #endregion

        #region IList
        public void Add(bool Item)
        {
            int Index = Count;
            EnsureCapacity(++Count);
            if (Item)
            {
                int ByteIndex = Index >> 3;
                Data[ByteIndex] = Data[ByteIndex].SetBit(Index & Filter, Item);
            }
        }

        public void Insert(int Index, bool Item)
        {
            ArgumentOutOfRangeException.ThrowIfBeyond(Index, Count);

            EnsureCapacity(++Count);

            int LastByte = (Count - 1) >> 3;
            int InsertByte = Index >> 3;
            int InsertBit = Index & Filter;

            bool Carry = Item;

            for (int ByteIndex = InsertByte; ByteIndex <= LastByte; ByteIndex++)
            {
                int StartBit = (ByteIndex == InsertByte) ? InsertBit : 0;
                int EndBit = (ByteIndex == LastByte) ? ((Count - 1) & Filter) : 7;

                (Data[ByteIndex], Carry) = InsertBitShift(Data[ByteIndex], StartBit, EndBit, Carry);
            }
        }


        public int IndexOf(bool Item)
        {
            int FullBytes = Count >> 3;

            if (Item)
            {
                for (int ByteIndex = 0; ByteIndex < FullBytes; ByteIndex++)
                {
                    byte Current = Data[ByteIndex];
                    if (Current == 0) { continue; }

                    for (int Bit = 0; Bit < 8; Bit++)
                    {
                        if (Current.GetBit(Bit))
                        {
                            return (ByteIndex << 3) + Bit;
                        }
                    }
                }
            }
            else
            {
                for (int ByteIndex = 0; ByteIndex < FullBytes; ByteIndex++)
                {
                    byte Current = Data[ByteIndex];
                    if (Current == byte.MaxValue) { continue; }

                    for (int Bit = 0; Bit < 8; Bit++)
                    {
                        if (!Current.GetBit(Bit))
                        {
                            return (ByteIndex << 3) + Bit;
                        }
                    }
                }
            }

            int LastByteLength = Count & Filter;
            if (LastByteLength != 0)
            {
                byte LastByte = Data[FullBytes];
                for (int Bit = 0; Bit < LastByteLength; Bit++)
                {
                    if (LastByte.GetBit(Bit) == Item)
                    {
                        return (FullBytes << 3) + Bit;
                    }
                }
            }

            return -1;
        }

        public bool Contains(bool Item)
        {
            Func<byte, bool> Contains = Item
                ? static Byte => Byte != 0
                : static Byte => Byte != byte.MaxValue;

            int FullBytes = Count >> 3;

            for (int i = 0; i < FullBytes; i++)
            {
                if (Contains(Data[i]))
                {
                    return true;
                }
            }

            int LastByteLength = Count & Filter;
            if (LastByteLength != 0)
            {
                byte LastByte = Data[FullBytes];
                byte Target = Item ? byte.MinValue : byte.MaxValue;

                Target <<= 8 - LastByteLength;

                return LastByte != Target;
            }

            return false;
        }


        public void CopyTo(bool[] Array, int ArrayIndex)
        {
            ArgumentOutOfRangeException.ThrowIfWithout(ArrayIndex, Array);
            ArgumentOutOfRangeException.ThrowIfBeyond(Count, Array.Length - ArrayIndex);

            foreach (bool Item in this)
            {
                Array[ArrayIndex++] = Item;
            }
        }

        public BitList Clone()
        {
            int Capacity = GetByteCount(Count);
            byte[] NewData = new byte[Capacity];
            Array.Copy(Data, NewData, Capacity);
            return new BitList(Count, NewData);
        }


        public bool Remove(bool Item)
        {
            int Index = IndexOf(Item);
            
            if (Index != -1)
            {
                RemoveAt(Index);
                return true;
            }
            return false;
        }

        public void RemoveAt(int Index)
        {
            if (Index < 0 || Index >= Count)
            {
                throw new ArgumentOutOfRangeException($"Index(={Index}) out of range [0..{Count})");
            }

            int LastByte = (Count - 1) >> 3;
            int RemoveByte = Index >> 3;
            int RemoveBit = Index & Filter;

            bool Carry = false;

            for (int ByteIndex = LastByte; ByteIndex >= RemoveByte; ByteIndex--)
            {
                int StartBit = (ByteIndex == RemoveByte) ? RemoveBit : 0;
                int EndBit = (ByteIndex == LastByte) ? ((Count - 1) & Filter) : 7;

                (Data[ByteIndex], Carry) = RemoveBitShift(Data[ByteIndex], StartBit, EndBit, Carry);
            }

            Count--;

            int ClearByte = Count >> 3;
            int ClearBit = Count & Filter;
            Data[ClearByte] = Data[ClearByte].SetBit(ClearBit, false);
        }

        public void Clear()
        {
            Array.Clear(Data, 0, GetByteCount(Count));
        }


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<bool> GetEnumerator()
        {
            int FullBytes = Count >> 3;

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

            int LastByteLength = Count & Filter;
            if (LastByteLength != 0)
            {
                byte Current = Data[FullBytes];
                int BitIndex = 0;
                if (++BitIndex > LastByteLength) { yield break; } yield return (Current & 0b0000_0001) != 0;
                if (++BitIndex > LastByteLength) { yield break; } yield return (Current & 0b0000_0010) != 0;
                if (++BitIndex > LastByteLength) { yield break; } yield return (Current & 0b0000_0100) != 0;
                if (++BitIndex > LastByteLength) { yield break; } yield return (Current & 0b0000_1000) != 0;
                if (++BitIndex > LastByteLength) { yield break; } yield return (Current & 0b0001_0000) != 0;
                if (++BitIndex > LastByteLength) { yield break; } yield return (Current & 0b0010_0000) != 0;
                if (++BitIndex > LastByteLength) { yield break; } yield return (Current & 0b0100_0000) != 0;
                if (++BitIndex > LastByteLength) { yield break; } yield return (Current & 0b1000_0000) != 0;
            }
        }

        #endregion

        #region IBinarySerializable
        public void Write(BinaryWriter Writer)
        {
            Writer.Write(Count);
            Writer.Write(Data, 0, GetByteCount(Count));
        }

        public static BitList Read(BinaryReader Reader)
        {
            int Count   = Reader.ReadInt32();
            byte[] Data = Reader.ReadBytes(GetByteCount(Count));

            return new BitList(Count, Data);
        }

        #endregion

        #region PublicMethods
        public void EnsureCapacity(int Capacity)
        {
            if (Capacity <= this.Capacity)
            {
                return;
            }

            byte[] NewData = new byte[GetByteCount(Capacity)];
            Array.Copy(Data, NewData, GetByteCount(Count));
            Data = NewData;
        }


        public byte[] ToByteArray()
        {
            int Capacity = GetByteCount(Count);
            byte[] Result = new byte[Capacity];

            Array.Copy(Data, Result, Capacity);

            return Result;
        }

        public bool[] ToBooleanArray()
        {
            bool[] Result = new bool[Count];
            int Index = 0;
            
            foreach (bool Bit in this)
            {
                Result[Index++] = Bit;
            }

            return Result;
        }

        public BitArray ToBitArray()
        {
            return new BitArray(Data, Count);
        }

        #endregion

        #region PrivateMethods
        private static int GetByteCount(int Count)
        {
            return (Count + 7) >> 3;
        }

        private static (byte, bool) InsertBitShift(byte Value, int StartBit, int EndBit, bool Carry)
        {
            int ShiftedCount = EndBit - StartBit + 1;

            byte Mask = (byte)(((1 << ShiftedCount) - 1) << StartBit);
            byte Shifted = (byte)((Value & Mask) << 1);

            Value = (byte)((Value & ~Mask) | (Shifted & Mask));

            if (Carry)
            {
                Value |= (byte)(1 << StartBit);
            }

            bool NewCarry = (Value & (1 << (EndBit + 1))) != 0;

            if (EndBit < 7)
            {
                Value &= (byte)~(1 << (EndBit + 1));
            }

            return (Value, NewCarry);
        }

        private static (byte, bool) RemoveBitShift(byte Value, int StartBit, int EndBit, bool Carry)
        {
            bool Removed = (Value & (1 << StartBit)) != 0;

            int ShiftedCount = EndBit - StartBit;

            if (ShiftedCount > 0)
            {
                byte Mask = (byte)(((1 << ShiftedCount) - 1) << (StartBit + 1));
                byte Shifted = (byte)((Value & Mask) >> 1);

                Value = (byte)((Value & ~Mask) | (Shifted & Mask));
                Value &= (byte)~(1 << EndBit);
            }

            if (Carry)
            {
                Value |= (byte)(1 << EndBit);
            }
            else
            {
                Value &= (byte)~(1 << EndBit);
            }

            return (Value, Removed);
        }

        #endregion
    }
}