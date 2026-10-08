using System.Buffers.Binary;
using System.Numerics;
using System.Runtime.InteropServices;
using Zion.Vectors;
using Vector2 = Zion.Vectors.Vector2;
using Vector3 = Zion.Vectors.Vector3;

namespace Zion.Serialization.ADF
{
    internal static class SequenceWriteHelper
    {
        public static void Write(ArenaStream Stream, ReadOnlySpan<bool> Span, bool Compression)
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

        public static void Write(ArenaStream Stream, ReadOnlySpan<byte> Span, bool Compression)
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

        public static void Write(ArenaStream Stream, ReadOnlySpan<sbyte> Span, bool Compression)
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

        public static void Write(ArenaStream Stream, ReadOnlySpan<short> Span, bool Compression)
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

        public static void Write(ArenaStream Stream, ReadOnlySpan<int> Span, bool Compression)
        {
            if (Compression)
            {
                //TODO: Write Enumerator<int>
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

        public static void Write(ArenaStream Stream, ReadOnlySpan<long> Span, bool Compression)
        {
            if (Compression)
            {
                //TODO: Write Enumerator<long>
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

        public static void Write(ArenaStream Stream, ReadOnlySpan<ushort> Span, bool Compression)
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

        public static void Write(ArenaStream Stream, ReadOnlySpan<uint> Span, bool Compression)
        {
            if (Compression)
            {
                //TODO: Write Enumerator<uint>
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

        public static void Write(ArenaStream Stream, ReadOnlySpan<ulong> Span, bool Compression)
        {
            if (Compression)
            {
                //TODO: Write Enumerator<ulong>
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

        public static void Write(ArenaStream Stream, ReadOnlySpan<char> Span, bool Compression)
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

        public static void Write(ArenaStream Stream, ReadOnlySpan<float> Span, bool Compression)
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

        public static void Write(ArenaStream Stream, ReadOnlySpan<double> Span, bool Compression)
        {
            if (Compression)
            {
                //TODO: Write Enumerator<double>
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

        public static void Write(ArenaStream Stream, ReadOnlySpan<decimal> Span, bool Compression)
        {
            if (Compression)
            {
                //TODO: Write Enumerator<decimal>
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

        public static void Write(ArenaStream Stream, ReadOnlySpan<string> Span, bool Compression)
        {
            //Optimize: Write in Enumerator
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

        public static void Write(ArenaStream Stream, ReadOnlySpan<Half> Span, bool Compression)
        {
            if (Compression)
            {
                //TODO: Write Enumerator<Half>
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

        public static void Write(ArenaStream Stream, ReadOnlySpan<Index> Span, bool Compression)
        {
            if (Compression)
            {
                //TODO: Write Enumerator<Index>
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

        public static void Write(ArenaStream Stream, ReadOnlySpan<Range> Span, bool Compression)
        {
            if (Compression)
            {
                //TODO: Write Enumerator<Range>
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

        //---------------
        public static void Write(ArenaStream Stream, ReadOnlySpan<BigInteger> Span, bool Compression)
        {
            
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<RGBColor> Span, bool Compression)
        {
            if (Compression)
            {
                //TODO: Write Enumerator<RGBColor>
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

        public static void Write(ArenaStream Stream, ReadOnlySpan<RGBAColor> Span, bool Compression)
        {
            if (Compression)
            {
                //TODO: Write Enumerator<RGBAColor>
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

        public static void Write(ArenaStream Stream, ReadOnlySpan<Vector2> Span, bool Compression)
        {
            if (Compression)
            {
                //TODO: Write Enumerator<Vector2>
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

        public static void Write(ArenaStream Stream, ReadOnlySpan<Vector2Int> Span, bool Compression)
        {
            if (Compression)
            {
                //TODO: Write Enumerator<Vector2Int>
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

        public static void Write(ArenaStream Stream, ReadOnlySpan<Vector3> Span, bool Compression)
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
                                MemoryMarshal.Cast<Vector3, int>(Source),
                                MemoryMarshal.Cast<byte, int>(Target)
                            );
                        }
                    }
                );
            }
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<Vector3Int> Span, bool Compression)
        {
            if (Compression)
            {
                //TODO: Write Enumerator<Vector3Int>
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
    }
}