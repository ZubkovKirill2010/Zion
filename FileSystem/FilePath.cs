namespace Zion.FileSystem
{
    public static class FilePath
    {
        private static readonly HashSet<char> InvalidFileNameChars = Path.GetInvalidFileNameChars().ToHashSet();
        private static readonly HashSet<char> InvalidPathChars = Path.GetInvalidPathChars().ToHashSet();


        public static bool IsInvalidFileNameChar(char Char)
        {
            return InvalidFileNameChars.Contains(Char);
        }

        public static bool IsInvalidPathChar(char Char)
        {
            return InvalidPathChars.Contains(Char);
        }


        public static bool IsInvalidFileName(string FileName)
        {
            return FileName.Contains(InvalidFileNameChars);
        }

        public static bool IsInvalidPath(string Path)
        {
            return Path.Contains(InvalidPathChars);
        }


        /// <summary>
        /// Generates a unique file path by appending an index if the file already exists.
        /// </summary>
        /// <param name="Directory">The target directory path.</param>
        /// <param name="FileName">The base file name without extension.</param>
        /// <param name="Extension">The file extension including the dot (e.g. ".txt").</param>
        /// <returns>A unique full file path in format: "{Directory}\{FileName}({n}){Extension}".</returns>
        public static string GetUniqueFilePath(string Directory, string FileName, string Extension)
        {
            string FilePath = Path.Combine(Directory, FileName + Extension);
            int FileIndex = 1;

            while (File.Exists(FilePath))
            {
                FilePath = Path.Combine(Directory, $"{FileName}({FileIndex++}){Extension}");
            }
            return FilePath;
        }

        /// <summary>
        /// Generates a unique file name by appending an index if the file already exists.
        /// </summary>
        /// <param name="Directory">The target directory path.</param>
        /// <param name="FileName">The base file name without extension.</param>
        /// <param name="Extension">The file extension including the dot (e.g. ".txt").</param>
        /// <returns>A unique file name in format: "{FileName}({n})".</returns>
        public static string GetUniqueFileName(string Directory, string FileName, string Extension)
        {
            string FilePath = Path.Combine(Directory, FileName + Extension);
            int FileIndex = 0;

            while (File.Exists(FilePath))
            {
                FileIndex++;
                FilePath = Path.Combine(Directory, $"{FileName}({FileIndex}){Extension}");
            }
            return $"{FileName}{(FileIndex > 0 ? $"({FileIndex})" : "")}";
        }
    }
}