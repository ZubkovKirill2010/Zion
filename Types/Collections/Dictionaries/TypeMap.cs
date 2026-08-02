using System.Collections;
using System.Data.Common;

namespace Zion
{
    public class TypeMap : IEnumerable
    {
        private readonly Dictionary<Type, object> Data;


        public int Count => Data.Count;

        public int Capacity => Data.Capacity;


        public TypeMap() : this(16) { }

        public TypeMap(int Capacity)
        {
            Data = new(Capacity);
        }

        public TypeMap(ICollection Collection)
        {
            Data = new(Collection.Count + 10);
            foreach (object Item in Collection.NotNull())
            {
                Add(Item);
            }
        }


        public T Get<T>()
        {
            return (T)Data[typeof(T)];
        }

        public bool TryGet<T>(out T Value)
        {
            if (Data.TryGetValue(typeof(T), out object? Object))
            {
                Value = (T)Object;
                return true;
            }
            Value = default!;
            return false;
        }

        public void Set<T>(T Value)
        {
            Data[Value.NotNull().GetType()] = Value;
        }


        public T GetOrAdd<T>(Func<T> Factory)
        {
            if (TryGet(out T value))
            {
                return value;
            }
            value = Factory();
            Add(value);
            return value;
        }


        public void Add<T>(T Value)
        {
            ArgumentNullException.ThrowIfNull(Value);
            Data.Add(Value.GetType(), Value);
        }

        public bool TryAdd<T>(T Value)
        {
            ArgumentNullException.ThrowIfNull(Value);
            return Data.TryAdd(Value.GetType(), Value);
        }


        public void Add(object Value)
        {
            ArgumentNullException.ThrowIfNull(Value);
            Data.Add(Value.GetType(), Value);
        }

        public bool TryAdd(object Value)
        {
            ArgumentNullException.ThrowIfNull(Value);
            return Data.TryAdd(Value.GetType(), Value);
        }


        public bool Contains<T>()
        {
            return Data.ContainsKey(typeof(T));
        }

        public bool Contains(Type Type)
        {
            return Data.ContainsKey(Type);
        }

        public void EnsureCapacity(int Capacity)
        {
            Data.EnsureCapacity(Capacity);
        }


        public bool Remove<T>()
        {
            return Data.Remove(typeof(T));
        }

        public bool Remove(Type Type)
        {
            return Data.Remove(Type);
        }


        public void Clear()
        {
            Data.Clear();
        }


        public IEnumerable<T> Enumerate<T>()
        {
            return Data.Values.OfType<T>();
        }

        public IEnumerator GetEnumerator()
        {
            return Data.Values.GetEnumerator();
        }
    }
}