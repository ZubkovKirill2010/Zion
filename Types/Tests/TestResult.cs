using System.Collections;
using System.Text;

namespace Zion
{
    public readonly struct TestResult<TIn, TOut>
    {
        public readonly TIn In;
        public readonly TOut? Out;
        public readonly TOut TargetOut;

        public readonly Exception? Exception;

        public TestResult(TIn In, TOut TargetOut, TOut Out)
        {
            this.In = In;
            this.TargetOut = TargetOut;
            this.Out = Out;
        }
        public TestResult(TIn In, TOut TargetOut, Exception? Exception)
        {
            this.In = In;
            this.TargetOut = TargetOut;
            this.Exception = Exception;
        }

        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();

            string Separator = new string('-', 44);

            Builder.AppendLine($"In:                | {ToString(In)}");
            Builder.AppendLine(Separator);

            Builder.AppendLine($"TargetOut:         | {ToString(TargetOut)}");
            Builder.AppendLine(Separator);

            if (Out is not null)
            {
                Builder.AppendLine($"Result:            | {ToString(Out)}");
            }
            else
            {
                Builder.AppendLine($"Result(Exception): | {ToString(Exception)}");
            }


            if (Out is ICollection<object> Collection)
            {

            }

            return Builder.ToString();
        }

        private static string ToString<T>(T Object)
        {
            if (Object is null)
            {
                return "null";
            }

            if (Object is string)
            {
                return $"\"{Object.ToString() ?? "null"}\"";
            }

            if (Object is Exception Error)
            {
                string Offset = '\n' + new string(' ', 19) + "| ";

                return
@$"{Error.GetType().FullName};
    Message: {Error.Message};
    Source: {Error.Source};
    Data: {Error.Data};
    StackTrace: {Error.StackTrace};".Replace("\n", Offset);
            }

            if (Object is IEnumerable Enumerable)
            {
                return $"{Object.GetType().FullName} {{ {string.Join(", ", Enumerable.Cast<object>())} }}";
            }
            else
            {
                return Object?.ToString() ?? "Object.ToString() => null";
            }
        }
    }
}