namespace Zion
{
    public static class Accessor
    {
        public static T Set<T>(out T Target, T Value)
        {
            Target = Value;
            return Target;
        }
        public static T SetByRef<T>(ref T Target, T Value)
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

        public static T SetIfNotNull<T>(ref T Target, T? Value)
        {
            if (Value is not null)
            {
                Target = Value;
            }
            return Target;
        }

        public static T Out<T>(this T Target, out T Value)
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

        public static void Sort<T>(ref T Min, ref T Max) where T : IComparable<T>
        {
            if (int.IsPositive(Min.CompareTo(Max)))
            {
                Reverse(ref Min, ref Max);
            }
        }


        public static I AddAndReturn<T, I>(this T Collection, I Item) where T : ICollection<I>
        {
            Collection.Add(Item);
            return Item;
        }
        public static T InsertAndReturn<T, I>(this T List, int Index, I Item) where T : IList<I>
        {
            List.Insert(Index, Item);
            return List;
        }

        public static T RemoveAndReturn<T, I>(this T List, I Item) where T : ICollection<I>
        {
            List.Remove(Item);
            return List;
        }
        public static T RemoveAtAndReturn<T, I>(this T List, int Index) where T : IList<I>
        {
            List.RemoveAt(Index);
            return List;
        }
    }
}