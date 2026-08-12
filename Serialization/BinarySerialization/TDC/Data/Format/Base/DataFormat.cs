namespace Zion.Serialization.TDC
{
    internal static class DataFormat
    {
        private static readonly IBinarySerializer<IFormat> _Serializer = new BinarySerializer<IFormat>
        (
            static (Writer, Value) =>
            {
                Writer.Write(Value.FormatId);
                Writer.Write(Value);
            },
            static Reader =>
            {
                byte FormatId = Reader.ReadByte();
                return FormatId switch
                {
                    0 => Reader.Read<PrimitiveFormat>(),
                    1 => Reader.Read<ContainerFormat>(),
                    2 => Reader.Read<EnumFormat>(),
                    _ => throw new NotSupportedException($"Format with FormatId = {FormatId} not exists")
                };
            }
        );

        extension(IFormat Format)
        {
            public static IBinarySerializer<IFormat> Serializer => _Serializer;
        }
    }
}