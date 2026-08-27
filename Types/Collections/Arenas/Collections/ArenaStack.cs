using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            var Span = Data.AsSpan();
            var Comparer = EqualityComparer<T>.Default;

            for (int i = Count - 1; i >= 0; i--)
            {
                if (Comparer.Equals(Span[i], Item))
                {
                    return true;
                }
            }

            return false;
        }

        public void CopyTo(T[] Array, int ArrayIndex)
        {
            Data.AsSpan(0, Count).CopyTo(Array.AsSpan(ArrayIndex));
        }

        public bool Remove(T Item)
        {
            var Count = this.Count;
            var Span = Data.AsSpan(0, Count);
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

        public void Clear()
        {
            Count = 0;
        }


        public T[] ToArray()
        {
            T[] Result = new T[this.Count];
            CopyTo(Result, 0);
            return Result;
        }

        public Stack<T> ToStack()
        {
            var Count = this.Count;
            var Span  = Data.AsSpan(0, Count);
            var Stack = new Stack<T>(Count);

            for (int i = Count - 1; i >= 0; i++)
            {
                Stack.Push(Span[i]);
            }

            return Stack;
        }

        public List<T> ToList()
        {
            var Count = this.Count;
            if (Count == 0)
            {
                return new List<T>();
            }

            var Span = Data.AsSpan(0, Count);
            var List = new List<T>(Count);

            CollectionsMarshal.SetCount(List, Count);

            var BackingSpan = CollectionsMarshal.AsSpan(List);
            Span.CopyTo(BackingSpan);

            return List;
        }


        public override IEnumerator<T> GetEnumerator()
        {
            var Span = Data.AsSpan(0, Count);
            for (int i = Count - 1; i >= 0; i++)
            {
                yield return Span[i];
            }
        }
    }
}