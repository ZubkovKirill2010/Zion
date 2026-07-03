namespace Zion.Serialization
{
    public static class BinaryWriterExtensions
    {
        extension(BinaryWriter Writer)
        {
            public void Write<T>(T Value) where T : IBinaryWritable
            {
                Value.Write(Writer);
            }

            public void Write<T>(T Value, IBinaryWriter<T>? ObjectWriter = null)
            {
                ObjectWriter ??= BinarySerializer.GetWriter<T>();
                BinarySerializer.WriterNotFound(ObjectWriter);
                ObjectWriter.Write(Writer, Value);
            }


            public void WriteCollection<T>(ICollection<T> Collection) where T : IBinaryWritable
            {
                ArgumentNullException.ThrowIfNull(Collection);

                Writer.Write(Collection.Count);
                foreach (T Item in Collection)
                {
                    Writer.Write(Item);
                }
            }

            public void WriteCollection<T>(ICollection<T> Collection, IBinaryWriter<T>? ObjectWriter = null)
            {
                ArgumentNullException.ThrowIfNull(Collection);

                if (Collection.Count == 0)
                {
                    Writer.Write(0);
                    return;
                }

                ObjectWriter ??= BinarySerializer.GetWriter<T>();

                if (ObjectWriter is not null)
                {
                    Writer.Write(Collection.Count);
                    foreach (T Item in Collection)
                    {
                        ObjectWriter.Write(Writer, Item);
                    }
                }
                else if (Collection.First() is IBinaryWritable)
                {
                    Writer.Write(Collection.Count);
                    foreach (IBinaryWritable Item in Collection.Cast<IBinaryWritable>())
                    {
                        Item.Write(Writer);
                    }
                }
                else
                {
                    BinarySerializer.WriterNotFound<T>();
                }
            }
        }
    }
}