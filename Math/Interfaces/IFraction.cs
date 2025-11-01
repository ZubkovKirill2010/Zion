namespace Zion.MathExpressions
{
    public interface IFraction
    {
        public Fraction GetValue() => GetValue(10);

        public abstract Fraction GetValue(int Accuracy);
    }
}