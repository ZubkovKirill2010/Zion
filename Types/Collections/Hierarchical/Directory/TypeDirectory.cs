using System.Collections;
using Zion.Serialization;

namespace Zion
{
    public class TypeDirectory<T> : IList<T>, IBinarySerializable<TypeDirectory<T>, T>
    {
        #region Data
        public TypeDirectory<T>? Parent { get; private set; }
        private readonly List<TypeDirectory<T>> Childs;
        private readonly List<T> Values;

        #endregion

        #region Properties
        public int ChildsCount => Childs.Count;
        public int ValuesCount => Values.Count;
        public int Count      => Values.Count;

        public bool IsRoot => Parent is null;
        public bool IsReadOnly => false;
            
        #endregion

        #region Constructors
        public TypeDirectory()
        {
            Childs = new();
            Values = new();
        }

        private TypeDirectory(List<TypeDirectory<T>> Childs, List<T> Values)
        {
            this.Childs = Childs.NotNull();
            this.Values = Values.NotNull();
        }

        #endregion

        #region Indexers
        public T this[int Index]
        {
            get => Values[Index];
            set => Values[Index] = value;
        }

        public T this[Index Index]
        {
            get => Values[Index];
            set => Values[Index] = value;
        }

        #endregion

        #region Values
        public void Add(T Item)
        {
            Values.Add(Item);
        }

        public void Insert(int Index, T Item)
        {
            Values.Insert(Index, Item);
        }


        public int IndexOf(T Item)
        {
            return Values.IndexOf(Item);
        }

        public bool Contains(T Item)
        {
            return Values.Contains(Item);
        }


        public void RemoveAt(int Index)
        {
            Values.RemoveAt(Index);
        }

        public bool Remove(T Item)
        {
            return Values.Remove(Item);
        }
        
        public void Clear()
        {
            Values.Clear();
        }


        public void CopyTo(T[] Array, int ArrayIndex)
        {
            Values.CopyTo(Array, ArrayIndex);
        }

        public void ForEach(Action<T> Action)
        {
            ArgumentNullException.ThrowIfNull(Action);
            foreach (T Item in Values)
            {
                Action(Item);
            }
        }


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<T> GetEnumerator()
        {
            return Values.GetEnumerator();
        }

        #endregion

        #region Childs
        public void Add(TypeDirectory<T> Item)
        {
            if (!Item.IsRoot)
            {
                throw new InvalidOperationException("The TypeDirectory is already located in another directory.");
            }
            Item.Parent = this;
            Childs.Add(Item.NotNull());
        }

        public int IndexOf(TypeDirectory<T> Item)
        {
            return Contains(Item) ? Childs.IndexOf(Item) : -1;
        }

        public bool Contains(TypeDirectory<T> Item)
        {
            return ReferenceEquals(Item.Parent, this);
        }

        public bool Remove(TypeDirectory<T> Item)
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

        
        public void ForEach(Action<TypeDirectory<T>> Action)
        {
            foreach (var Item in Childs)
            {
                Action(Item);
            }
        }

        public void ForEachRecursive(Action<TypeDirectory<T>> Action)
        {
            foreach (var Item in Childs)
            {
                Action(Item);
                Item.ForEachRecursive(Action);
            }
        }

        
        public IEnumerable<TypeDirectory<T>> EnumerateDirectories()
        {
            return Childs;
        }

        #endregion

        #region IBinarySerializable
        public void Write(BinaryWriter Writer, Action<T> Write)
        {
            Writer.WriteCollection(Values, Write);
            Writer.Write(ChildsCount);
            foreach (var Item in Childs)
            {
                Item.Write(Writer, Write);
            }
        }

        public static TypeDirectory<T> Read(BinaryReader Reader, Func<T> Read)
        {
            var Values = Reader.ReadList(Read);
            var ChildsCount = Reader.ReadInt32();
            var Childs = new List<TypeDirectory<T>>(ChildsCount);
            var Result = new TypeDirectory<T>(Childs, Values);

            for (int i = 0; i < ChildsCount; i++)
            {
                TypeDirectory<T> Child = TypeDirectory<T>.Read(Reader, Read);
                Child.Parent = Result;
                Childs.Add(Child);
            }

            return Result;
        }

        #endregion
    }
}