namespace Zion.Serialization.ADF
{
    internal ref struct CollectionWriteContext<T>
    {
        public delegate void WriteAction(ArenaStream Stream, ReadOnlySpan<T> Span, bool Compression);

        public readonly string Name;
        public readonly uint   FormatId;
        public readonly object? Collection;

        private readonly int Capacity;
        private readonly ReadOnlySpan<T> Span;        
        private readonly WriteAction Writing;


        public CollectionWriteContext(string Name, uint FormatId, object? Collection, ReadOnlySpan<T> Span, WriteAction Writing)
        {
            this.Name = Name.NotNull();
            this.FormatId = FormatId | ADFPrimitives.Sequence;

            this.Collection = Collection.NotNull();
            this.Span = Span;

            this.Writing = Writing.NotNull();

            Capacity = ADFPrimitives.TryGetInfo(FormatId, out var PrimitiveInfo)
                ? Span.Length * PrimitiveInfo.Size + 4
                : Span.Length * 8;
        }


        public void Write(ADFWritingContext Context, StreamGroup Target)
        {
            var Link = Reference.CreateNewReference(Target.ChildsLength);
            var Stream = Context.Arena.GetStream(Capacity);

            Stream.Write(Span.Length);

            if (Context.Compression)
            {
                Target.BaseStream.Write7BitEncodedUInt64(Link);
                Writing(Stream, Span, true);
            }
            else
            {
                Target.BaseStream.Write(Link);
                Writing(Stream, Span, false);
            }

            Target.Add(Stream);
        }
    }
}