namespace Zion.Serialization.ADF
{
    internal abstract class ClassWriteStrategy<T> : ProvidedWriteStrategy<T>
    {
        public ClassWriteStrategy(ADFWritingContext Context)
            : base(Context) { }


        protected sealed override ArenaStream GetStreamForData(StreamGroup BaseGroup)
        {
            var NewGroup = new StreamGroup(Context.Arena.GetStream(1));
            BaseGroup.Add(NewGroup);
            return NewGroup.BaseStream;
        }


        public override void Write(StreamGroup Group, T Value)
        {
            if (Context.Registries.References.TryGetReference(Value, out Reference Reference))
            {
                Group.BaseStream.Write(Reference.Id);
            }
            else
            {
                base.Write(Group, Value);
            }
        }
    }
}