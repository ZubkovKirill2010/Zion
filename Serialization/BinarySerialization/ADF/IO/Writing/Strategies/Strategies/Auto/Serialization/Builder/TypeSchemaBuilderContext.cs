using System.Linq.Expressions;

namespace Zion.Serialization.ADF
{
    internal readonly struct TypeSchemaBuilderContext<T>
    {
        public readonly ParameterExpression ContextParameter = Expression.Parameter(typeof(ADFWritingContext), "Context");
        public readonly ParameterExpression TargetParameter = Expression.Parameter(typeof(StreamGroup), "Target");
        public readonly ParameterExpression ValueParameter = Expression.Parameter(typeof(T), "Value");

        public readonly HashSet<string> FieldNames;

        public TypeSchemaBuilderContext(int Capacity)
        {
            FieldNames = new(Capacity);
        }
    }
}