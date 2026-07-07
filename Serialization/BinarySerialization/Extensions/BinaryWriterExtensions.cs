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


            public void Write<T, I>(T Value) where T : IBinaryWritable<I> where I : IBinaryWritable
            {
                Value.Write(Writer, Item => Item.Write(Writer));
            }

            public void Write<T, I>(T Value, Action<BinaryWriter, I> ObjectWriter) where T : IBinaryWritable<I>
            {
                ArgumentNullException.ThrowIfNull(ObjectWriter);
                Value.Write(Writer, Item => ObjectWriter(Writer, Item));
            }

            public void Write<T, I>(T Value, IBinaryWriter<I>? ObjectWriter = null) where T : IBinaryWritable<I>
            {
                ObjectWriter ??= BinarySerializer.GetWriter<I>();
                BinarySerializer.WriterNotFound(ObjectWriter);
                Value.Write(Writer, Item => ObjectWriter.Write(Writer, Item));
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

            public void WriteCollection<T>(ICollection<T> Collection, Action<T> ObjectWriter)
            {
                ArgumentNullException.ThrowIfNull(Collection);
                ArgumentNullException.ThrowIfNull(ObjectWriter);

                Writer.Write(Collection);
                foreach (T Item in Collection)
                {
                    ObjectWriter(Item);
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