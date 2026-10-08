using System.Buffers.Binary;
using System.Numerics;
using System.Runtime.InteropServices;
using Zion.Vectors;
using Vector2 = Zion.Vectors.Vector2;
using Vector3 = Zion.Vectors.Vector3;

namespace Zion.Serialization.ADF
{
    //_Span
    public abstract partial class BaseADFWriter
    {
        public void Write(string Name, ReadOnlySpan<bool> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<byte> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<sbyte> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<short> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<int> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<long> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<ushort> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<uint> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<ulong> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<char> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<float> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<double> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<decimal> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<string> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<Half> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<Index> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<Range> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<BigInteger> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<RGBColor> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<RGBAColor> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<Vector2> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<Vector2Int> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<Vector3> Span)
        {
            Write(Name, null, Span);
        }

        public void Write(string Name, ReadOnlySpan<Vector3Int> Span)
        {
            Write(Name, null, Span);
        }


        private void Write(string Name, object? Collection, ReadOnlySpan<bool> Span)
        {
            WritePrimitives<bool>
            (
                new
                (
                    Name, ADFPrimitives.Boolean,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        Stream.UseSpan
                        (
                            Span,
                            (Target, Source) =>
                            {
                                int Count = Source.Length;
                                int ByteCount = (Count + 7) >> 3;

                                int ByteIndex = 0;
                                int BitIndex = 0;

                                byte Current = 0;

                                for (int i = 0; i < Count; i++)
                                {
                                    if (Source[i])
                                    {
                                        Current |= (byte)(1 << BitIndex);
                                    }

                                    BitIndex++;

                                    if (BitIndex == 8)
                                    {
                                        Target[ByteIndex++] = Current;
                                        Current = 0;
                                        BitIndex = 0;
                                    }
                                }

                                if (BitIndex > 0)
                                {
                                    Target[ByteIndex] = Current;
                                }
                            }
                        );
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<byte> Span)
        {
            WritePrimitives<byte>
            (
                new
                (
                    Name, ADFPrimitives.Byte,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        Stream.UseSpan
                        (
                            Span,
                            static (Target, Source) =>
                            {
                                Source.CopyTo(Target);
                            }
                        );
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<sbyte> Span)
        {
            WritePrimitives<sbyte>
            (
                new
                (
                    Name, ADFPrimitives.SByte,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        Stream.UseSpan
                        (
                            Span,
                            static (Target, Source) =>
                            {
                                MemoryMarshal.AsBytes(Source).CopyTo(Target);
                            }
                        );
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<short> Span)
        {
            WritePrimitives<short>
            (
                new
                (
                    Name, ADFPrimitives.Int16,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        Stream.UseSpan
                        (
                            Span,
                            static (Target, Source) =>
                            {
                                if (BitConverter.IsLittleEndian)
                                {
                                    MemoryMarshal.AsBytes(Source).CopyTo(Target);
                                }
                                else
                                {
                                    BinaryPrimitives.ReverseEndianness
                                    (
                                        Source,
                                        MemoryMarshal.Cast<byte, short>(Target)
                                    );
                                }
                            }
                        );   
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<int> Span)
        {
            WritePrimitives<int>
            (
                new
                (
                    Name, ADFPrimitives.Int32,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {
                            //TODO: WriteCompressed<int>
                        }
                        else
                        {
                            Stream.UseSpan
                            (
                                Span,
                                static (Target, Source) =>
                                {
                                    if (BitConverter.IsLittleEndian)
                                    {
                                        MemoryMarshal.AsBytes(Source).CopyTo(Target);
                                    }
                                    else
                                    {
                                        BinaryPrimitives.ReverseEndianness
                                        (
                                            Source,
                                            MemoryMarshal.Cast<byte, int>(Target)
                                        );
                                    }
                                }
                            );
                        }
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<long> Span)
        {
            WritePrimitives<long>
            (
                new
                (
                    Name, ADFPrimitives.Int64,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {
                            //TODO: WriteCompressed<long>
                        }
                        else
                        {
                            Stream.UseSpan
                            (
                                Span,
                                static (Target, Source) =>
                                {
                                    if (BitConverter.IsLittleEndian)
                                    {
                                        MemoryMarshal.AsBytes(Source).CopyTo(Target);
                                    }
                                    else
                                    {
                                        BinaryPrimitives.ReverseEndianness
                                        (
                                            Source,
                                            MemoryMarshal.Cast<byte, long>(Target)
                                        );
                                    }
                                }
                            );
                        }
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<ushort> Span)
        {
            WritePrimitives<ushort>
            (
                new
                (
                    Name, ADFPrimitives.UInt16,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        Stream.UseSpan
                        (
                            Span,
                            static (Target, Source) =>
                            {
                                if (BitConverter.IsLittleEndian)
                                {
                                    MemoryMarshal.AsBytes(Source).CopyTo(Target);
                                }
                                else
                                {
                                    BinaryPrimitives.ReverseEndianness
                                    (
                                        Source,
                                        MemoryMarshal.Cast<byte, ushort>(Target)
                                    );
                                }
                            }
                        );
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<uint> Span)
        {
            WritePrimitives<uint>
            (
                new
                (
                    Name, ADFPrimitives.UInt32,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {
                            //TODO: WriteCompressed<uint>
                        }
                        else
                        {
                            Stream.UseSpan
                            (
                                Span,
                                static (Target, Source) =>
                                {
                                    if (BitConverter.IsLittleEndian)
                                    {
                                        MemoryMarshal.AsBytes(Source).CopyTo(Target);
                                    }
                                    else
                                    {
                                        BinaryPrimitives.ReverseEndianness
                                        (
                                            Source,
                                            MemoryMarshal.Cast<byte, uint>(Target)
                                        );
                                    }
                                }
                            );
                        }
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<ulong> Span)
        {
            WritePrimitives<ulong>
            (
                new
                (
                    Name, ADFPrimitives.UInt64,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {
                            //TODO: WriteCompressed<ulong>
                        }
                        else
                        {
                            Stream.UseSpan
                            (
                                Span,
                                static (Target, Source) =>
                                {
                                    if (BitConverter.IsLittleEndian)
                                    {
                                        MemoryMarshal.AsBytes(Source).CopyTo(Target);
                                    }
                                    else
                                    {
                                        BinaryPrimitives.ReverseEndianness
                                        (
                                            Source,
                                            MemoryMarshal.Cast<byte, ulong>(Target)
                                        );
                                    }
                                }
                            );
                        }
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<char> Span)
        {
            WritePrimitives<char>
            (
                new
                (
                    Name, ADFPrimitives.Char,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        Stream.UseSpan
                        (
                            Span,
                            static (Target, Source) =>
                            {
                                if (BitConverter.IsLittleEndian)
                                {
                                    MemoryMarshal.AsBytes(Source).CopyTo(Target);
                                }
                                else
                                {
                                    BinaryPrimitives.ReverseEndianness
                                    (
                                        MemoryMarshal.Cast<char, short>(Source),
                                        MemoryMarshal.Cast<byte, short>(Target)
                                    );
                                }
                            }
                        );
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<float> Span)
        {
            WritePrimitives<float>
            (
                new
                (
                    Name, ADFPrimitives.Single,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {

                        }
                        else
                        {
                            Stream.UseSpan
                            (
                                Span,
                                static (Target, Source) =>
                                {
                                    if (BitConverter.IsLittleEndian)
                                    {
                                        MemoryMarshal.AsBytes(Source).CopyTo(Target);
                                    }
                                    else
                                    {
                                        BinaryPrimitives.ReverseEndianness
                                        (
                                            MemoryMarshal.Cast<float, int>(Source),
                                            MemoryMarshal.Cast<byte, int>(Target)
                                        );
                                    }
                                }
                            );
                        }
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<double> Span)
        {
            WritePrimitives<double>
            (
                new
                (
                    Name, ADFPrimitives.Double,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {
                            //TODO: Write Span<double>
                        }
                        else
                        {
                            Stream.UseSpan
                            (
                                Span,
                                static (Target, Source) =>
                                {
                                    if (BitConverter.IsLittleEndian)
                                    {
                                        MemoryMarshal.AsBytes(Source).CopyTo(Target);
                                    }
                                    else
                                    {
                                        BinaryPrimitives.ReverseEndianness
                                        (
                                            MemoryMarshal.Cast<double, long>(Source),
                                            MemoryMarshal.Cast<byte, long>(Target)
                                        );
                                    }
                                }
                            );
                        }
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<decimal> Span) 
        {
            WritePrimitives<decimal>
            (
                new
                (
                    Name, ADFPrimitives.Decimal,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {
                            //TODO: Write Span<decimal>
                        }
                        else
                        {
                            Stream.UseSpan
                            (
                                Span,
                                static (Target, Source) =>
                                {
                                    if (BitConverter.IsLittleEndian)
                                    {
                                        MemoryMarshal.AsBytes(Source).CopyTo(Target);
                                    }
                                    else
                                    {
                                        var UIntSource = MemoryMarshal.Cast<decimal, uint>(Source);
                                        var UIntTarget = MemoryMarshal.Cast<byte, uint>(Target);

                                        for (int i = 0; i < UIntSource.Length; i++)
                                        {
                                            UIntTarget[i] = BinaryPrimitives.ReverseEndianness(UIntSource[i]);
                                        }
                                    }
                                }
                            );
                        }
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<string> Span)
        {
            WritePrimitives<string>
            (
                new
                (
                    Name, ADFPrimitives.String,
                    Collection, Span,
                    (Stream, Span, Compression) =>
                    {
                        //Optimize: Write in Span
                        var Registry = StringRegistry;

                        if (Compression)
                        {
                            foreach (var String in Span)
                            {
                                var Id = Registry.GetOrAdd(String);
                                Stream.Write7BitEncodedUInt(Id);
                            }
                        }
                        else
                        {
                            foreach (var String in Span)
                            {
                                var Id = Registry.GetOrAdd(String);
                                Stream.Write7BitEncodedUInt(Id);
                            }
                        }
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<Half> Span)
        {
            WritePrimitives<Half>
            (
                new
                (
                    Name, ADFPrimitives.Half,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {
                           //TODO: Write Span<Half>
                        }
                        else
                        {
                            Stream.UseSpan
                            (
                                Span,
                                static (Target, Source) =>
                                {
                                    if (BitConverter.IsLittleEndian)
                                    {
                                        MemoryMarshal.AsBytes(Source).CopyTo(Target);
                                    }
                                    else
                                    {
                                        BinaryPrimitives.ReverseEndianness
                                        (
                                            MemoryMarshal.Cast<Half, short>(Source),
                                            MemoryMarshal.Cast<byte, short>(Target)
                                        );
                                    }
                                }
                            );
                        }
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<Index> Span)
        {
            WritePrimitives<Index>
            (
                new
                (
                    Name, ADFPrimitives.Index,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {
                            //TODO: Write Span<Index>
                        }
                        else
                        {
                            Stream.UseSpan
                            (
                                Span,
                                static (Target, Source) =>
                                {
                                    if (BitConverter.IsLittleEndian)
                                    {
                                        MemoryMarshal.AsBytes(Source).CopyTo(Target);
                                    }
                                    else
                                    {
                                        BinaryPrimitives.ReverseEndianness
                                        (
                                            MemoryMarshal.Cast<Index, int>(Source),
                                            MemoryMarshal.Cast<byte, int>(Target)
                                        );
                                    }
                                }
                            );
                        }
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<Range> Span)
        {
            WritePrimitives<Range>
            (
                new
                (
                    Name, ADFPrimitives.Range,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {
                            //TODO: Write Span<Range>
                        }
                        else
                        {
                            Stream.UseSpan
                            (
                                Span,
                                static (Target, Source) =>
                                {
                                    if (BitConverter.IsLittleEndian)
                                    {
                                        MemoryMarshal.AsBytes(Source).CopyTo(Target);
                                    }
                                    else
                                    {
                                        BinaryPrimitives.ReverseEndianness
                                        (
                                            MemoryMarshal.Cast<Range, int>(Source),
                                            MemoryMarshal.Cast<byte, int>(Target)
                                        );
                                    }
                                }
                            );
                        }
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<BigInteger> Span)
        {
            WritePrimitives<BigInteger>
            (
                new
                (
                    Name, ADFPrimitives.BigInteger,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {

                        }
                        else
                        {

                        }
                        //TODO: Write Span<BigInteger>
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<RGBColor> Span)
        {
            WritePrimitives<RGBColor>
            (
                new
                (
                    Name, ADFPrimitives.RGB,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {
                            //TODO: Write Span<RGBColor>
                        }
                        else
                        {
                            Stream.UseSpan
                            (
                                Span,
                                static (Target, Source) =>
                                {
                                    MemoryMarshal.AsBytes(Source).CopyTo(Target);
                                }
                            );
                        }
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<RGBAColor> Span)
        {
            WritePrimitives<RGBAColor>
            (
                new
                (
                    Name, ADFPrimitives.RGBA,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {
                            //TODO: Write Span<RGBAColor>
                        }
                        else
                        {
                            Stream.UseSpan
                            (
                                Span,
                                static (Target, Source) =>
                                {
                                    MemoryMarshal.AsBytes(Source).CopyTo(Target);
                                }
                            );
                        }
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<Vector2> Span)
        {
            WritePrimitives<Vector2>
            (
                new
                (
                    Name, ADFPrimitives.Vector2,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {
                            //TODO: Write Span<Vector2>
                        }
                        else
                        {
                            Stream.UseSpan
                            (
                                Span,
                                static (Target, Source) =>
                                {
                                    if (BitConverter.IsLittleEndian)
                                    {
                                        MemoryMarshal.AsBytes(Source).CopyTo(Target);
                                    }
                                    else
                                    {
                                        BinaryPrimitives.ReverseEndianness
                                        (
                                            MemoryMarshal.Cast<Vector2, int>(Source),
                                            MemoryMarshal.Cast<byte, int>(Target)
                                        );
                                    }
                                }
                            );
                        }
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<Vector2Int> Span)
        {
            WritePrimitives<Vector2Int>
            (
                new
                (
                    Name, ADFPrimitives.Vector2Int,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {
                            //TODO: Write Span<Vector2Int>
                        }
                        else
                        {
                            Stream.UseSpan
                            (
                                Span,
                                static (Target, Source) =>
                                {
                                    if (BitConverter.IsLittleEndian)
                                    {
                                        MemoryMarshal.AsBytes(Source).CopyTo(Target);
                                    }
                                    else
                                    {
                                        BinaryPrimitives.ReverseEndianness
                                        (
                                            MemoryMarshal.Cast<Vector2Int, int>(Source),
                                            MemoryMarshal.Cast<byte, int>(Target)
                                        );
                                    }
                                }
                            );
                        }
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<Vector3> Span)
        {
            WritePrimitives<Vector3>
            (
                new
                (
                    Name, ADFPrimitives.Vector3,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {
                            //TODO: Write Span<Vector3>
                        }
                        else
                        {
                            Stream.UseSpan
                            (
                                Span,
                                static (Target, Source) =>
                                {
                                    if (BitConverter.IsLittleEndian)
                                    {
                                        MemoryMarshal.AsBytes(Source).CopyTo(Target);
                                    }
                                    else
                                    {
                                        BinaryPrimitives.ReverseEndianness
                                        (
                                            MemoryMarshal.Cast<Vector3, int>(Source),
                                            MemoryMarshal.Cast<byte, int>(Target)
                                        );
                                    }
                                }
                            );
                        }
                    }
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<Vector3Int> Span)
        {
            WritePrimitives<Vector3Int>
            (
                new
                (
                    Name, ADFPrimitives.Vector3Int,
                    Collection, Span,
                    static (Stream, Span, Compression) =>
                    {
                        if (Compression)
                        {
                            //TODO: Write Span<Vector3Int>
                        }
                        else
                        {
                            Stream.UseSpan
                            (
                                Span,
                                static (Target, Source) =>
                                {
                                    if (BitConverter.IsLittleEndian)
                                    {
                                        MemoryMarshal.AsBytes(Source).CopyTo(Target);
                                    }
                                    else
                                    {
                                        BinaryPrimitives.ReverseEndianness
                                        (
                                            MemoryMarshal.Cast<Vector3Int, int>(Source),
                                            MemoryMarshal.Cast<byte, int>(Target)
                                        );
                                    }
                                }
                            );
                        }
                    }
                )
            );
        }


        private void WritePrimitives<T>(CollectionWriteContext<T> WriteContext)
        {
            ThrowIfDisposed();

            var NameId = StringRegistry.GetOrAdd(WriteContext.Name);
            var Target = GetStreamGroup(WriteContext.Name, in NameId, in WriteContext.FormatId);

            if (WriteContext.Collection is not null
                && References.TryGetReference(WriteContext.Collection, out var Reference))
            {
                Target.BaseStream.WriteCompressed(Context, Reference.Id);
            }
            else
            {
                WriteContext.Write(Context, Target);
            }

            OnWrited(WriteContext.Name, in NameId, in WriteContext.FormatId);
        }
    }
}