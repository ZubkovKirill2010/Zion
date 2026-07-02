namespace Zion.Serialization.NSD
{
    public interface INSDPrimitive<T> where T : INSDPrimitive<T>
    {
        public void Write(BinaryWriter Writer);

        public static abstract T Read(BinaryReader Reader);
    }
}