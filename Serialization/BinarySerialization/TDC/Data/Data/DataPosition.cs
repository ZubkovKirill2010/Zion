namespace Zion.Serialization.TDC
{
    public readonly struct DataPosition : IBinarySerializable<DataPosition>
    {
        public readonly ulong Start;
        public readonly ulong Length;

        public DataPosition(ulong Start, ulong Length)
        {
            this.Start = Start;
            this.Length   = Length;
        }

        public DataPosition(long Start, long Length)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Start);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(Length);
            this.Start  = (ulong)Start;
            this.Length = (ulong)Length;
        }


        public void Write(BinaryWriter Writer)
        {
            Writer.WriteVarInt(Start);
            Writer.WriteVarInt(Length);
        }

        public static DataPosition Read(BinaryReader Reader)
        {
            return new DataPosition
            (
                Reader.ReadVarInt(),
                Reader.ReadVarInt()
            );
        }
    }
}