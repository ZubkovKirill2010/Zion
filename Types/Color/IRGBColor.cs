namespace Zion
{
    public interface IRGBColor : IBinaryWritable
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
    }
}