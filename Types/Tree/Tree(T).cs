using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json.Serialization;

namespace Zion
{
    [Serializable]
    public class Tree<T> : IList<Tree<T>>, IBinaryGeneric<Tree<T>, T>, IColorText
    {
        private static InvalidOperationException IsParentException => new InvalidOperationException("Cannot add a parent structure to its child elements");

        [JsonPropertyName("Value")] public T Value { get; set; }
        [JsonPropertyName("Child")] private List<Tree<T>> Childs { get; set; }
        [JsonPropertyName("Parent")] public Tree<T>? Parent { get; private set; }

        public Tree<T> Root => Parent is null ? this : Parent.Root;

        public const string FileExtension = ".struct";

        public int Count => Childs.Count;

        public bool IsRoot => Parent is null;
        public bool IsEmpty => Childs.Count == 0;
        public bool IsReadOnly => ((ICollection<Tree<T>>)Childs).IsReadOnly;


        public Tree(T Value)
        {
            this.Value = Value;
            Childs = new List<Tree<T>>();
            Parent = null;
        }
        public Tree(T Value, Tree<T> Parent)
        {
            this.Value = Value;
            this.Parent = Parent;
            Childs = new List<Tree<T>>();
        }
        public Tree(T Value, IList<T> Childs)
        {
            this.Value = Value;
            Parent = null;
            SetChilds(Childs);
        }
        public Tree(T Value, IList<Tree<T>> Childs)
        {
            this.Value = Value;
            Parent = null;
            SetChilds(Childs);
        }
        public Tree(T Value, IList<Tree<T>> Childs, Tree<T>? Parent)
        {
            this.Value = Value;
            this.Parent = Parent;
            SetChilds(Childs);
        }

        Tree<T> IList<Tree<T>>.this[int Index]
        {
            get => Childs[Index];
            set
            {
                value.Parent = this;
                Childs[Index] = value;
            }
        }


        public override bool Equals(object? Object)
        {
            return Object is Tree<T> Tree && ReferenceEquals(this, Tree);
        }

        public override string ToString()
        {
            StringBuilder Result = new StringBuilder();
            BuildTreeString(Result, string.Empty, true);
            return Result.ToString().TrimEnd();
        }

        public string ToColorString()
        {
            return ToColorString(new RGBColor(67, 211, 224));
        }

        public string ToColorString(RGBColor LineColor)
        {
            StringBuilder Result = new StringBuilder();
            BuildTreeColorString(Result, string.Empty, true, LineColor);
            return Result.ToString().TrimEnd();
        }

        private void BuildTreeString(StringBuilder Builder, string Prefix, bool IsLast)
        {
            Builder.Append(Prefix);

            if (!IsRoot)
            {
                Builder.Append(IsLast ? "└──" : "├──");
            }

            Builder.AppendLine(Value?.ToString());

            if (!IsRoot)
            {
                Prefix += IsLast ? "   " : "│  ";
            }

            for (int i = 0; i < Childs.Count; i++)
            {
                bool LastChild = i == Childs.Count - 1;
                Childs[i].BuildTreeString(Builder, Prefix, LastChild);
            }
        }
        private void BuildTreeColorString(StringBuilder Builder, string Prefix, bool IsLast, RGBColor LineColor)
        {
            Builder.Append(Prefix);

            if (!IsRoot)
            {
                Builder.Append(IsLast ? new ColorText("└──", LineColor).ToString() : new ColorText("├──", LineColor));
            }

            Builder.AppendLine(Value?.ToString());

            if (!IsRoot)
            {
                Prefix += IsLast ? "   " : new ColorText("│  ", LineColor);
            }

            for (int i = 0; i < Childs.Count; i++)
            {
                bool LastChild = i == Childs.Count - 1;
                Childs[i].BuildTreeColorString(Builder, Prefix, LastChild, LineColor);
            }
        }


        [MemberNotNull(nameof(Childs))]
        public void SetChilds(IList<Tree<T>> Childs)
        {
            ArgumentNullException.ThrowIfNull(Childs);

            List<Tree<T>> NewChilds = new List<Tree<T>>(Childs.Count);

            foreach (Tree<T> Child in Childs.Where(Child => Child is not null))
            {
                if (IsAncestor(Child))
                {
                    throw IsParentException;
                }
                Child.Parent = this;
                NewChilds.Add(Child);
            }
            this.Childs = NewChilds;
        }
        [MemberNotNull(nameof(Childs))]
        public void SetChilds(IList<T> Childs)
        {
            SetChilds(Childs, Item => Item);
        }
        [MemberNotNull(nameof(Childs))]
        public void SetChilds<I>(IList<I> Childs, Converter<I, T> Converter)
        {
            List<Tree<T>>? NewChilds = new List<Tree<T>>(Childs.Count);

            foreach (I Child in Childs.Where(Child => Child is not null))
            {
                NewChilds.Add(new Tree<T>(Converter(Child), this));
            }

            this.Childs = NewChilds;
        }

        public void ForEach(Converter<T, T> Action)
        {
            for (int i = 0; i < Count; i++)
            {
                Childs[i].Value = Action(Childs[i].Value);
            }
        }

