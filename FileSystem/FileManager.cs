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

            string DirectoryPath = Path.GetDirectoryName(FilePath);
            string NewFilePath = Path.Combine(DirectoryPath, NewName);

            File.Move(FilePath, NewFilePath);
        }

        public static void Rename(string FilePath, string NewName, bool Overwrite)
        {
            FileNotFoundException.ThrowIfNotExists(FilePath);
            InvalidFileNameException.ThrowIfInvalid(NewName);

            string DirectoryPath = Path.GetDirectoryName(FilePath);
            string NewFilePath = Path.Combine(DirectoryPath, NewName);

            File.Move(FilePath, NewFilePath, Overwrite);
        }

        public static void SetExtension(string FilePath, string NewExtension, bool Overwrite = true)
        {
            ArgumentNullException.ThrowIfNull(NewExtension);
            FileNotFoundException.ThrowIfNotExists(FilePath);

            if (NewExtension.Length == 0)
            {
                throw new ArgumentException("New file extension cannot be empty", nameof(NewExtension));
            }

            if (NewExtension == ".")
            {
                throw new ArgumentException("New file extension cannot be just a dot", nameof(NewExtension));
            }

            if (FileSystem.FilePath.IsInvalidFileName(NewExtension))
            {
                throw new ArgumentException($"New file extension '{NewExtension}' contains invalid characters", nameof(NewExtension));
            }

            string DirectoryPath = Path.GetDirectoryName(FilePath);
            string FileNameWithoutExtension = Path.GetFileNameWithoutExtension(FilePath);
            string NewFileName = FileNameWithoutExtension + NewExtension.EnsurePrefix(".");
            string NewFilePath = Path.Combine(DirectoryPath, NewFileName);

            File.Move(FilePath, NewFilePath, Overwrite);
        }

        public static bool MoveToRecycleBin(string FilePath, bool ShowDialog = false)
        {
            try
            {
                Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(
                    FilePath,
                    UIOption.OnlyErrorDialogs,
                    RecycleOption.SendToRecycleBin,
                    ShowDialog ? UICancelOption.ThrowException : UICancelOption.DoNothing);

                return true;
            }
            catch (OperationCanceledException)
            {
                return false;
            }
        }

        public static bool MoveDirectoryToRecycleBin(string DirectoryPath, bool ShowDialog = false)
        {
            try
            {
                Microsoft.VisualBasic.FileIO.FileSystem.DeleteDirectory(
                    DirectoryPath,
                    UIOption.OnlyErrorDialogs,
                    RecycleOption.SendToRecycleBin,
                    ShowDialog ? UICancelOption.ThrowException : UICancelOption.DoNothing);

                return true;
            }
            catch (OperationCanceledException)
            {
                return false;
            }
        }

        public static void CopyDirectory(string SourceDirectoryPath, string DestinationDirectoryPath, Action<ProgressInfo>? UpdateProgress = null)
        {
            CopyDirectoryAsync(SourceDirectoryPath, DestinationDirectoryPath, UpdateProgress)
                .GetAwaiter()
                .GetResult();
        }

        public static async Task CopyDirectoryAsync(string SourceDirectoryPath, string DestinationDirectoryPath, Action<ProgressInfo>? UpdateProgress = null)
        {
            ArgumentNullException.ThrowIfNull(SourceDirectoryPath);
            ArgumentNullException.ThrowIfNull(DestinationDirectoryPath);

            if (!Directory.Exists(SourceDirectoryPath))
            {
                throw new DirectoryNotFoundException($"Source directory not found: {SourceDirectoryPath}");
            }

            List<string> AllFiles = Directory
                .EnumerateFiles(SourceDirectoryPath, "*", SearchOption.AllDirectories)
                .ToList();

            List<string> AllDirectories = Directory
                .EnumerateDirectories(SourceDirectoryPath, "*", SearchOption.AllDirectories)
                .ToList();

            foreach (string CurrentDirectory in AllDirectories)
            {
                string RelativePath = Path.GetRelativePath(SourceDirectoryPath, CurrentDirectory);
                string NewDirectoryPath = Path.Combine(DestinationDirectoryPath, RelativePath);
                Directory.CreateDirectory(NewDirectoryPath);
            }

            long TotalBytes = AllFiles.Sum(FilePath => new FileInfo(FilePath).Length);
            long CopiedBytes = 0;

            ParallelOptions ParallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            if (UpdateProgress is null)
            {
                await Parallel.ForEachAsync(
                    AllFiles,
                    ParallelOptions,
                    async (CurrentFile, CancellationToken) =>
                    {
                        string RelativePath = Path.GetRelativePath(SourceDirectoryPath, CurrentFile);
                        string DestinationFilePath = Path.Combine(DestinationDirectoryPath, RelativePath);

                        await CopyFileAsync(CurrentFile, DestinationFilePath);
                    });
            }
            else
            {
                await Parallel.ForEachAsync(
                    AllFiles,
                    ParallelOptions,
                    async (CurrentFile, CancellationToken) =>
                    {
                        string RelativePath = Path.GetRelativePath(SourceDirectoryPath, CurrentFile);
                        string DestinationFilePath = Path.Combine(DestinationDirectoryPath, RelativePath);

                        await CopyFileAsync(
                            CurrentFile,
                            DestinationFilePath,
                            (BytesCopied) =>
                            {
                                long NewTotalCopied = Interlocked.Add(ref CopiedBytes, BytesCopied);
                                ProgressInfo Progress = new ProgressInfo(TotalBytes, NewTotalCopied);
                                UpdateProgress(Progress);
                            });
                    });
            }
        }

        public static void CopyFile(string SourceFilePath, string DestinationFilePath, Action<long>? OnBytesCopied = null)
        {
            CopyFileAsync(SourceFilePath, DestinationFilePath, OnBytesCopied)
                .GetAwaiter()
                .GetResult();
        }

        public static async Task CopyFileAsync(string SourceFilePath, string DestinationFilePath, Action<long>? OnBytesCopied = null)
        {
            ArgumentNullException.ThrowIfNull(SourceFilePath);
            ArgumentNullException.ThrowIfNull(DestinationFilePath);

            if (!File.Exists(SourceFilePath))
            {
                throw new FileNotFoundException($"Source file not found: {SourceFilePath}");
            }

            const int BufferSize = 81920;

            using FileStream SourceStream = new FileStream(
                SourceFilePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                BufferSize,
                useAsync: true);

            using FileStream DestinationStream = new FileStream(
                DestinationFilePath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                BufferSize,
                useAsync: true);

            byte[] Buffer = new byte[BufferSize];
            int BytesRead;

            while ((BytesRead = await SourceStream.ReadAsync(Buffer, 0, Buffer.Length)) > 0)
            {
                await DestinationStream.WriteAsync(Buffer, 0, BytesRead);
                OnBytesCopied?.Invoke(BytesRead);
            }

            await DestinationStream.FlushAsync();
        }

        public static async Task CopyFileAsync(
            string SourceFilePath,
            string DestinationFilePath,
            Action<long>? OnBytesCopied,
            CancellationToken CancellationToken)
        {
            ArgumentNullException.ThrowIfNull(SourceFilePath);
            ArgumentNullException.ThrowIfNull(DestinationFilePath);

            if (!File.Exists(SourceFilePath))
            {
                throw new FileNotFoundException($"Source file not found: {SourceFilePath}");
            }

            const int BufferSize = 81920;

            using FileStream SourceStream = new FileStream(
                SourceFilePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                BufferSize,
                useAsync: true);

            using FileStream DestinationStream = new FileStream(
                DestinationFilePath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                BufferSize,
                useAsync: true);

            byte[] Buffer = new byte[BufferSize];
            int BytesRead;

            while ((BytesRead = await SourceStream.ReadAsync(Buffer, 0, Buffer.Length, CancellationToken)) > 0)
            {
                await DestinationStream.WriteAsync(Buffer, 0, BytesRead, CancellationToken);
                OnBytesCopied?.Invoke(BytesRead);
            }

            await DestinationStream.FlushAsync(CancellationToken);
        }

        public static string FormatBytesHumanReadable(long Bytes)
        {
            string[] SizeSuffixes = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };

            if (Bytes == 0)
            {
                return "0 B";
            }

            int SuffixIndex = 0;
            double DisplaySize = Bytes;

            while (DisplaySize >= 1024 && SuffixIndex < SizeSuffixes.Length - 1)
            {
                DisplaySize /= 1024;
                SuffixIndex++;
            }

            return $"{DisplaySize:0.##} {SizeSuffixes[SuffixIndex]}";
        }

        public static string GetFileSizeHumanReadable(string FilePath)
        {
            ArgumentNullException.ThrowIfNull(FilePath);
            FileNotFoundException.ThrowIfNotExists(FilePath);

            FileInfo FileInformation = new FileInfo(FilePath);
            long FileSizeInBytes = FileInformation.Length;

            return FormatBytesHumanReadable(FileSizeInBytes);
        }

        public static bool IsFileHidden(string FilePath)
        {
            ArgumentNullException.ThrowIfNull(FilePath);
            FileNotFoundException.ThrowIfNotExists(FilePath);

            FileInfo FileInformation = new FileInfo(FilePath);
            return (FileInformation.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
        }

        public static bool IsDirectoryHidden(string DirectoryPath)
        {
            ArgumentNullException.ThrowIfNull(DirectoryPath);

            if (!Directory.Exists(DirectoryPath))
            {
                throw new DirectoryNotFoundException($"Directory not found: {DirectoryPath}");
            }

            DirectoryInfo DirectoryInformation = new DirectoryInfo(DirectoryPath);
            return (DirectoryInformation.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
        }
    }
}