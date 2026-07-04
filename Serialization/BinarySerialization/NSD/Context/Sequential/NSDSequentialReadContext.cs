using System.Text;

namespace Zion.Serialization.NSD
{
    public sealed class NSDSequentialReadContext : NSDReadContext
    {
        private readonly byte[] Buffer;
        private readonly int Length;

        public NSDSequentialReadContext(Stream Stream) : base(Stream)
        {
            using MemoryStream MemoryStream = new MemoryStream();
            Stream.CopyTo(MemoryStream);

            Buffer = MemoryStream.ToArray();
            Length = Buffer.Length;
        }

        internal protected override void ReadAll(NSDBatchReader Batch)
        {
            using MemoryStream MemoryStream = new MemoryStream(Buffer);
            using BinaryReader Reader       = new BinaryReader(MemoryStream, Encoding.UTF8, false);

            while (MemoryStream.Position < Length)
            {
                long RecordStart = MemoryStream.Position;

                string Key = Reader.ReadString();
                uint Length = Reader.ReadUInt32();

                long DataStart = MemoryStream.Position;

                if (!Batch.TryRead(Key, MemoryStream))
                {
                    MemoryStream.Seek(Length, SeekOrigin.Current);
                }

                if (MemoryStream.Position > DataStart + Length)
                {
                    throw new InvalidDataException
                    (
                        $"Read beyond data boundary for key '{Key}'. " +
                        $"Expected {DataStart + Length}, got {MemoryStream.Position}"
                    );
                }

                if (MemoryStream.Position < DataStart + Length)
                {
                    MemoryStream.Seek(DataStart + Length - MemoryStream.Position, SeekOrigin.Current);
                }

                if (MemoryStream.Position == RecordStart)
                {
                    throw new InvalidOperationException
                    (
                        $"No progress made while reading NSD file at position {RecordStart}"
                    );
                }
            }
        }
    }
}