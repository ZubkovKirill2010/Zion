namespace Zion
{
    public sealed partial class ObjectReader //_Collections
    {
        public bool TryReadList<T>(SafeGetter<T> Reader, out List<T> Value)
        {
            Value = null!;

            if (IsEnd) { return false; }

            int StartIndex = Index;
            char Current = Text[StartIndex];

            if (Current != '[') { return false; }

            Index++;
            SkipSpaces();

            if (IsAt(Index, ']'))
            {
                Value = new List<T>(0);
                return true;
            }

            if (Reader(out T FirstItem))
            {
                Value = new List<T>()
                {
                    FirstItem
                };
            }
            else
            {
                Index = StartIndex;
                return false;
            }

            while (Index < Length)
            {
                SkipSpaces();

                if (Text[Index] == ']')
                {
                    Index++;
                    break;
                }

                if (Text[Index] == ',')
                {
                    Index++;
                    SkipSpaces();

                    if (Reader(out T Item))
                    {
                        Value.Add(Item);
                    }
                    else
                    {
                        Index = StartIndex;
                        return false;
                    }
                }
                else
                {
                    Index = StartIndex;
                    return false;
                }
            }

            return true;
        }

        public bool TryReadArray<T>(SafeGetter<T> Reader, out T[] Value)
        {
            if (TryReadList(Reader, out List<T> List))
            {
                Value = List.ToArray();
                return true;
            }

            Value = null!;
            return false;
        }

        public List<T> ReadList<T>(SafeGetter<T> Reader)
        {
            return Unsafe((out List<T> Value) => TryReadList(Reader, out Value));
        }

        public T[] ReadArray<T>(SafeGetter<T> Reader)
        {
            return Unsafe((out T[] Value) => TryReadArray(Reader, out Value));
        }
    }
}