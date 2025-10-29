using System.Collections;
using System.Text;

namespace Zion.Diagnostics
{
    public enum FunctionResult : byte { Normal, Exception, InfinityCycle }

    public readonly struct TestResult<TIn, TOut>
    {
        public readonly FunctionResult OutType;

        public readonly TIn In;
        public readonly TOut? Out;
        public readonly TOut TargetOut;

        public readonly TimeSpan Time;
        public readonly Exception? Exception;

        public bool InfinityCycle => OutType == FunctionResult.InfinityCycle;

        public TestResult(TIn In, TOut TargetOut)
        {
            OutType = FunctionResult.InfinityCycle;
            this.In = In;
            this.TargetOut = TargetOut;
        }
        public TestResult(TIn In, TOut TargetOut, TimeSpan Time, TOut Out)
        {
            OutType = FunctionResult.Normal;
            this.In = In;
            this.TargetOut = TargetOut;
            this.Out = Out;
            this.Time = Time;
        }
        public TestResult(TIn In, TOut TargetOut, TimeSpan Time, Exception? Exception)
        {
            OutType = FunctionResult.Exception;
            this.In = In;
            this.TargetOut = TargetOut;
            this.Exception = Exception;
            this.Time = Time;
        }

        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();

            string Separator = new string('-', 44);

            Builder.AppendLine($"In:                | {ToString(In)}");
            Builder.AppendLine(Separator);

            Builder.AppendLine($"TargetOut:         | {ToString(TargetOut)}");
            Builder.AppendLine(Separator);

            if (InfinityCycle)
            {
                Builder.AppendLine("Result:            | Infinity loop");
                return Builder.ToString();
            }

            Builder.AppendLine($"Time:              | {Time.Milliseconds} ms | {Time.Ticks} ticks");
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