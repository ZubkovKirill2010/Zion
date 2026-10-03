namespace Zion.Serialization.ADF
{
    internal abstract class ClassWriteStrategy<T> : ProvidedWriteStrategy<T>
    {
        public ClassWriteStrategy(uint FormatId) : base(FormatId) { }


        protected sealed override StreamGroup GetGroupForData(ADFWritingContext Context, StreamGroup BaseGroup)
        {
            var NewGroup = new StreamGroup(Context.Arena.GetStream(1));
            BaseGroup.Add(NewGroup);
            return NewGroup;
        }

        protected sealed override bool TryWriteReference(ADFWritingContext Context, ArenaStream BaseStream, T Value)
        {
            if (Context.Registries.References.TryGetReference(Value, out Reference Reference))
            {
                BaseStream.WriteCompressed(Context, Reference.Id);
                return true;
            }
            return false;
        }

        protected sealed override void Write(ADFWritingContext Context, StreamGroup Base, ADFObjectWriter Writer, T Value)
        {
            var Link = Reference.CreateNewReference(Base.ChildsLength);
            Base.BaseStream.WriteCompressed(Context, Link);
            WriteValue(Writer, Value);
        }

        protected sealed override void OnWrited(ADFWritingContext Context, uint FormatId, T Value)
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