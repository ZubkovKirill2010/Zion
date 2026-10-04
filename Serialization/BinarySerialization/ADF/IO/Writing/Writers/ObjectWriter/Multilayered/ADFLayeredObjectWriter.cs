namespace Zion.Serialization.ADF
{
    //TODO: Realize ADFLayeredObjectWriter
    public sealed class ADFLayeredObjectWriter<T> : ADFObjectWriter
    {
        private readonly IADFSerializer<T>[] Serializers;

        internal ADFLayeredObjectWriter(ADFWritingContext Context, StreamGroup Target, IADFSerializer<T>[] Serializers)
            : base(Context, Target)
        {
            this.Serializers = Serializers; 
        }


        internal void Serialize(T Value)
        {

        }


        protected override StreamGroup GetStreamGroup(string Name, in uint NameId, in uint FormatId)
        {
            return base.GetStreamGroup(Name, NameId, FormatId);
        }

        protected override void OnWrited(string Name, in uint NameId, in uint FormatId)
        {
            base.OnWrited(Name, NameId, FormatId);
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();
        }
    }
}