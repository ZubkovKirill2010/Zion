namespace Zion
{
    public sealed class IntegerFormatException : FormatException
    {
        public enum FormatError
        {
            IncorrectFormat, CantBeNegative, MemoryLimitExceeded
        }

        public readonly FormatError Error;
        public readonly Type TargetType;

        public IntegerFormatException(Type TargetType)
            : this(FormatError.IncorrectFormat, TargetType) { }

        public IntegerFormatException(FormatError Error, Type TargetType)
        {
            this.Error = Error;
            this.TargetType = TargetType;
        }
    }
}