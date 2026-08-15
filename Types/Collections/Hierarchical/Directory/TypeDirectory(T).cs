using System.Collections;
using Zion.Serialization;

namespace Zion
{
    public class TypeDirectory<T, I> : IList<I>
    {
        #region Data
        public TypeDirectory<T, I>? Parent { get; private set; }
        private readonly List<TypeDirectory<T, I>> Childs;
        private readonly List<I> Values;
        public T Header;

        #endregion

        #region Properties
        public int ChildsCount => Childs.Count;
        public int ValuesCount => Values.Count;
        public int Count => Values.Count;

        public bool IsRoot => Parent is null;
        public bool IsReadOnly => false;

        #endregion

        #region Constructors
        public TypeDirectory()
        {
            Childs = new();
            Values = new();
        }

        public TypeDirectory(T Header)
            : this()
        {
            this.Header = Header;
        }

        public TypeDirectory(IEnumerable<TypeDirectory<T, I>> Childs, IEnumerable<I> Values)
        {
            this.Childs = Childs.WhereNotNull().ToList();
            this.Values = Values.ToList();
        }

        public TypeDirectory(T Header, IEnumerable<TypeDirectory<T, I>> Childs, IEnumerable<I> Values)
            : this(Childs, Values)
        {
            this.Header = Header;
        }

        #endregion

        #region Indexers
        public I this[int Index]
        {
            get => Values[Index];
            set => Values[Index] = value;
        }

        public I this[Index Index]
        {
            get => Values[Index];
            set => Values[Index] = value;
        }

        #endregion

        #region Values
        public void Add(I Item)
        {
            Values.Add(Item);
        }

        public void Insert(int Index, I Item)
        {
            Values.Insert(Index, Item);
        }


        public int IndexOf(I Item)
        {
            return Values.IndexOf(Item);
        }

        public bool Contains(I Item)
        {
            return Values.Contains(Item);
        }


        public void RemoveAt(int Index)
        {
            Values.RemoveAt(Index);
        }

        public bool Remove(I Item)
        {
            return Values.Remove(Item);
        }

        public void Clear()
        {
            Values.Clear();
        }


        public void CopyTo(I[] Array, int ArrayIndex)
        {
            Values.CopyTo(Array, ArrayIndex);
        }

        public void ForEach(Action<I> Action)
        {
            ArgumentNullException.ThrowIfNull(Action);
            foreach (I Item in Values)
            {
                Action(Item);
            }
        }


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<I> GetEnumerator()
        {
            return Values.GetEnumerator();
        }

        #endregion

        #region Childs
        public void Add(TypeDirectory<T, I> Item)
        {
            if (!Item.IsRoot)
            {
                throw new InvalidOperationException("The TypeDirectory is already located in another directory.");
            }
            Item.Parent = this;
            Childs.Add(Item.NotNull());
        }

        public int IndexOf(TypeDirectory<T, I> Item)
        {
            return Contains(Item) ? Childs.IndexOf(Item) : -1;
        }

        public bool Contains(TypeDirectory<T, I> Item)
        {
            return ReferenceEquals(Item.Parent, this);
        }

        public bool Remove(TypeDirectory<T, I> Item)
        {
            if (Contains(Item))
            {
                Childs.Remove(Item);
                Item.Parent = null;
                return true;
            }
            return false;
        }

        public void RemoveChilds()
        {
            ForEach(static Item => Item.Parent = null);
            Childs.Clear();
        }

        public void Destroy()
        {
            Parent?.Remove(this);
            Values.Clear();
            RemoveChilds();
        }


        public void ForEach(Action<TypeDirectory<T, I>> Action)
        {
            foreach (var Item in Childs)
            {
                Action(Item);
            }
        }

        public void ForEachRecursive(Action<TypeDirectory<T, I>> Action)
        {
            foreach (var Item in Childs)
            {
                Action(Item);
                Item.ForEachRecursive(Action);
            }
        }


        public IEnumerable<TypeDirectory<T, I>> EnumerateDirectories()
        {
            return Childs;
        }

        #endregion
    }
}