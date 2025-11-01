namespace Zion.MathExpressions
{
    public sealed class Amount : IFraction
    {
        private readonly List<IFraction> Members;

        public Amount()
        {
            Members = new List<IFraction>(3);
        }

        public Fraction GetValue(int Accuracy)
        {
            return Fraction.Sum(List.Convert(Members, Value => Value.GetValue(Accuracy)));
        }

        public T Add<T>(T Member) where T : IFraction
        {
            Members.Add(Member);
            return Member;
        }
    }
}