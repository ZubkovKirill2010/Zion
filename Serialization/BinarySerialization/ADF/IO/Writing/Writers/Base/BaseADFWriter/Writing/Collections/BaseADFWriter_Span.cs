using System.Numerics;
using System.Runtime.CompilerServices;
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

        public void Write<T>(string Name, ReadOnlySpan<T> Span)
        {
            Write(Name, null, Span);
        }


        private void Write(string Name, object? Collection, ReadOnlySpan<bool> Span)
        {
            WriteSpan<bool>
            (
                new
                (
                    Name, ADFPrimitives.Boolean,
                    Collection, Span, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<byte> Span)
        {
            WriteSpan<byte>
            (
                new
                (
                    Name, ADFPrimitives.Byte,
                    Collection, Span, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<sbyte> Span)
        {
            WriteSpan<sbyte>
            (
                new
                (
                    Name, ADFPrimitives.SByte,
                    Collection, Span, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<short> Span)
        {
            WriteSpan<short>
            (
                new
                (
                    Name, ADFPrimitives.Int16,
                    Collection, Span, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<int> Span)
        {
            WriteSpan<int>
            (
                new
                (
                    Name, ADFPrimitives.Int32,
                    Collection, Span, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<long> Span)
        {
            WriteSpan<long>
            (
                new
                (
                    Name, ADFPrimitives.Int64,
                    Collection, Span, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<ushort> Span)
        {
            WriteSpan<ushort>
            (
                new
                (
                    Name, ADFPrimitives.UInt16,
                    Collection, Span, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<uint> Span)
        {
            WriteSpan<uint>
            (
                new
                (
                    Name, ADFPrimitives.UInt32,
                    Collection, Span, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<ulong> Span)
        {
            WriteSpan<ulong>
            (
                new
                (
                    Name, ADFPrimitives.UInt64,
                    Collection, Span, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<char> Span)
        {
            WriteSpan<char>
            (
                new
                (
                    Name, ADFPrimitives.Char,
                    Collection, Span, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<float> Span)
        {
            WriteSpan<float>
            (
                new
                (
                    Name, ADFPrimitives.Single,
                    Collection, Span, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<double> Span)
        {
            WriteSpan<double>
            (
                new
                (
                    Name, ADFPrimitives.Double,
                    Collection, Span, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<decimal> Span) 
        {
            WriteSpan<decimal>
            (
                new
                (
                    Name, ADFPrimitives.Decimal,
                    Collection, Span, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<string> Span)
        {
            ThrowIfDisposed();

            var Registry = StringRegistry;

            var NameId = StringRegistry.GetOrAdd(Name);
            var Target = GetStreamGroup(Name, in NameId, ADFPrimitives.String | ADFPrimitives.Sequence);
            var Stream = Target.BaseStream;

            if (Context.Compression)
            {
                for (int i = 0; i < Span.Length; i++)
                {
                    Stream.Write7BitEncodedUInt(Registry.GetOrAdd(Span[i]));
                }
            }
            else
            {
                for (int i = 0; i < Span.Length; i++)
                {
                    Stream.Write(Registry.GetOrAdd(Span[i]));
                }
            }

            OnWrited(Name, in NameId, ADFPrimitives.String | ADFPrimitives.Sequence);
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<Half> Span)
        {
            WriteSpan<Half>
            (
                new
                (
                    Name, ADFPrimitives.Half,
                    Collection, Span, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<Index> Span)
        {
            WriteSpan<Index>
            (
                new
                (
                    Name, ADFPrimitives.Index,
                    Collection, Span, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<Range> Span)
        {
            WriteSpan<Range>
            (
                new
                (
                    Name, ADFPrimitives.Range,
                    Collection, Span, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<BigInteger> Span)
        {
            ThrowIfDisposed();

            var NameId = StringRegistry.GetOrAdd(Name);
            var Target = GetStreamGroup(Name, in NameId, ADFPrimitives.BigInteger | ADFPrimitives.Sequence);

            if (Collection is not null
                && References.TryGetReference(Collection, out var Reference))
            {
                Target.BaseStream.WriteCompressed(Context, Reference.Id);
            }
            else
            {
                var Base   = Target.BaseStream;
                var Stream = Context.Arena.GetStream(Span.Length << 3);
                var Offset = Target.ChildsLength;

                for (int i = 0; i < Span.Length; i++)
                {
                    Base.Write(Reference.CreateNewReference(Offset + Stream.Length));
                    Stream.Write(Span[i]);
                }

                Target.Add(Stream);
            }

            OnWrited(Name, in NameId, ADFPrimitives.BigInteger | ADFPrimitives.Sequence);
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<RGBColor> Span)
        {
            WriteSpan<RGBColor>
            (
                new
                (
                    Name, ADFPrimitives.RGB,
                    Collection, Span, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<RGBAColor> Span)
        {
            WriteSpan<RGBAColor>
            (
                new
                (
                    Name, ADFPrimitives.RGBA,
                    Collection, Span, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<Vector2> Span)
        {
            WriteSpan<Vector2>
            (
                new
                (
                    Name, ADFPrimitives.Vector2,
                    Collection, Span, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<Vector2Int> Span)
        {
            WriteSpan<Vector2Int>
            (
                new
                (
                    Name, ADFPrimitives.Vector2Int,
                    Collection, Span, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<Vector3> Span)
        {
            WriteSpan<Vector3>
            (
                new
                (
                    Name, ADFPrimitives.Vector3,
                    Collection, Span, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<Vector3Int> Span)
        {
            WriteSpan<Vector3Int>
            (
                new
                (
                    Name, ADFPrimitives.Vector3Int,
                    Collection, Span, SequenceWriteHelper.Write
                )
            );
        }

        private void Write<T>(string Name, object? Collection, ReadOnlySpan<T> Span)
        {
            ThrowIfDisposed();

            var Type   = typeof(T);
            var NameId = StringRegistry.GetOrAdd(Name.NotNull());

            if (Type.IsEnum)
            {
                WriteEnums(Name, NameId, Span);
                return;
            }

            var Context = this.Context;

            var Entry    = Context.WriteStrategies.GetEntry<T>(Context, Type);
            var FormatId = Entry.FormatId | ADFPrimitives.Sequence;
            var Strategy = Entry.Strategy;

            var Target = GetStreamGroup(Name, in NameId, in FormatId);

            //Optimize
            foreach (var Item in Span)
            {
                Strategy.Write(Context, Target, Item);
            }

            OnWrited(Name, in NameId, in FormatId);
        }

        private void WriteEnums<T>(string Name, in uint NameId, ReadOnlySpan<T> Span)
        {
            var FormatId = ADFPrimitives.Sequence | TypeAssociation.GetOrAdd
            (
                typeof(T),
                () => FormatRegistry.Add(DataFormat.GetEnumFormat<T>())
            );

            var ItemSize = Unsafe.SizeOf<T>();
            var Target = GetStreamGroup(Name, in NameId, in FormatId);
            var Stream = Context.Arena.GetStream(ItemSize * Span.Length);

            Stream.Write(Span.Length);

            if (Span.Length > 0)
            {
                ref var FirstByte = ref Unsafe.As<T, byte>
                (
                    ref MemoryMarshal.GetReference(Span)
                );

                var Bytes = MemoryMarshal.CreateReadOnlySpan
                (
                    ref FirstByte,
                    ItemSize * Span.Length
                );

                Stream.UseSpan
                (
                    Bytes,
                    static (Target, Source) => Source.CopyTo(Target)
                );
            }

            Target.Add(Stream);

            OnWrited(Name, in NameId, in FormatId);
        }


        private void WriteSpan<T>(SpanWriteContext<T> WriteContext)
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