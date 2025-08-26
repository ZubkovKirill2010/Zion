using System.Collections;

namespace Zion
{
    public class BitArray : IBinaryObject<BitArray>, IList<bool>
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

        public void Insert(int ndex, bool Item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int Index)
        {
            throw new NotImplementedException();
        }

        public void Add(bool Item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(bool Item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(bool[] Array, int ArrayIndex)
        {
            throw new NotImplementedException();
        }

        public BitArray Clone()
        {
            return new BitArray(Data, Length);
        }

        public bool Remove(bool Item)
        {
            throw new NotImplementedException();
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
                    ByteIndex = 0;
                }
                else
                {
                    ByteIndex++;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}