namespace Zion
{
    public static class FilePath
    {
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