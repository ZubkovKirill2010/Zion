namespace Zion.Serialization.TDC
{
    public readonly struct DataPosition : IBinarySerializable<DataPosition>
    {
        public readonly long Position;
        public readonly long Length;

        public DataPosition(long Position, long Length)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Position);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(Length);
            this.Position = Position;
            this.Length   = Length;
        }


        public void Write(BinaryWriter Writer)
        {
            Writer.Write(Position);
            Writer.Write(Length);
        }

        public static DataPosition Read(BinaryReader Reader)
        {
            return new DataPosition
            (
                Reader.ReadInt64(),
                Reader.ReadInt64()
            );
        }
    }
}