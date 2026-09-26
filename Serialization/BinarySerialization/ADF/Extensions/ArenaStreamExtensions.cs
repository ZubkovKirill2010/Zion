namespace Zion.Serialization.ADF
{
    internal static class ArenaStreamExtensions
    {
        extension(ArenaStream Stream)
        {
            public void WriteCompressedZero(ADFWritingContext Context)
            {
                if (Context.Options.Compression)
                {
                    Stream.Write((byte)0);
                }
                else
                {
                    Stream.Write(0u);
                }
            }

            public void WriteCompressed(ADFWritingContext Context, uint Value)
            {
                if (Context.Options.Compression)
                {
                    Stream.Write7BitEncodedUInt(Value);
                }
                else
                {
                    Stream.Write(Value);
                }
            }

            public void WriteCompressed(ADFWritingContext Context, long Value)
            {
                if (Context.Options.Compression)
                {
                    Stream.Write7BitEncodedInt64(Value);
                }
                else
                {
                    Stream.Write(Value);
                }
            }
        }
    }
}