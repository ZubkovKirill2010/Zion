using Zion.FileSystem;
using Zion.Vectors;

namespace Zion
{
    public static class ExceptionExtensions
    {
        extension(Exception Exception)
        {
            public static void ThrowIf(bool Condition, string Message)
            {
                if (Condition)
                {
                    throw new Exception(Message);
                }
            }
        }

        extension(ArgumentOutOfRangeException)
        {
            public static void ThrowIfWithout(int Index, int Min, int Max)
            {
                if (Index < Min || Index >= Max)
                {
                    throw new ArgumentOutOfRangeException($"Index(={Index}) out of range [{Min}..{Max})");
                }
            }

            public static void ThrowIfWithout(int Index, string String)
            {
                if (int.IsNegative(Index) || Index >= String.Length)
                {
                    throw new ArgumentOutOfRangeException($"Index(={Index}) out of string");
                }
            }

            public static void ThrowIfWithout(int Index, int Count)
            {
                if (Index < 0 || Index >= Count)
                {
                    throw new ArgumentOutOfRangeException($"Index(={Index}) out of range [0..{Count})");
                }
            }

            public static void ThrowIfWithout<T>(int Index, ICollection<T> Collection)
            {
                if (Index < 0 || Index >= Collection.Count)
                {
                    throw new ArgumentOutOfRangeException($"Index(={Index}) out of range [0..{Collection.Count})");
                }
            }

            public static void ThrowIfWithout<T>(int Index, T[] Array)
            {
                if (Index < 0 || Index >= Array.Length)
                {
                    throw new ArgumentOutOfRangeException($"Index(={Index}) out of range [0..{Array.Length})");
                }
            }


            public static void ThrowIfWithout<T>(int x, int y, IMatrix<T> Matrix)
            {
                if (!Matrix.IsInside(x, y))
                {
                    throw new ArgumentOutOfRangeException($"Position [{x}, {y}] out of range [[0; 0]..{Matrix.Size})");
                }
            }

            public static void ThrowIfWithout<T>(Vector2Int Position, IMatrix<T> Matrix)
            {
                if (!Matrix.IsInside(Position))
                {
                    throw new ArgumentOutOfRangeException($"Position {Position} out of range [[0; 0]..{Matrix.Size})");
                }
            }


            public static void ThrowIfWithout<T>(T Index, T Min, T Max) where T : IComparable<T>
            {
                if (!Index.IsInRange(Min, Max))
                {
                    throw new ArgumentOutOfRangeException($"Index(={Index}) out of range [{Min}..{Max})");
                }
            }
        }

        extension(FileNotFoundException)
        {
            public static void ThrowIfNotExists(string FilePath)
            {
                FileNotFoundException.ThrowIf
                (
                    !File.Exists(FilePath.NotNull()),
                    $"File '{FilePath}' not exists"
                );
            }
        }

        extension(InvalidFileNameException)
        {
            public static void ThrowIfInvalid(string FileName)
            {
                InvalidFileNameException.ThrowIf(FilePath.IsInvalidFileName(FileName.NotNull()), $"FileName '{FileName}' is invalid");
            }
        }
    }
}