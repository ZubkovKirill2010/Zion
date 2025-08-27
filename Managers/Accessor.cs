namespace Zion
{
    public static class Accessor
    {
        public static T Set<T>(out T Target, T Value)
        {
            Target = Value;
            return Target;
        }

        public static T SetIf<T>(ref T Target, T Value, bool Condition)
        {
            if (Condition)
            {
                Target = Value;
            }
            return Target;
        }
        public static T SetIf<T>(ref T Target, T Value, Func<T, bool> Condition)
        {
            if (Condition(Target))
            {
                Target = Value;
            }
            return Target;
        }

        public static T Out<T>(T Target, out T Value)
        {
            Value = Target;
            return Target;
        }

        public static void Reverse<T>(ref T A, ref T B)
        {
            T Temp = A;
            A = B;
            B = Temp;
        }
        public static void Reverse<T>(ref T A, ref T B, ref T Temp)
        {
            Temp = A;
            A = B;
            B = Temp;
        }
    }
}