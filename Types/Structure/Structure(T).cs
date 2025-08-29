using System.Collections;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Zion
{
    [Serializable]
    public class Structure<T> : IList<Structure<T>>, IBinaryGeneric<Structure<T>, T>
    {
        private static InvalidOperationException IsParentException => new InvalidOperationException("Cannot add a parent structure to its child elements");

        [JsonPropertyName("Value")] public T Value { get; set; }
        [JsonPropertyName("Child")] private List<Structure<T>> Childs { get; set; }
        [JsonPropertyName("Parent")] public Structure<T>? Parent { get; private set; }

        public Structure<T> Root => IsRoot ? this : Parent.Root;


        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true
        };

        public const string FileExtension = ".struct";

        public int Count => Childs.Count;
        public bool IsRoot => Parent is null;
        public bool IsReadOnly => ((ICollection<Structure<T>>)Childs).IsReadOnly;


        public Structure(T Value)
        {
            this.Value = Value;
            Childs = new List<Structure<T>>();
            Parent = null;
        }
        public Structure(T Value, Structure<T> Parent)
        {
            this.Value = Value;
            this.Parent = Parent;
            Childs = new List<Structure<T>>();
        }
        public Structure(T Value, IList<T> Childs)
        {
            this.Value = Value;
            Parent = null;
            SetChilds(Childs);
        }
        public Structure(T Value, IList<Structure<T>> Childs)
        {
            this.Value = Value;
            Parent = null;
            SetChilds(Childs);
        }
        public Structure(T Value, IList<Structure<T>> Childs, Structure<T>? Parent)
        {
            this.Value = Value;
            this.Parent = Parent;
            SetChilds(Childs);
        }

        Structure<T> IList<Structure<T>>.this[int Index]
        {
            get => Childs[Index];
            set
            {
                value.Parent = this;
                Childs[Index] = value;
            }
        }


        public override string ToString()
        {
            StringBuilder Result = new StringBuilder();
            BuildTreeString(Result, string.Empty, true);
            return Result.ToString().TrimEnd();
        }

        public string ToColorString(Color LineColor)
        {
            StringBuilder Result = new StringBuilder();
            BuildTreeColorString(Result, string.Empty, true, LineColor);
            return Result.ToString().TrimEnd();
        }


        public void Write(BinaryWriter Writer, Action<BinaryWriter, T> Write)
        {
            Write(Writer, Value);
            Writer.Write(Count);
            foreach (Structure<T> Child in Childs)
            {
                Child.Write(Writer, Write);
            }
        }

        public static Structure<T> Read(BinaryReader Reader, Func<BinaryReader, T> Read)
        {
            return Read<T>(Reader, Read, null);
        }
        public static Structure<I> Read<I>(BinaryReader Reader, Func<BinaryReader, I> Read, Structure<I>? Parent)
        {
            int Count = 0;
            Structure<I> Result = new Structure<I>
            (
                Read(Reader),
                new List<Structure<I>>(Accessor.Set(out Count, Reader.ReadInt32())),
                Parent
            );

            for (int i = 0; i < Count; i++)
            {
                Result.Add(Read<I>(Reader, Read, Result));
            }

            return Result;
        }

        public void SaveIn(string FilePath, Action<BinaryWriter, T> Write)
        {
            using FileStream Stream = new FileStream(FilePath, FileMode.Create);
            using BinaryWriter Writer = new BinaryWriter(Stream);
            this.Write(Writer, Write);
        }
        public static Structure<I> Read<I>(string FilePath, Func<BinaryReader, I> Read)
        {
            if (!System.IO.File.Exists(FilePath))
            {
                throw new FileNotFoundException($"File \"{FilePath}\" not exists");
            }

            using FileStream Stream = new FileStream(FilePath, FileMode.Open);
            using BinaryReader Reader = new BinaryReader(Stream);
            return Read<I>(Reader, Read, null);
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
        private void BuildTreeColorString(StringBuilder Builder, string Prefix, bool IsLast, Color LineColor)
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


        public void SetChilds(IList<Structure<T>> Childs)
        {
            ArgumentNullException.ThrowIfNull(Childs);

            List<Structure<T>> NewChilds = new List<Structure<T>>(Childs.Count);

            foreach (Structure<T> Child in Childs.Where(Child => Child is not null))
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
        public void SetChilds(IList<T> Childs)
        {
            SetChilds(Childs, Item => Item);
        }
        public void SetChilds<I>(IList<I> Childs, Converter<I, T> Converter)
        {
            List<Structure<T>>? NewChilds = new List<Structure<T>>(Childs.Count);

            foreach (I Child in Childs.Where(Child => Child is not null))
            {
                NewChilds.Add(new Structure<T>(Converter(Child), this));
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
            foreach (Structure<T> Child in Childs)
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
            foreach (Structure<T> Child in Childs)
            {
                Child.InvokeForAll(Action, Level + 1);
            }
        }

        public void Add(T Item)
        {
            ArgumentNullException.ThrowIfNull(Item);
            Add(new Structure<T>(Item, this));
        }
        public void Add(Structure<T> Item)
        {
            ArgumentNullException.ThrowIfNull(Item);
            if (IsAncestor(Item))
            {
                throw IsParentException;
            }
            Item.Parent = this;
            Childs.Add(Item);
        }
        public void Insert(int Index, Structure<T> Item)
        {
            ArgumentNullException.ThrowIfNull(Item);
            if (IsAncestor(Item))
            {
                throw IsParentException;
            }
            Item.Parent = this;
            Childs.Insert(Index, Item);
        }

        public int IndexOf(Structure<T> Item)
        {
            ArgumentNullException.ThrowIfNull(Item);
            return Childs.IndexOf(Item);
        }
        public bool Contains(Structure<T> Item)
        {
            ArgumentNullException.ThrowIfNull(Item);
            return Childs.Contains(Item);
        }
        public void CopyTo(Structure<T>[] Array, int ArrayIndex)
        {
            ArgumentNullException.ThrowIfNull(Array);
            Childs.CopyTo(Array, ArrayIndex);
        }

        public void Clear()
        {
            foreach (Structure<T> Child in Childs)
            {
                Child.Parent = null;
            }
            Childs.Clear();
        }
        public bool Remove(Structure<T> Item)
        {
            Item.Parent = null;
            return Childs.Remove(Item);
        }
        public void RemoveAt(int Index)
        {
            Childs[Index].Parent = null;
            Childs.RemoveAt(Index);
        }


        public IEnumerator<Structure<T>> GetEnumerator()
        {
            return Childs.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        public bool IsAncestor(Structure<T> Structure)
        {
            ArgumentNullException.ThrowIfNull(Structure);

            Structure<T>? Current = this;

            while (Current is not null)
            {
                if (ReferenceEquals(Current, Structure))
                {
                    return true;
                }
                Current = Current.Parent;
            }

            return false;
        }
    }
}