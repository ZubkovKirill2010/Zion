using System.Collections;

namespace Zion
{
    public class BitArray : IBinaryObject<BitArray>, IEnumerable<bool>
    {
        private readonly byte[] Data;

        public readonly int Length;

        public int Count => Length;
        public bool IsReadOnly => true;


        public BitArray(int Length)
        {
            Data = new byte[(Length / 8f).RoundToInt(RoundMode.Ceiling)];
        }
        private BitArray(byte[] Data, int Length)
        {
            this.Length = Length;
            this.Data = new byte[Data.Length];

            for (int i = 0; i < Data.Length; i++)
            {
                this.Data[i] = Data[i];
            }
        }


        public bool this[int Index]
        {
            get
            {
                int ByteIndex = Index / 8;
                return Data[ByteIndex].GetBit(Index - ByteIndex);
            }
            set
            {
                int ByteIndex = Index / 8;
                Data[ByteIndex].SetBit(Index - ByteIndex, value);
            }
        }


        public static implicit operator bool[](BitArray BitArray)
        {
            bool[] Array = new bool[BitArray.Length];

            for (int i = 0; i < Array.Length; i++)
            {
                Array[i] = BitArray[i];
            }

            return Array;
        }
        public static implicit operator List<bool>(BitArray BitArray)
        {
            List<bool> List = new List<bool>(BitArray.Length);

            foreach (bool Item in BitArray)
            {
                List.Add(Item);
            }

            return List;
        }


        public void Write(BinaryWriter Writer)
        {
            Writer.Write(Data);
            Writer.Write(Length);
        }
        public static BitArray Read(BinaryReader Reader)
        {
            return new BitArray
            (
                Reader.ReadByteArray(),
                Reader.ReadInt32()
            );
        }


        public int IndexOf(bool Item)
        {
            for (int i = 0; i < Length; i++)
            {
                if (this[i] == Item)
                {
                    return i;
                }
            }
            return -1;
        }

        public bool Contains(bool Item)
        {
            Predicate<byte> Condition = Item ? Byte => Byte != 0 : Byte => Byte == 0;
            foreach (byte Byte in Data)
            {
                if (Condition(Byte))
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(bool[] Array, int ArrayIndex)
        {
            ArgumentNullException.ThrowIfNull(Array);
            ArgumentOutOfRangeException.ThrowIfNegative(ArrayIndex);

            if (Array.Length - ArrayIndex < Length)
            {
                throw new ArgumentException("Target array too small");
            }

            for (int i = 0; i < Length; i++)
            {
                Array[ArrayIndex + i] = this[i];
            }
        }

        public BitArray Clone()
        {
            return new BitArray(Data, Length);
        }


        public IEnumerator<bool> GetEnumerator()
        {
            int ByteIndex = 0;
            int BitIndex = 0;

            for (int i = 0; i < Length; i++)
            {
                yield return Data[ByteIndex].GetBit(BitIndex);

                BitIndex++;

                if (BitIndex == 8)
                {
                    ByteIndex++;
                    BitIndex = 0;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}