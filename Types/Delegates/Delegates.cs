namespace Zion
{
    public delegate bool TryParser<T>(string String, out T Value);

    public delegate Task AsyncAction();
    public delegate Task AsyncAction<in T>(T T1);
    public delegate Task AsyncAction<in T1, in T2>(T1 T1, T2 T2);
    public delegate Task AsyncAction<in T1, in T2, in T3>(T1 T1, T2 T2, T3 T3);
    public delegate Task AsyncAction<in T1, in T2, in T3, in T4>(T1 T1, T2 T2, T3 T3, T4 T4);
    public delegate Task AsyncAction<in T1, in T2, in T3, in T4, in T5>(T1 T1, T2 T2, T3 T3, T4 T4, T5 T5);
    public delegate Task AsyncAction<in T1, in T2, in T3, in T4, in T5, in T6>(T1 T1, T2 T2, T3 T3, T4 T4, T5 T5, T6 T6);
}