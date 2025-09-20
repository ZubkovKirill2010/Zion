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
        /// Value >= Min && Value <= Max
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

        /// <summary>
        /// Checks if the value is within the specified range (not inclusive).
        /// Value >= Min && Value < Max
        /// </summary>
        /// <typeparam name="T">The type of value to compare.</typeparam>
        /// <param name="Value">The value to check.</param>
        /// <param name="Min">The minimum bound of the range.</param>
        /// <param name="Max">The maximum bound of the range.</param>
        /// <returns>True if the value is within bounds, otherwise false.</returns>
        public static bool IsInRange<T>(this T Value, T Min, T Max) where T : IComparable<T>
        {
            return Value.CompareTo(Min) >= 0 && Value.CompareTo(Max) < 0;
        }


        /// <summary>
        /// Converts an object to a colored string representation if it implements IColorText,
        /// otherwise returns the standard string representation.
        /// </summary>
        /// <param name="Object">The object to convert to string (can be null)</param>
        /// <param name="Nullable">The string to return if the object is null (default: "null")</param>
        /// <returns>
        /// Colored text if object implements IColorText, standard string representation otherwise,
        /// or the nullable replacement string for null objects.
        /// </returns>
        /// <remarks>
        /// If the object implements IColorText, the method calls its ToColorString() implementation.
        /// For non-IColorText objects, it falls back to standard ToString() behavior.
        /// </remarks>
        public static string ToColorString(this object? Object, string Nullable = "null")
        {
            return Object is IColorText Colorable
                ? Object?.ToColorString() ?? Nullable
                : ToNotNullString(Object, Nullable);
        }


        /// <summary>
        /// Converts an object to its string representation, ensuring a non-null result.
        /// Uses "null" as the default replacement for null objects.
        /// </summary>
        /// <param name="Object">The object to convert to string (can be null)</param>
        /// <returns>
        /// The object's string representation or "null" if the object is null.
        /// </returns>
        public static string ToNotNullString(this object? Object)
        {
            return ToNotNullString(Object, "null");
        }

        /// <summary>
        /// Converts an object to its string representation with a custom replacement for null objects.
        /// </summary>
        /// <param name="Object">The object to convert to string (can be null)</param>
        /// <param name="Nullable">The custom string to return if the object is null</param>
        /// <returns>
        /// The object's string representation or the specified nullable replacement string.
        /// </returns>
        public static string ToNotNullString(this object? Object, string Nullable)
        {
            return Object?.ToString() ?? string.Empty;
        }
    }
}