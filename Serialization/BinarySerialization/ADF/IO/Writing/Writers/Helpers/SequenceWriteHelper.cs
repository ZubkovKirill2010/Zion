using System.Buffers.Binary;
using System.Runtime.CompilerServices;
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
                WriteCompressed(Stream, Span);
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
                WriteCompressed(Stream, Span);
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
                WriteCompressed(Stream, Span);
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
                WriteCompressed(Stream, Span);
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
                WriteCompressed(Stream, Span);
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
                WriteCompressed(Stream, Span);
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
                //TODO: WriteVarInt Span<decimal>
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

        public static void Write(ArenaStream Stream, ReadOnlySpan<Half> Span, bool Compression)
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

        public static void Write(ArenaStream Stream, ReadOnlySpan<Index> Span, bool Compression)
        {
            if (Compression)
            {
                WriteCompressed(Stream, MemoryMarshal.Cast<Index, int>(Span));
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
                WriteCompressed(Stream, MemoryMarshal.Cast<Range, int>(Span));
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

        public static void Write(ArenaStream Stream, ReadOnlySpan<RGBColor> Span, bool Compression)
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

        public static void Write(ArenaStream Stream, ReadOnlySpan<RGBAColor> Span, bool Compression)
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

        public static void Write(ArenaStream Stream, ReadOnlySpan<Vector2> Span, bool Compression)
        {
            if (Compression)
            {
                WriteCompressed(Stream, MemoryMarshal.Cast<Vector2, float>(Span));
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
                WriteCompressed(Stream, MemoryMarshal.Cast<Vector2Int, int>(Span));
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
                WriteCompressed(Stream, MemoryMarshal.Cast<Vector3, float>(Span));
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
                WriteCompressed(Stream, MemoryMarshal.Cast<Vector3Int, int>(Span));
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


        private static void WriteCompressed(ArenaStream Stream, ReadOnlySpan<int> Span)
        {
            var Written = 0;

            Stream.Reserve(Span.Length * 5);
            Stream.UseSpan
            (
                new VarIntSpanState<int>(Span, ref Written),
                static (Target, State) =>
                {
                    ref var Destination = ref MemoryMarshal.GetReference(Target);
                    var Written = 0;

                    foreach (var Value in State.Source)
                    {
                        Written += WriteVarInt(ref Unsafe.Add(ref Destination, Written), Value);
                    }
                }
            );

            Stream.TrimExcess(Written);
        }

        private static void WriteCompressed(ArenaStream Stream, ReadOnlySpan<uint> Span)
        {
            var Written = 0;

            Stream.Reserve(Span.Length * 5);
            Stream.UseSpan
            (
                new VarIntSpanState<uint>(Span, ref Written),
                static (Target, State) =>
                {
                    ref var Destination = ref MemoryMarshal.GetReference(Target);
                    var Written = 0;

                    foreach (var Value in State.Source)
                    {
                        Written += WriteVarInt(ref Unsafe.Add(ref Destination, Written), Value);
                    }
                }
            );

            Stream.TrimExcess(Written);
        }

        private static void WriteCompressed(ArenaStream Stream, ReadOnlySpan<long> Span)
        {
            var Written = 0;

            Stream.Reserve(Span.Length * 5);
            Stream.UseSpan
            (
                new VarIntSpanState<long>(Span, ref Written),
                static (Target, State) =>
                {
                    ref var Destination = ref MemoryMarshal.GetReference(Target);
                    var Written = 0;

                    foreach (var Value in State.Source)
                    {
                        Written += WriteVarInt(ref Unsafe.Add(ref Destination, Written), Value);
                    }
                }
            );

            Stream.TrimExcess(Written);
        }

        private static void WriteCompressed(ArenaStream Stream, ReadOnlySpan<ulong> Span)
        {
            var Written = 0;

            Stream.Reserve(Span.Length * 5);
            Stream.UseSpan
            (
                new VarIntSpanState<ulong>(Span, ref Written),
                static (Target, State) =>
                {
                    ref var Destination = ref MemoryMarshal.GetReference(Target);
                    var Written = 0;

                    foreach (var Value in State.Source)
                    {
                        Written += WriteVarInt(ref Unsafe.Add(ref Destination, Written), Value);
                    }
                }
            );

            Stream.TrimExcess(Written);
        }


        private static void WriteCompressed(ArenaStream Stream, ReadOnlySpan<float> Span)
        {
            //TODO
        }

        private static void WriteCompressed(ArenaStream Stream, ReadOnlySpan<double> Span)
        {
            //TODO
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteVarInt(ref byte Destination, int Value)
        {
            return WriteVarInt(ref Destination, (uint)((Value << 1) ^ (Value >> 31)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteVarInt(ref byte Destination, uint value)
        {
            if (value < 0x80)
            {
                Destination = (byte)value;
                return 1;
            }

            int i = 0;
            while (value >= 0x80)
            {
                Unsafe.Add(ref Destination, i++) = (byte)(value | 0x80);
                value >>= 7;
            }
            Unsafe.Add(ref Destination, i++) = (byte)value;
            return i;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteVarInt(ref byte Destination, ulong Value)
        {
            if (Value < 0x80)
            {
                Destination = (byte)Value;
                return 1;
            }

            int i = 0;
            while (Value >= 0x80)
            {
                Unsafe.Add(ref Destination, i++) = (byte)(Value | 0x80);
                Value >>= 7;
            }
            Unsafe.Add(ref Destination, i++) = (byte)Value;
            return i;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteVarInt(ref byte Destination, long Value)
        {
            return WriteVarInt(ref Destination, (ulong)((Value << 1) ^ (Value >> 63)));
        }


        private ref struct VarIntSpanState<T> where T : unmanaged
        {
            public ReadOnlySpan<T> Source;
            public ref int Written;

            public VarIntSpanState(ReadOnlySpan<T> Source, ref int Written)
            {
                this.Source = Source;
                this.Written = ref Written;
            }
        }
    }
}