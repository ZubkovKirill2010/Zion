namespace Zion
{
    public static class FilePath
    {
        public static string GetUniqueFilePath(string Directory, string FileName, string Extension)
        {
            string FilePath = Path.Combine(Directory, FileName + Extension);
            int FileIndex = 1;

            while (File.Exists(FilePath))
            {
                FilePath = $"{Directory}\\{FileName}({FileIndex}){Extension}";
                FileIndex++;
            }
            return FilePath;
        }
        public static string GetUniqueFileName(string Directory, string FileName, string Extension)
        {
            string FilePath = Path.Combine(Directory, FileName + Extension);
            int FileIndex = 1;

            while (File.Exists(FilePath))
            {
                FilePath = $"{Directory}\\{FileName}({FileIndex++}){Extension}";
            }
            return $"{FileName}({FileIndex})";
        }
    }
}