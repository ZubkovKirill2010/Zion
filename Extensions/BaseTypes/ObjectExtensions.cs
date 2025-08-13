using System.Reflection;

namespace Zion
{
    public static class ObjectExtensions
    {
        public static bool ContainsAttribute(this object Object, object Attribute)
        {
            return Object.GetType().GetCustomAttribute<Attribute>() is not null;
        }
        public static bool ContainsAttribute(this Type Type, object Attribute)
        {
            return Type.GetCustomAttribute<Attribute>() is not null;
        }

        public static bool IsClamp<T>(this T Value, T Min, T Max) where T : IComparable
        {
            return Value.CompareTo(Min) >= 0 && Value.CompareTo(Max) <= 0;
        }
    }
}