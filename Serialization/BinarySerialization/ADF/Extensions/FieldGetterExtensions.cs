using System.Linq.Expressions;
using System.Reflection;

namespace Zion.Serialization.ADF
{
    public static class FieldGetterExtensions
    {
        extension(FieldGetter Getter)
        {
            public static FieldGetter Create(FieldInfo Info)
            {
                var Parameter = Expression.Parameter(typeof(object), "obj");
                var CastObject = Expression.Convert(Parameter, Info.DeclaringType);
                var AccessField = Expression.Field(CastObject, Info);
                var CastResult = Expression.Convert(AccessField, typeof(object));

                return Expression.Lambda<FieldGetter>(CastResult, Parameter).Compile();
            }
        }
    }
}