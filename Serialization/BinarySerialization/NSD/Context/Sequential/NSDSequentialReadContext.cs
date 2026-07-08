using System.Text;

namespace Zion.Serialization.NSD
{
    public sealed class NSDSequentialReadContext : NSDReadContext
    {
        public NSDSequentialReadContext(Stream Stream) : base(Stream)
        {

        }

        internal protected override void ReadAll(NSDBatchReader Batch)
        {
            BinaryReader Reader = new BinaryReader(Stream, Encoding.UTF8, true);

            while (Stream.Position < Stream.Length)
            {
                string Key = Reader.ReadString();
                uint Length = Reader.ReadUInt32();

                if (!Batch.TryRead(Key, Stream))
                {
                    if (Stream.CanSeek)
                    {
                        Stream.Seek(Length, SeekOrigin.Current);
                    }
                    else
                    {
                        byte[] Discard = new byte[Math.Min(Length, 81920)];
                        long Remaining = Length;
                        while (Remaining > 0)
                        {
                            int Read = Stream.Read(Discard, 0, (int)Math.Min(Remaining, Discard.Length));
                            if (Read == 0) throw new EndOfStreamException();
                            Remaining -= Read;
                        }
                    }
                }
            }
        }
    }
}