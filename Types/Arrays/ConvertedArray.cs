using System.Collections;

namespace Zion
{
    public sealed class ConvertedArray<TIn, TOut> : IList<TOut>
    {
        private readonly Converter<TIn, TOut> Converter;
        private readonly IList<TIn> List;
        private readonly TOut?[] Results;

        private readonly int CountField;

        public ConvertedArray(IList<TIn> List, Converter<TIn, TOut> Converter)
        {
            this.List = List;
            this.Converter = Converter;
            CountField = List.Count;
            Results = new TOut[List.Count];
        }

        public TOut this[int Index]
        {
            get
            {
                TOut? Value = Results[Index];
                if (Value is null)
                {
                    Value = Converter(List[Index]);
                    Results[Index] = Value;
                }
                return Value;
            }
            set => throw new NotSupportedException("ConverterArray is read only");
        }

        public TIn GetOriginal(int Index) => List[Index];

        public bool IsReadOnly => true;
        public int Count => CountField;

        public void Add(TOut item)
        {
            throw new NotSupportedException("ConverterArray is read only");
        }
        public void Clear()
        {
            throw new NotSupportedException("ConverterArray is read only");
        }
        public bool Contains(TOut item)
        {
            throw new NotSupportedException("ConverterArray is read only");
        }
        public void CopyTo(TOut[] array, int arrayIndex)
        {
            throw new NotSupportedException("ConverterArray is read only");
        }

        public int IndexOf(TOut item)
        {
            throw new NotSupportedException("ConverterArray is read only");
        }
        public void Insert(int index, TOut item)
        {
            throw new NotSupportedException("ConverterArray is read only");
        }
        public bool Remove(TOut item)
        {
            throw new NotSupportedException("ConverterArray is read only");
        }
        public void RemoveAt(int index)
        {
            throw new NotSupportedException("ConverterArray is read only");
        }

        public IEnumerator<TOut> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}