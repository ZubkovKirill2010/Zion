using System.Runtime.InteropServices;

namespace Zion
{
    public sealed class ArenaStack<T> : ArenaCollection<T>, ICollection<T>
    {
        public bool IsReadOnly => false;

        public int Count
        {
            get;
            private set
            {
                ArgumentOutOfRangeException.ThrowIfNegative(value);
                Expand(value);
                field = value;
            }
        }


        public ArenaStack(ArenaSpan<T> Data) : base(Data) { }


        public void Push(T Item)
        {
            var Data = this.Data;
            Data[Count++] = Item;
        }

        public T Pop()
        {
            return Data[--Count];
        }

        public T Peek()
        {
            return Data[Count - 1];
        }


        public bool TryPop(out T Item)
        {
            if (Count > 0)
            {
                Item = Pop();
                return true;
            }
            Item = default!;
            return false;
        }

        public bool TryPeek(out T Item)
        {
            if (Count > 0)
            {
                Item = Peek();
                return true;
            }
            Item = default!;
            return false;
        }


        public void Add(T Item)
        {
            Push(Item);
        }

        public bool Contains(T Item)
        {
            return Data.Use
            (
                Span =>
                {
                    var Comparer = EqualityComparer<T>.Default;

                    for (int i = Span.Length - 1; i >= 0; i--)
                    {
                        if (Comparer.Equals(Span[i], Item))
                        {
                            return true;
                        }
                    }

                    return false;
                }
            );
        }

        public void CopyTo(T[] Array, int ArrayIndex)
        {
            Data.Use
            (
                Count,
                Span =>
                {
                    var Destination = Array.AsSpan(ArrayIndex);
                    Span.CopyTo(Destination);
                }
            );
        }

        public bool Remove(T Item)
        {
            return Data.Use
            (
                Count,
                Span =>
                {
                    var Count = this.Count;
                    var Comparer = EqualityComparer<T>.Default;

                    for (int i = 0; i < Count; i++)
                    {
                        if (Comparer.Equals(Span[i], Item))
                        {
                            Data.Move(i + 1, i, Count - i - 1);
                            this.Count--;
                            return true;
                        }
                    }

                    return false;
                }
            );            
        }

        public void Clear()
        {
            Count = 0;
        }


        public T[] ToArray()
        {
            T[] Result = new T[Count];
            CopyTo(Result, 0);
            return Result;
        }

        public Stack<T> ToStack()
        {
            return Data.Use
            (
                Count,
                Span =>
                {
                    var Stack = new Stack<T>(Span.Length);

                    for (int i = Span.Length - 1; i >= 0; i++)
                    {
                        Stack.Push(Span[i]);
                    }

                    return Stack;
                }
            );            
        }

        public List<T> ToList()
        {
            var Count = this.Count;
            if (Count == 0)
            {
                return new List<T>();
            }

            return Data.Use
            (
                Count,
                Span =>
                {
                    var Result = new List<T>(Count);

                    CollectionsMarshal.SetCount(Result, Count);
                    Span.CopyTo(CollectionsMarshal.AsSpan(Result));

                    return Result;
                }
            );            
        }


        public override IEnumerator<T> GetEnumerator()
        {
            //TODO
            throw new NotImplementedException();
        }
    }
}