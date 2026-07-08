using Zion.FileSystem;

namespace Zion
{
    public sealed class InvalidFileNameException : ArgumentException
    {
        public readonly string? FileName;

        public InvalidFileNameException(string? FileName)
            : base($"FileName '{FileName ?? "null"}' is invalid")
        {
            this.FileName = FileName;
        }

        public static void ThrowIfInvalid(string FileName)
        {
            if (FileName is null || FilePath.IsInvalidFileName(FileName))
            {
                throw new InvalidFileNameException(FileName);
            }
        }
    }
}