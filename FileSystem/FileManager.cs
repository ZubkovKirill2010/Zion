using Microsoft.VisualBasic.FileIO;
using SearchOption = System.IO.SearchOption;

namespace Zion.FileSystem
{
    public static class FileManager
    {
        public static void Rename(string FilePath, string NewName)
        {
            FileNotFoundException.ThrowIfNotExists(FilePath);
            InvalidFileNameException.ThrowIfInvalid(NewName);

            string NewFilePath = Path.GetDirectoryName(FilePath) + NewName;
            File.Move(FilePath, NewFilePath);
            File.Delete(FilePath);
        }

        public static void SetExtension(string FilePath, string NewExtension, bool Overwrite = true)
        {
            ArgumentNullException.ThrowIfNull(NewExtension);
            FileNotFoundException.ThrowIfNotExists(FilePath);

            if (NewExtension.Length == 0
                || NewExtension == "."
                || FileSystem.FilePath.IsInvalidFileName(NewExtension))
            {
                throw new ArgumentException($"New file extension '{NewExtension}' is invalid");
            }

            string FilePathWithoutExtension = Path.GetFileNameWithoutExtension(FilePath);
            string NewFilePath = FilePathWithoutExtension + NewExtension.EnsurePrefix(".");

            File.Move(FilePath, NewFilePath, Overwrite);
            File.Delete(FilePath);
        }


        public static bool MoveToRecycleBin(string Path, bool ShowDialog = false)
        {
            try
            {
                Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(Path,
                    UIOption.OnlyErrorDialogs,
                    ShowDialog ? RecycleOption.SendToRecycleBin : RecycleOption.SendToRecycleBin,
                    UICancelOption.ThrowException);
                return true;
            }
            catch (OperationCanceledException)
            {
                return false;
            }
            catch (Exception Exception)
            {
                throw new Exception($"Ошибка при удалении в корзину: {Exception.Message}", Exception);
            }
        }

        public static bool MoveDirectoryToRecycleBin(string Path, bool ShowDialog = false)
        {
            try
            {
                Microsoft.VisualBasic.FileIO.FileSystem.DeleteDirectory(Path,
                    UIOption.OnlyErrorDialogs,
                    ShowDialog ? RecycleOption.SendToRecycleBin : RecycleOption.SendToRecycleBin,
                    UICancelOption.ThrowException);
                return true;
            }
            catch (OperationCanceledException)
            {
                return false;
            }
        }


        public static void CopyDirectory(string DirectoryPath, string NewDirectoryPath, Action<ProgressInfo>? UpdateProgress = null)
        {
            CopyDirectoryAsync(DirectoryPath, NewDirectoryPath).GetAwaiter().GetResult();
        }

        public static async Task CopyDirectoryAsync(string DirectoryPath, string NewDirectoryPath, Action<ProgressInfo>? UpdateProgress = null)
        {
            ArgumentNullException.ThrowIfNull(DirectoryPath);
            ArgumentNullException.ThrowIfNull(NewDirectoryPath);

            List<string> Files = Directory.EnumerateFiles(DirectoryPath, "*", SearchOption.AllDirectories).ToList();
            List<string> Directories = Directory.EnumerateDirectories(DirectoryPath, "*", SearchOption.AllDirectories).ToList();

            foreach (string CurrentDirectory in Directories)
            {
                string RelativePath = Path.GetRelativePath(DirectoryPath, CurrentDirectory);
                string NewDirectory = Path.Combine(NewDirectoryPath, RelativePath);
                Directory.CreateDirectory(NewDirectory);
            }

            long TotalBytes = Files.Sum(File => new FileInfo(File).Length);
            long CopiedBytes = 0;

            ParallelOptions Options = new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            Action<string, string> CopyFile = UpdateProgress is null
            ? async (Source, Destination) => await CopyFileAsync(Source, Destination)
            : async (Source, Destination) => await CopyFileAsync(Source, Destination, Bytes =>
            {
                long NewCopied = Interlocked.Add(ref CopiedBytes, Bytes);
                UpdateProgress(new ProgressInfo(TotalBytes, NewCopied));
            });

            await Parallel.ForEachAsync(Files, Options, async (File, Token) =>
            {
                string RelativePath = Path.GetRelativePath(DirectoryPath, File);
                string NewFile = Path.Combine(NewDirectoryPath, RelativePath);
                CopyFile(File, NewFile);
            });
        }


        public static void CopyFile(string Source, string Destination, Action<long>? OnBytesCopied = null)
        {
            CopyFileAsync(Source, Destination).GetAwaiter().GetResult();
        }

        public static async Task CopyFileAsync(string Source, string Destination, Action<long>? OnBytesCopied = null)
        {
            ArgumentNullException.ThrowIfNull(Source);
            ArgumentNullException.ThrowIfNull(Destination);

            const int BufferSize = 81920;
            using FileStream SourceStream = new FileStream(Source, FileMode.Open, FileAccess.Read);
            using FileStream DestinationStream = new FileStream(Destination, FileMode.Create, FileAccess.Write);

            byte[] Buffer = new byte[BufferSize];
            int BytesRead;

            while ((BytesRead = await SourceStream.ReadAsync(Buffer, 0, Buffer.Length)) > 0)
            {
                await DestinationStream.WriteAsync(Buffer, 0, BytesRead);
                OnBytesCopied?.Invoke(BytesRead);
            }
        }
    }
}