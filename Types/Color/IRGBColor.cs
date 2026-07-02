using Zion.Serialization;

namespace Zion
{
    public interface IRGBColor : IBinaryWritable
    {
        byte R { get; set; }
        byte G { get; set; }
        byte B { get; set; }
    }
}