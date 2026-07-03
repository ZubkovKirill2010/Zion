namespace Zion.Serialization.NSD
{
    public sealed class NSDBufferedWriteContext : NSDWriteContext
    {
        public NSDBufferedWriteContext(Stream Stream) : base(Stream) { }


        protected override void WritePrimitiveSafe<T>(T Value)
        {
            //TODO
        }

        protected override void WriteSafe<T>(T Value)
        {
            //TODO
        }

        protected override void WriteSafe<T>(T Value, IBinaryWriter<T>? ObjectWriter = null)
        {
            //TODO
        }
    }
}