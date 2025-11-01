namespace Zion.MathExpressions
{
    public interface IMathTerm
    {
        Fraction GetValue() => GetValue(10);

        abstract Fraction GetValue(int Accuracy);

        virtual IMathTerm Simplify() => this;
    }
}