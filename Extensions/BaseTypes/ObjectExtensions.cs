using System.Reflection;

namespace Zion
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Checks if the object's type contains the specified attribute.
        /// </summary>
        /// <param name="Object">The object to check.</param>
        /// <param name="Attribute">The attribute type to look for.</param>
        /// <returns>True if the attribute exists, otherwise false.</returns>
        public static bool ContainsAttribute(this object Object, object Attribute)
        {
            return Object.GetType().GetCustomAttribute<Attribute>() is not null;
        }

        /// <summary>
        /// Checks if the type contains the specified attribute.
        /// </summary>
        /// <param name="Type">The type to check.</param>
        /// <param name="Attribute">The attribute type to look for.</param>
        /// <returns>True if the attribute exists, otherwise false.</returns>
        public static bool ContainsAttribute(this Type Type, object Attribute)
        {
            return Type.GetCustomAttribute<Attribute>() is not null;
        }

        /// <summary>
        /// Checks if the value is within the specified range (inclusive).
        /// </summary>
        /// <typeparam name="T">The type of value to compare.</typeparam>
        /// <param name="Value">The value to check.</param>
        /// <param name="Min">The minimum bound of the range.</param>
        /// <param name="Max">The maximum bound of the range.</param>
        /// <returns>True if the value is within bounds, otherwise false.</returns>
        public static bool IsClamp<T>(this T Value, T Min, T Max) where T : IComparable
        {
            return Value.CompareTo(Min) >= 0 && Value.CompareTo(Max) <= 0;
        }
    }
}