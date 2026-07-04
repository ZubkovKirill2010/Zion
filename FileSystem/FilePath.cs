using System.Runtime.InteropServices;

namespace Zion.FileSystem
{
    public static class FilePath
    {
        private static readonly HashSet<char> InvalidPathChars = Path.GetInvalidPathChars().ToHashSet();
        private static readonly HashSet<char> InvalidFileNameChars = Path.GetInvalidFileNameChars()
            .Union(new[] { '/', '\\', ':', '*', '?', '"', '<', '>', '|' })
            .ToHashSet();

        private static readonly HashSet<string> ReservedWindowsNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "CON", "PRN", "AUX", "NUL",
            "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
            "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9"
        };


        public static bool IsInvalidFileNameChar(char Char)
        {
            return InvalidFileNameChars.Contains(Char) || Char < 32;
        }

        public static bool IsInvalidPathChar(char Char)
        {
            return InvalidPathChars.Contains(Char);
        }

        public static bool IsInvalidFileName(string FileName)
        {
            return string.IsNullOrWhiteSpace(FileName)
                || FileName.Length > 255
                || FileName.Any(IsInvalidFileNameChar)
                || (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    && (
                        FileName.EndsWith(' ')
                        || FileName.EndsWith('.')
                        || ReservedWindowsNames.Contains(Path.GetFileNameWithoutExtension(FileName))
                       )
                   );
        }


        public static string GetUniqueFilePath(string Directory, string FileName, string Extension)
        {
            if (!string.IsNullOrEmpty(Extension))
            {
                Extension = Extension.EnsurePrefix(".");
            }

            string FilePath = Path.Combine(Directory, FileName + Extension);
            int FileIndex = 1;

            while (File.Exists(FilePath))
            {
                FilePath = Path.Combine(Directory, $"{FileName}({FileIndex++}){Extension}");
            }
            return FilePath;
        }

        public static string GetUniqueFileName(string Directory, string FileName, string Extension)
        {
            if (!string.IsNullOrEmpty(Extension))
            {
                Extension = Extension.EnsurePrefix(".");
            }

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
