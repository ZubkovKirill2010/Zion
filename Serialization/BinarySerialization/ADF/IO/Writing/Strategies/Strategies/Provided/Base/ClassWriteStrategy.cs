namespace Zion.Serialization.ADF
{
    internal abstract class ClassWriteStrategy<T> : ProvidedWriteStrategy<T>
    {
        public ClassWriteStrategy(ADFWritingContext Context, uint FormatId)
            : base(Context, FormatId) { }


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
            var Link = Reference.CreateNewReference(Base.ChildsLength);
            Base.BaseStream.WriteCompressed(Context, Link);
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