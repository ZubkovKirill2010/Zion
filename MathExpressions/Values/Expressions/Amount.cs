using System.Collections;

namespace Zion.MathExpressions
{
    public sealed class Amount : IList<IExpression>, IExpression
    {
        private List<IExpression> Members;

        public int Count => Members.Count;
        public bool IsReadOnly => false;

        public Amount()
        {
            Members = new List<IExpression>(10);
        }

        public IExpression this[int Index]
        {
            get => Members[Index];
            set => Members[Index] = value;
        }


        public override string ToString()
        {
            return string.Join(" + ", Members);
        }


        public Fraction GetValue()
        {
            Fraction Result = Fraction.Zero;

            foreach (IExpression Member in Members)
            {
                Result += Member.GetValue();
            }
            return Result;
        }

        public IExpression Add(IExpression Member)
        {
            Members.Add(Member);
            return Member;
        }


        public void RemoveAt(int Index) => Members.RemoveAt(Index);
        public void Clear() => Members.Clear();
        public bool Remove(IExpression Item) => Members.Remove(Item);
        public bool Contains(IExpression Item) => Members.Contains(Item);
        public int IndexOf(IExpression Item) => Members.IndexOf(Item);

        public void Insert(int Index, IExpression Item) => Members.Insert(Index, Item);
        public void CopyTo(IExpression[] Array, int ArrayIndex) => Members.CopyTo(Array, ArrayIndex);



        public IEnumerator<IExpression> GetEnumerator() => Members.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Members.GetEnumerator();
        void ICollection<IExpression>.Add(IExpression Item) => Members.Add(Item);
    }
}