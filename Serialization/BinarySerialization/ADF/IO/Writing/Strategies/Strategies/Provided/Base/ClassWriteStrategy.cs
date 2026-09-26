namespace Zion.Serialization.ADF
{
    internal abstract class ClassWriteStrategy<T> : ProvidedWriteStrategy<T>
    {
        public ClassWriteStrategy(ADFWritingContext Context)
            : base(Context) { }


        protected sealed override StreamGroup GetGroupForData(StreamGroup BaseGroup)
        {
            var NewGroup = new StreamGroup(Context.Arena.GetStream(1));
            BaseGroup.Add(NewGroup);
            return NewGroup;
        }

        protected sealed override bool TryWriteReference(ArenaStream BaseStream, T Value)
        {
            if (Context.Registries.References.TryGetReference(Value, out Reference Reference))
            {
                BaseStream.WriteCompressed(Context, Reference.Id);
                return true;
            }
            return false;
        }

        protected sealed override void Write(StreamGroup Base, ADFObjectWriter Writer, T Value)
        {
            Base.BaseStream.WriteCompressed(Context, Base.Length + 1L);
            WriteValue(Writer, Value);
        }

        protected sealed override void OnWrited(uint FormatId, T Value)
        {
            var Definition = new DataDefinition
            (
                FormatId,
                Context.CurrentPage,
                Context.CurrentPosition
            );

            Context.Registries.References.Add(Value!, Definition);
        }


        protected abstract void WriteValue(ADFObjectWriter Writer, T Value);
    }
}