        public void InvokeForAll(Action<T> Action)
        {
            Action(Value);
            foreach (Tree<T> Child in Childs)
            {
                Child.InvokeForAll(Action);
            }
        }
        public void InvokeForAll(Action<int, T> Action)
        {
            InvokeForAll(Action, 0);
        }
        private void InvokeForAll(Action<int, T> Action, int Level)
        {
            Action(Level, Value);
            foreach (Tree<T> Child in Childs)
            {
                Child.InvokeForAll(Action, Level + 1);
            }
        }

        public Tree<T> AddIf(T Item, bool Condition)
        {
            if (Condition)
            {
                Add(Item);
            }
            return this;
        }
        public Tree<T> AddIf(Tree<T> Item, bool Condition)
        {
            if (Condition)
            {
                Add(Item);
            }
            return this;
        }

        public Tree<T> AddIf(T Item, Predicate<T> Condition)
        {
            if (Condition(Item))
            {
                Add(Item);
            }
            return this;
        }
        public Tree<T> AddIf(Tree<T> Item, Predicate<Tree<T>> Condition)
        {
            if (Condition(Item))
            {
                Add(Item);
            }
            return this;
        }

        public Tree<T> AddWhere(Predicate<T> Condition, params T[] Items)
        {
            foreach (T Item in Items.Where(Condition))
            {
                Add(Item);
            }
            return this;
        }
        public Tree<T> AddWhere(Predicate<Tree<T>> Condition, params Tree<T>[] Items)
        {
            foreach (Tree<T> Item in Items.Where(Condition))
            {
                Add(Item);
            }
            return this;
        }

        public Tree<T>? GetParent(int Level)
        {
            if (Level < 0) { return null; }
            if (Level == 0) { return this; }

            Tree<T>? Current = this;

            for (int i = 0; i < Level; i++)
            {
                Current = Current.Parent;

                if (Current is null)
                {
                    return null;
                }
            }

            return Current;
        }
        public bool TryGetParent(int Level, out Tree<T>? Parent)
        {
            if (Level < 0)
            {
                Parent = null;
                return false;
            }

            Parent = this;

            if (Level == 0)
            {
                return true;
            }

            for (int i = 0; i < Level; i++)
            {
                Parent = Parent.Parent;

                if (Parent is null)
                {
                    return false;
                }
            }

            return true;
        }

        public void Add(T Item)
        {
            ArgumentNullException.ThrowIfNull(Item);
            Add(new Tree<T>(Item, this));
        }
        public void Add(Tree<T> Item)
        {
            ArgumentNullException.ThrowIfNull(Item);
            if (IsAncestor(Item))
            {
                throw IsParentException;
            }
            Item.Parent = this;
            Childs.Add(Item);
        }
        public void Insert(int Index, Tree<T> Item)
        {
            ArgumentNullException.ThrowIfNull(Item);
            if (IsAncestor(Item))
            {
                throw IsParentException;
            }
            Item.Parent = this;
            Childs.Insert(Index, Item);
        }

        public int IndexOf(Tree<T> Item)
        {
            ArgumentNullException.ThrowIfNull(Item);
            return Childs.IndexOf(Item);
        }
        public bool Contains(Tree<T> Item)
        {
            ArgumentNullException.ThrowIfNull(Item);
            return Childs.Contains(Item);
        }
        public void CopyTo(Tree<T>[] Array, int ArrayIndex)
        {
            ArgumentNullException.ThrowIfNull(Array);
            Childs.CopyTo(Array, ArrayIndex);
        }

        public void Clear()
        {
            foreach (Tree<T> Child in Childs)
            {
                Child.Parent = null;
            }
            Childs.Clear();
        }
        public bool Remove(Tree<T> Item)
        {
            Item.Parent = null;
            return Childs.Remove(Item);
        }
        public void RemoveAt(int Index)
        {
            Childs[Index].Parent = null;
            Childs.RemoveAt(Index);
        }


        public IEnumerator<Tree<T>> GetEnumerator()
        {
            return Childs.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        public void Write(BinaryWriter Writer, Action<T> Write)
        {
            Write(Value);
            Writer.Write(Count);
            foreach (Tree<T> Child in Childs)
            {
                Child.Write(Writer, Write);
            }
        }

        public static Tree<T> Read(BinaryReader Reader, Func<T> Read)
        {
            return Read<T>(Reader, Read, null);
        }
        public static Tree<I> Read<I>(BinaryReader Reader, Func<I> Read, Tree<I>? Parent)
        {
            int Count = 0;
            Tree<I> Result = new Tree<I>
            (
                Read(),
                new List<Tree<I>>(Accessor.Set(out Count, Reader.ReadInt32())),
                Parent
            );

            for (int i = 0; i < Count; i++)
            {
                Result.Add(Read<I>(Reader, Read, Result));
            }

            return Result;
        }


        public bool IsAncestor(Tree<T> Tree)
        {
            ArgumentNullException.ThrowIfNull(Tree);

            Tree<T>? Current = this;

            while (Current is not null)
            {
                if (ReferenceEquals(Current, Tree))
                {
                    return true;
                }
                Current = Current.Parent;
            }

            return false;
        }
    }
}